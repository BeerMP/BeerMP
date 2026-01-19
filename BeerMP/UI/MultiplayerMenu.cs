using BeerMP.Network;
using UnityEngine;

namespace BeerMP.UI
{
	// Temporary menu for testing.
	public class MultiplayerMenu : MonoBehaviour
	{
		private bool visible;
		private int maxPlayers = 4;
		private bool isPublic = true;
		private Rect windowRect = new Rect( (Screen.width / 2f) - 150f, (Screen.height / 2f) - 100f, 300f, 200f );

		private GameObject quadObj;
		private GameObject interfaceObj;
		private GameObject interfaceActiveObj;

		private void Start()
		{
			quadObj = GameObject.Find( "Scene" ).transform.GetChild( 2 ).gameObject;
			interfaceObj = GameObject.Find( "Interface" );
			interfaceActiveObj = GameObject.Find( "InterfaceActive" );
		}

		private void OnGUI()
		{
			if ( !visible ) return;
			windowRect = GUI.Window( 0, windowRect, DrawWindow, "Host Lobby" );
		}

		private void DrawWindow( int id )
		{
			var y = 25f;

			GUI.Label( new Rect( 10f, y, 80f, 20f ), "Max Players:" );
			GUI.Label( new Rect( 95f, y, 30f, 20f ), maxPlayers.ToString() );
			maxPlayers = Mathf.RoundToInt( GUI.HorizontalSlider( new Rect( 125f, 25f + 5f, 165f, 20f ), maxPlayers, 2, Networking.MAX_CONCURRENT_CONNECTIONS ) );
			y += 25f;

			isPublic = GUI.Toggle( new Rect( 10f, y, 280f, 20f ), isPublic, "Public Lobby" );
			y += 35f;

			if ( GUI.Button( new Rect( 10f, windowRect.height - 40f, 135f, 30f ), "Start" ) )
			{
				Networking.HostSession( maxPlayers, isPublic );
				Hide();
			}

			if ( GUI.Button( new Rect( 155f, windowRect.height - 40f, 135f, 30f ), "Cancel" ) ) Hide();

			GUI.DragWindow( new Rect( 0f, 0f, windowRect.width, 20f ) );
		}

		public void Show()
		{
			if ( quadObj ) quadObj.SetActive( false );
			if ( interfaceObj ) interfaceObj.SetActive( false );
			if ( interfaceActiveObj ) interfaceActiveObj.SetActive( false );
			visible = true;
		}

		public void Hide()
		{
			if ( quadObj ) quadObj.SetActive( true );
			if ( interfaceObj ) interfaceObj.SetActive( true );
			if ( interfaceActiveObj ) interfaceActiveObj.SetActive( true );
			visible = false;
		}

	}
}
