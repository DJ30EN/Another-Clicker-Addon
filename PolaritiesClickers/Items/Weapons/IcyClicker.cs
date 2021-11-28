using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using PolaritiesClickers.Projectiles;

namespace PolaritiesClickers.Items.Weapons
{
	//A more advanced example using a custom click effect, which uses a custom clicker projectile
	public class IcyClicker : ModItem
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
			string uniqueName = ClickerCompat.RegisterClickEffect(mod, "Chiller", "Cold Click", "Inflict extra damage and slow enemy movement", 8, new Color(206, 255, 255), delegate (Player player, Microsoft.Xna.Framework.Vector2 position, int type, int damage, float knockBack)
			{
                Main.PlaySound(SoundID.Item, (int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, 27);
                Projectile.NewProjectile(Main.MouseWorld, Microsoft.Xna.Framework.Vector2.Zero, ModContent.ProjectileType<IcyClickerProjectile>(), (int)(damage * 0.5f), knockBack, player.whoAmI);
			});

			DisplayName.SetDefault("Icy Clicker");
		}

		public override void SetDefaults()
		{
			ClickerCompat.SetClickerWeaponDefaults(item);

			ClickerCompat.SetRadius(item, 1.25f);
			ClickerCompat.SetColor(item, new Color(206, 255, 255));
			ClickerCompat.SetDust(item, 42);

			//Here we use our custom effect, registered as 'modName:internalName'
			ClickerCompat.AddEffect(item, "PolaritiesClickers:Chiller");

			//We can add more than one effect aswell! (Here using the IEnumerable overload to make it more compact)
			//ClickerCompat.AddEffect(item, new List<string> { "ClickerClass:Inferno", "ClickerClass:Embrittle" });

			item.damage = 3;
			item.width = 30;
			item.height = 30;
			item.knockBack = 0.5f;
			item.value = 250;
			item.rare = ItemRarityID.White;
		}

		public override void AddRecipes()
		{
			Mod ThoriumMod = ModLoader.GetMod("ThoriumMod");
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ThoriumMod.ItemType("IcyShard"), 6);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}