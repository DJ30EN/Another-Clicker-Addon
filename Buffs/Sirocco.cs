using Terraria;
using Terraria.ModLoader;

namespace PolaritiesClickers.Buffs
{
	public class Sirocco : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Sirocco");
			Description.SetDefault("Movement Speed and Jumping Power are increased");
			Main.buffNoSave[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.jumpSpeedBoost = 1.33f;
			player.moveSpeed += 0.33f;
			player.maxRunSpeed += 0.33f;
		}
	}
}