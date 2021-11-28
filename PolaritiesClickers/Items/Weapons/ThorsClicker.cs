using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PolaritiesClickers.Items.Weapons
{
	//A more advanced example using a custom click effect, which uses a custom clicker projectile
	public class ThorsClicker : ModItem
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
			string uniqueName = ClickerCompat.RegisterClickEffect(mod, "QuadClick", "Quadruple Click", "Deals damage 4 times on a single attack", 10, new Color(64, 247, 255), delegate (Player player, Vector2 position, int type, int damage, float knockBack)
			{
				Main.PlaySound(SoundID.Item, (int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, 37);
				Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, type, damage, knockBack, player.whoAmI);
				Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, type, damage, knockBack, player.whoAmI);
				Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, type, damage, knockBack, player.whoAmI);
			});

			DisplayName.SetDefault("Thor's Clicker");
		}

		public override void SetDefaults()
		{
			ClickerCompat.SetClickerWeaponDefaults(item);

			ClickerCompat.SetRadius(item, 2f);
			ClickerCompat.SetColor(item, new Color(64, 247, 255));
			ClickerCompat.SetDust(item, 15);

			//Here we use our custom effect, registered as 'modName:internalName'
			ClickerCompat.AddEffect(item, "PolaritiesClickers:QuadClick");

			//We can add more than one effect aswell! (Here using the IEnumerable overload to make it more compact)
			//ClickerCompat.AddEffect(item, new List<string> { "ClickerClass:Inferno", "ClickerClass:Embrittle" });

			item.damage = 6;
			item.width = 30;
			item.height = 30;
			item.knockBack = 2f;
			item.value = 8500;
			item.rare = ItemRarityID.Green;
		}

		public override void AddRecipes()
		{
			Mod ThoriumMod = ModLoader.GetMod("ThoriumMod");
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ThoriumMod.ItemType("ThoriumBar"), 8);
			recipe.AddTile(ThoriumMod.TileType("ThoriumAnvil"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}