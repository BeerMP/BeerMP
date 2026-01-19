using BeerMP.Components;
using BeerMP.Extensions;
using BeerMP.UI;
using HutongGames.PlayMaker.Actions;
using MSCLoader;
using UnityEngine;

namespace BeerMP.Scene
{
	internal static class Orchestrator
	{
		public static void OnMainMenuLoaded()
		{
			// Grab the button stack and buttons.
			var buttonStack = GameObject.Find( "Interface" ).transform.GetChild( 2 ).gameObject;
			var continueButton = buttonStack.transform.FindChild( "ButtonContinue" ).gameObject;
			var newGameButton = buttonStack.transform.FindChild( "ButtonNewgame" ).gameObject;

			// Create a dedicated multiplayer button so that people aren't forced to play this.
			var multiplayerButton = newGameButton.Instantiate( buttonStack.transform, "MultiplayerButton" );

			// Reposition other buttons.
			var gapHeight = continueButton.transform.localPosition - newGameButton.transform.localPosition;
			continueButton.transform.localPosition += gapHeight;
			newGameButton.transform.localPosition += gapHeight;

			// Change the TextMesh values for the multiplayer button.
			var textMeshes = multiplayerButton.GetComponentsInChildren<TextMesh>();
			for ( int i = 0; i < textMeshes.Length; i++ )
			{
				textMeshes[i].text = "MULTIPLAYER";
			}

			// Update the scale of the interaction collider.
			var collider = multiplayerButton.GetComponent<BoxCollider>();
			var renderer = textMeshes[0].GetComponent<MeshRenderer>();
			collider.size = new Vector3( renderer.bounds.size.x / multiplayerButton.transform.lossyScale.x, 0.08f, 0.01f );
			collider.center = multiplayerButton.transform.InverseTransformPoint( renderer.bounds.center );

			// Inject the multiplayer menu.
			var fsm = multiplayerButton.GetComponent<PlayMakerFSM>();
			fsm.FsmInject( "State 1", () => MPUI.Canvas.GetComponent<MultiplayerMenu>().Show(), index: 1 );
			fsm.GetState( "State 1" ).RemoveActionsAfter( 1 );

			// Loading screen is disabled by default. Therefore we need to fetch it from the continue button.
			var continueFsm = continueButton.GetComponent<PlayMakerFSM>();
			continueFsm.InitializeFSM();

			var loadingScreen = (continueFsm.FsmStates[3].Actions[1] as ActivateGameObject).gameObject.GameObject.Value;
			loadingScreen.AddComponent<DynamicLoadingScreen>();
			loadingScreen.SetActive( true );
		}
	}
}
