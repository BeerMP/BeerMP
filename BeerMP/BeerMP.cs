using BeerMP.Network;
using BeerMP.Scene;
using BeerMP.UI;
using MSCLoader;
using System;
using System.IO;
using System.Reflection;

namespace BeerMP
{
	/// <summary> Main mod class. </summary>
	public class BeerMP : Mod
	{
		/// <summary> This is the unique ID which other mods can reference. </summary>
		public override string ID => "BeerMP";
		/// <summary> Local client version. </summary>
		public override string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();
		/// <summary> Project maintainers. Full list of contributors can be found on the GitHub repository. </summary>
		public override string Author => "BrennFuchS & diehy";
		/// <summary> Description displayed in the mods list. </summary>
		public override string Description => "Letting you and your friends experience the chaos of building a car in 1990s Finland, together!";
		/// <summary> Which games this mod supports, and in this case it's both. </summary>
		public override Game SupportedGames => Game.MySummerCar_And_MyWinterCar;

		/// <summary> Indicates whether the runtime data DLL for this game is present. </summary>
		public static bool RuntimeDataAvailable { get; private set; }

		/// <summary> Entrypoint. Called once by mod loader on startup. </summary>
		public override void ModSetup()
		{
			// TODO: Prompt a download. Also investigate whether the runtime DLLs can be marked as a dependency on the mod loader website.
			var dllPath = Path.Combine( ModLoader.GetModAssetsFolder( this ), ModLoader.CurrentGame == Game.MySummerCar ? "BeerMP.MSC.dll" : "BeerMP.MWC.dll" );
			if ( !File.Exists( dllPath ) )
			{
				ModUI.ShowMessage( $"<color=yellow>Missing runtime data!</color>{Environment.NewLine}Ensure you compile the correct runtime data library for this game!", "BeerMP" );
			}

			// Initialize components.
			MPUI.BuildCanvas();
			Networking.Initialize();

			// Lifecycle hooks.
			SetupFunction( Setup.OnMenuLoad, Orchestrator.OnMainMenuLoaded );
			SetupFunction( Setup.Update, OnUpdate );
		}

		private void OnUpdate()
		{
			Networking.Update();
		}
	}
}
