using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PolaritiesClickers.Projectiles
{
	public class GeothermalClickerProjectile : ModProjectile
	{
		public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Chain Eruption");     //The English name of the projectile
		}

		public override void SetDefaults() 
        {
			projectile.width = 150;               //The width of projectile hitbox
			projectile.height = 150;              //The height of projectile hitbox
			projectile.friendly = true;         //Can the projectile deal damage to enemies?
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 10;
			projectile.alpha = 255;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
			projectile.extraUpdates = 3;
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
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 14);

				for (int k = 0; k < 6; k++)
				{
					int index = Dust.NewDust(projectile.Center, 10, 10, 174, Main.rand.NextFloat(-9f, 9f), Main.rand.NextFloat(-9f, 9f), 125, default, 1f);
					Dust dust = Main.dust[index];
					dust.noGravity = true;
				}
				for (int k = 0; k < 9; k++)
				{
					int index = Dust.NewDust(projectile.Center, 10, 10, 31, Main.rand.NextFloat(-12f, 12f), Main.rand.NextFloat(-12f, 12f), 125, default, 1f);
					Dust dust = Main.dust[index];
					dust.noGravity = true;
				}
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModLoader.GetMod("ThoriumMod").BuffType("Singed"), 180);
			target.AddBuff(BuffID.OnFire, 180, false);

			if (projectile.ai[1] == 0)
			{
				Projectile.NewProjectile(target.Center, Vector2.Zero, projectile.type, projectile.damage, projectile.knockBack, projectile.owner, ai1: 1);
			}
		}
	}
}