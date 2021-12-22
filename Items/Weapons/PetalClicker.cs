using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using PolaritiesClickers.Buffs;
using System.Collections.Generic;
using PolaritiesClickers.Projectiles;

namespace PolaritiesClickers.Items.Weapons
{
	//A more advanced example using a custom click effect, which uses a custom clicker projectile
	public class PetalClicker : ModItem
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
			string uniqueName = ClickerCompat.RegisterClickEffect(mod, "Petalstorm", "Petal Dance", "5 petals spiral out", 8, new Color(255, 193, 235), delegate (Player player, Vector2 position, int type, int damage, float knockBack)
			{
				Main.PlaySound(SoundID.Grass, Main.MouseWorld);
				for (int index = 0; index < 5; index++)
				{
					Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<PetalClickerProjectile>(), (int)(damage * 0.5), 0, player.whoAmI, index);
				}
			});

			DisplayName.SetDefault("Petal Clicker");
		}

		public override void SetDefaults()
		{
			ClickerCompat.SetClickerWeaponDefaults(item);

			ClickerCompat.SetRadius(item, 1.8f);
			ClickerCompat.SetColor(item, new Color(255, 193, 235));
			ClickerCompat.SetDust(item, 166);

			//Here we use our custom effect, registered as 'modName:internalName'
			ClickerCompat.AddEffect(item, "PolaritiesClickers:Petalstorm");

			//We can add more than one effect aswell! (Here using the IEnumerable overload to make it more compact)
			//ClickerCompat.AddEffect(item, new List<string> { "ClickerClass:Inferno", "ClickerClass:Embrittle" });

			item.damage = 5;
			item.width = 30;
			item.height = 30;
			item.knockBack = 1f;
			item.value = 3000;
			item.rare = ItemRarityID.Green;
		}

		public override void AddRecipes()
		{
			Mod ThoriumMod = ModLoader.GetMod("ThoriumMod");
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ThoriumMod.ItemType("Petal"), 8);
			recipe.AddTile(ThoriumMod.TileType("ArcaneArmorFabricator"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}