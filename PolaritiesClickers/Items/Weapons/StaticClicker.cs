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
	public class StaticClicker : ModItem
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
			string uniqueName = ClickerCompat.RegisterClickEffect(mod, "Zap", "Static", "Zap nearby enemies", 10, new Color(248, 223, 26), delegate (Player player, Microsoft.Xna.Framework.Vector2 position, int type, int damage, float knockBack)
			{
				for (int i = 0; i <= 20; i++)
				{
					int index = Dust.NewDust(Main.MouseWorld, 10, 10, 159, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f), 67, default, 1f);
                    Dust dust = Main.dust[index];
					dust.noGravity = true;
				}
                Main.PlaySound(SoundID.Item, (int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, 94);
				for (int i = 0; i < Main.maxNPCs; i++)
				{
                    NPC target = Main.npc[i];
					if (target.CanBeChasedBy() && target.DistanceSQ(Main.MouseWorld) < 200 * 200)
					{
                        Projectile.NewProjectile(target.position, Microsoft.Xna.Framework.Vector2.Zero, ModContent.ProjectileType<StaticClickerProjectile>(), damage, knockBack, player.whoAmI);
					}
				}
			});

			DisplayName.SetDefault("Static Clicker");
		}

		public override void SetDefaults()
		{
			ClickerCompat.SetClickerWeaponDefaults(item);

			ClickerCompat.SetRadius(item, 1.4f);
			ClickerCompat.SetColor(item, new Color(125, 217, 213));
			ClickerCompat.SetDust(item, 162);

			//Here we use our custom effect, registered as 'modName:internalName'
			ClickerCompat.AddEffect(item, "PolaritiesClickers:Zap");

			//We can add more than one effect aswell! (Here using the IEnumerable overload to make it more compact)
			//ClickerCompat.AddEffect(item, new List<string> { "ClickerClass:Inferno", "ClickerClass:Embrittle" });

			item.damage = 4;
			item.width = 30;
			item.height = 30;
			item.knockBack = 1f;
			item.value = 800;
			item.rare = ItemRarityID.Blue;
		}
	}
}