using MSCLoader;
using System.Reflection;

namespace BeerMP
{
	public class BeerMP : Mod
	{
		public override string ID => "BeerMP";
		public override string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();
		public override string Author => "BrennFuchS & diehy";
		public override string Description => "Letting you and your friends experience the chaos of building a car in 1990s Finland, together!";
		public override Game SupportedGames => Game.MySummerCar_And_MyWinterCar;
	}
}
