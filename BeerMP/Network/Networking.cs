using BeerMP.Components;
using MSCLoader;
using Steamworks;
using System;
using UnityEngine;

namespace BeerMP.Network
{
	/// <summary> Underlying network manager responsible for session management, remote connections, and routing. </summary>
	public static class Networking
	{
		/// <summary> Current version of the networking protocol. </summary>
		public const int PROTOCOL_VERSION = 1;
		/// <summary> Maximum number of concurrent connections allowed at once. </summary>
		public const int MAX_CONCURRENT_CONNECTIONS = 12;
		/// <summary> Conservative MTU size to avoid fragmentation. </summary>
		public const int MAX_MESSAGE_SIZE = 1024;

		/// <summary> Indicates whether Steamworks interfaces are available. Can't do anything without them. </summary>
		public static bool IsValid { get; private set; }

		/// <summary> Current lobby we're in, if any. </summary>
		public static CSteamID CurrentLobby { get; private set; }
		/// <summary> Whether we're the host of the current session. </summary>
		public static bool IsHost { get; private set; }
		/// <summary> Whether we're a client connected to the current session. </summary>
		public static bool IsClient => !IsHost && InSession;
		/// <summary> Whether we're currently in a session. </summary>
		public static bool InSession => CurrentLobby.IsValid() && CurrentLobby.IsLobby();

		// Steamworks callbacks and call results.
		private static CallResult<LobbyCreated_t> crLobbyCreated;
		private static CallResult<LobbyEnter_t> crLobbyEntered;
		private static Callback<LobbyChatUpdate_t> cbLobbyChatUpdate;
		private static Callback<LobbyDataUpdate_t> cbLobbyDataUpdate;
		private static Callback<P2PSessionRequest_t> cbP2pSessionRequest;
		private static Callback<P2PSessionConnectFail_t> cbP2pSessionConnectFail;

		/// <summary> Initializes the network manager. </summary>
		public static void Initialize()
		{
			// No steamworks interfaces = playing multiplayer in your imagination.
			if ( !SteamAPI.Init() ) return;

			IsValid = true;
			crLobbyCreated = CallResult<LobbyCreated_t>.Create( OnLobbyCreated );
			crLobbyEntered = CallResult<LobbyEnter_t>.Create( OnLobbyEntered );
			cbLobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create( OnLobbyChatUpdate );
			cbLobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create( OnLobbyDataUpdate );
			cbP2pSessionRequest = Callback<P2PSessionRequest_t>.Create( OnP2PSessionRequest );
			cbP2pSessionConnectFail = Callback<P2PSessionConnectFail_t>.Create( OnP2PSessionConnectFail );
		}

		/// <summary> Begins hosting a new P2P session. </summary>
		public static void HostSession( int maxPlayers, bool isPublic )
		{
			if ( !IsValid ) return;
			if ( InSession )
			{
				ModUI.ShowMessage( "<color=orange>Already in a session!</color>\nYou must disconnect from this one before creating a new one.", "BeerMP" );
				return;
			}

			IsHost = true;
			GameObject.Find( "Loading" ).GetComponentInParent<DynamicLoadingScreen>().SetVisibility( true, "CREATING SESSION..." );
			crLobbyCreated.Set( SteamMatchmaking.CreateLobby( isPublic ? ELobbyType.k_ELobbyTypePublic : ELobbyType.k_ELobbyTypeFriendsOnly, maxPlayers ) );
		}

		/// <summary> Called when a response is received from the Steamworks matchmaking service after requesting a new lobby. </summary>
		private static void OnLobbyCreated( LobbyCreated_t param, bool bIOFailure )
		{
			// Typically this will fail because the game was gifted and the account is limited.
			if ( bIOFailure || param.m_eResult != EResult.k_EResultOK )
			{
				IsHost = false;
				GameObject.Find( "Loading" ).GetComponentInParent<DynamicLoadingScreen>().Hide();
				ModUI.ShowMessage( $"Failed to create session: {param.m_eResult}" );
				return;
			}

			// Set the lobby metadata.
			CurrentLobby = new CSteamID( param.m_ulSteamIDLobby );
			SteamMatchmaking.SetLobbyData( CurrentLobby, "protocol", PROTOCOL_VERSION.ToString() );

			// Load into the game.
			GameObject.Find( "Loading" ).GetComponentInParent<DynamicLoadingScreen>().SetText( "SESSION STARTED\nNOW LOADING YEAR 1995" );
			Application.LoadLevelAsync( "GAME" );
		}

		/// <summary> Called when a response is received from the Steamworks matchmaking service after joining a lobby. </summary>
		private static void OnLobbyEntered( LobbyEnter_t param, bool bIOFailure )
		{
			if ( bIOFailure || param.m_EChatRoomEnterResponse != (uint)EChatRoomEnterResponse.k_EChatRoomEnterResponseSuccess )
			{
				GameObject.Find( "Loading" ).GetComponentInParent<DynamicLoadingScreen>().Hide();
				ModUI.ShowMessage( $"Failed to join session: {(EChatRoomEnterResponse)param.m_EChatRoomEnterResponse}" );
				return;
			}

			// TODO: Authenticate with the host BEFORE loading into the game scene.
			CurrentLobby = new CSteamID( param.m_ulSteamIDLobby );

			GameObject.Find( "Loading" ).GetComponentInParent<DynamicLoadingScreen>().SetText( "SESSION JOINED\nNOW LOADING YEAR 1995" );
			Application.LoadLevelAsync( "GAME" );
		}

		private static void OnLobbyChatUpdate( LobbyChatUpdate_t param )
		{
			throw new NotImplementedException();
		}

		private static void OnLobbyDataUpdate( LobbyDataUpdate_t param )
		{
			throw new NotImplementedException();
		}

		/// <summary> Called when a remote connection is attempting to communicate with us. </summary>
		private static void OnP2PSessionRequest( P2PSessionRequest_t param )
		{
			// Clients do not accept connection requests under any circumstances.
			if ( IsClient ) return;
		}

		/// <summary> Called when a remote connection is experiencing issues. </summary>
		private static void OnP2PSessionConnectFail( P2PSessionConnectFail_t param )
		{
			// TODO: Investigate what the typical reasons are.
		}

		public static void Update()
		{
			if ( !IsValid ) return;
			SteamAPI.RunCallbacks();
		}
	}
}
