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
	public class CupidClicker : ModItem
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
			string uniqueName = ClickerCompat.RegisterClickEffect(mod, "Chu", "Charm", "Damage and Charm the enemy for 5 seconds", 12, new Color(255, 142, 180), delegate (Player player, Microsoft.Xna.Framework.Vector2 position, int type, int damage, float knockBack)
			{
				Main.PlaySound(SoundID.Item, (int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, 4);
				Projectile.NewProjectile(Main.MouseWorld, Microsoft.Xna.Framework.Vector2.Zero, ModContent.ProjectileType<CupidClickerProjectile>(), (int)(damage * 0.5f), knockBack, player.whoAmI);
			});

			DisplayName.SetDefault("Cupid Clicker");
		}

		public override void SetDefaults()
		{
			ClickerCompat.SetClickerWeaponDefaults(item);

			ClickerCompat.SetRadius(item, 1.5f);
			ClickerCompat.SetColor(item, new Color(255, 142, 180));
			ClickerCompat.SetDust(item, 90);

			//Here we use our custom effect, registered as 'modName:internalName'
			ClickerCompat.AddEffect(item, "PolaritiesClickers:Chu");

			//We can add more than one effect aswell! (Here using the IEnumerable overload to make it more compact)
			//ClickerCompat.AddEffect(item, new List<string> { "ClickerClass:Inferno", "ClickerClass:Embrittle" });

			item.damage = 4;
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
			recipe.AddIngredient(ThoriumMod.ItemType("LifeQuartz"), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}