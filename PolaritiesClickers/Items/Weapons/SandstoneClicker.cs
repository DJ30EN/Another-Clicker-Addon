using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using PolaritiesClickers.Buffs;
using System.Collections.Generic;
using PolaritiesClickers.Projectiles;

namespace PolaritiesClickers.Items.Weapons
{
	//A more advanced example using a custom click effect, which uses a custom clicker projectile
	public class SandstoneClicker : ModItem
	{
		public override bool Autoload(ref string name)
		{
			return ClickerCompat.ClickerClass != null;
		}

		public override void SetStaticDefaults()
		{
			ClickerCompat.RegisterClickerWeapon(this);
			//We want to cache the result to make accessing it easier in other places. For the purpose of the example, we don't
			//(Make sure to unload the saved string again in Mod.Unload()!)
			string uniqueName = ClickerCompat.RegisterClickEffect(mod, "WindBoost", "Sirocco", "Increase jump power and movement speed by 33%", 8, new Color(211, 190, 97), delegate (Player player, Microsoft.Xna.Framework.Vector2 position, int type, int damage, float knockBack)
			{
                Main.PlaySound(SoundID.Item, (int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, 24);
				player.AddBuff(ModContent.BuffType<Sirocco>(), 360, false);
				for (int i = 0; i < 15; i++)
				{
					int index = Dust.NewDust(player.position, player.width, player.height, 31, 0f, 0f, 150, default(Color), 1.25f);
                    Dust dust = Main.dust[index];
					dust.noGravity = true;
					dust.velocity *= 0.75f;
					int x = Main.rand.Next(-50, 51);
					int y = Main.rand.Next(-50, 51);
					dust.position.X += x;
					dust.position.Y += y;
					dust.velocity.X = -x * 0.075f;
					dust.velocity.Y = -y * 0.075f;
				}
			});

			DisplayName.SetDefault("Sandstone Clicker");
		}

		public override void SetDefaults()
		{
			ClickerCompat.SetClickerWeaponDefaults(item);

			ClickerCompat.SetRadius(item, 1.5f);
			ClickerCompat.SetColor(item, new Color(211, 190, 97));
			ClickerCompat.SetDust(item, 31);

			//Here we use our custom effect, registered as 'modName:internalName'
			ClickerCompat.AddEffect(item, "PolaritiesClickers:WindBoost");

			//We can add more than one effect aswell! (Here using the IEnumerable overload to make it more compact)
			//ClickerCompat.AddEffect(item, new List<string> { "ClickerClass:Inferno", "ClickerClass:Embrittle" });

			item.damage = 3;
			item.width = 30;
			item.height = 30;
			item.knockBack = 1f;
			item.value = 800;
			item.rare = ItemRarityID.Blue;
		}

		public override void AddRecipes()
		{
			Mod ThoriumMod = ModLoader.GetMod("ThoriumMod");
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ThoriumMod.ItemType("SandStone"), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}