using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PolaritiesClickers.Projectiles
{
	public class StaticClickerProjectile : ModProjectile
	{
		public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Static Shock");     //The English name of the projectile
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

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Main.PlaySound(SoundID.Item, target.position, 15);
			for (int i = 0; i < 15; i++)
			{
				int index = Dust.NewDust(target.position, target.width, target.height, 159, 0f, 0f, 150, default(Color), 1.25f);
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
		}
	}
}