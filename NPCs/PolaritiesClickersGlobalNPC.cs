using PolaritiesClickers.Items.Weapons;
using Terraria;
using Terraria.ModLoader;

namespace PolaritiesClickers.NPCs
{
	public class PolaritiesClickersGlobalNPC : GlobalNPC
	{
		public override void NPCLoot(NPC npc)
		{
			Mod ThoriumMod = ModLoader.GetMod("ThoriumMod");
			if (npc.type == ThoriumMod.NPCType("TheGrandThunderBirdv2"))
			{
				if (Main.rand.NextBool(25))
				{
					Item.NewItem(npc.Hitbox, ModContent.ItemType<StaticClicker>(), 1, false, -1);
				}
			}
		}
	}
}