using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PolaritiesClickers.Projectiles
{
	public class CupidClickerProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charm");     //The English name of the projectile
		}

		public override void SetDefaults()
		{
			projectile.width = 30;               //The width of projectile hitbox
			projectile.height = 30;              //The height of projectile hitbox
			projectile.friendly = true;         //Can the projectile deal damage to enemies?
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 10;
			projectile.alpha = 255;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
		}

		public bool Spawned
		{
			get => projectile.ai[0] == 1f;
			set => projectile.ai[0] = value ? 1f : 0f;
		}

		public override void AI()
        {
            if (!Spawned)
            {
				for (int g = 0; g < 6; g++)
                {
                    g = Gore.NewGore(projectile.Center, new Vector2(Main.rand.Next(-4, 4), Main.rand.Next(-4, 4)), 331, (float)Main.rand.Next(40, 120) * 0.01f);
                    Main.gore[g].sticky = false;
                }
            }
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModLoader.GetMod("ThoriumMod").BuffType("Charmed"), 300);
			target.AddBuff(BuffID.Lovestruck, 300);
		}
	}
}
