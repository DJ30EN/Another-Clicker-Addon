using Terraria;
using Terraria.ModLoader;

namespace PolaritiesClickers.Buffs
{
	public class RiskyClicks : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Risky Clicks");
			Description.SetDefault("Enemies receive more damage halfway into clicker radius");
			Main.buffNoSave[Type] = false;
		}
	}
}