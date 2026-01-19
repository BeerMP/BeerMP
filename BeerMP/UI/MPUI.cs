using MSCLoader;
using UnityEngine;

namespace BeerMP.UI
{
	/// <summary> UI manager for the mod. </summary>
	internal static class MPUI
	{
		public static GameObject Canvas;

		public static void BuildCanvas()
		{
			// Prevent duplicate canvases.
			if ( Canvas ) return;

			// Create the canvas and add our UI elements.
			Canvas = ModUI.CreateCanvas( "BeerMP_UI", true );
			Canvas.AddComponent<MultiplayerMenu>();
		}
	}
}
