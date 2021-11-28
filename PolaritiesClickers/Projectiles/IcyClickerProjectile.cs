using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PolaritiesClickers.Projectiles
{
	public class IcyClickerProjectile : ModProjectile
	{
		public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Cold Click");     //The English name of the projectile
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
			projectile.coldDamage = true;
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
				Spawned = true;

				for (int i = 0; i <= 20; i++)
				{
					int index = Dust.NewDust(projectile.Center, 10, 10, 92, Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f), 67, default, 1f);
					Dust dust = Main.dust[index];
					dust.noGravity = true;
				}
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModLoader.GetMod("ThoriumMod").BuffType("EnemyFrozen"), 180);
		}
	}
}