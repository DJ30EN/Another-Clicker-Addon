using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using PolaritiesClickers.Projectiles;

namespace PolaritiesClickers.Items.Weapons
{
	//A more advanced example using a custom click effect, which uses a custom clicker projectile
	public class ReefClicker : ModItem
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
			string uniqueName = ClickerCompat.RegisterClickEffect(mod, "CoralSpikes", "Coral Spikes", "Leave 3 sticking corals for 30 seconds", 10, new Color(251, 215, 184), delegate (Player player, Microsoft.Xna.Framework.Vector2 position, int type, int damage, float knockBack)
			{
                Main.PlaySound(SoundID.Splash, (int)Main.MouseWorld.X, (int)Main.MouseWorld.Y);
				for (int d = 0; d < 15; d++)
				{
                    Dust.NewDust(Main.MouseWorld, 10, 10, 33, Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f), 0, default(Color), 1.5f);
				}
				for (int i = 0; i < 3; i++)
				{
                    Projectile.NewProjectile(Main.MouseWorld.X, Main.MouseWorld.Y, Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-2f, -3f), ModContent.ProjectileType<ReefClickerProjectile>(), (int)(damage * 0.25f), knockBack, player.whoAmI);
				}
			});

			DisplayName.SetDefault("Reef Clicker");
		}

		public override void SetDefaults()
		{
			ClickerCompat.SetClickerWeaponDefaults(item);

			ClickerCompat.SetRadius(item, 1.3f);
			ClickerCompat.SetColor(item, new Color(211, 195, 173));
			ClickerCompat.SetDust(item, 225);

			//Here we use our custom effect, registered as 'modName:internalName'
			ClickerCompat.AddEffect(item, "PolaritiesClickers:CoralSpikes");

			//We can add more than one effect aswell! (Here using the IEnumerable overload to make it more compact)
			//ClickerCompat.AddEffect(item, new List<string> { "ClickerClass:Inferno", "ClickerClass:Embrittle" });

			item.damage = 4;
			item.width = 30;
			item.height = 30;
			item.knockBack = 0.5f;
			item.value = 250;
			item.rare = ItemRarityID.White;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Coral, 12);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}