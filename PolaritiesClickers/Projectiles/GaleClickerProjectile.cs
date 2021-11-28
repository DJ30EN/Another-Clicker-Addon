using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace PolaritiesClickers.Projectiles
{
	public class GaleClickerProjectile : ModProjectile
	{
		public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Gale Feather");     //The English name of the projectile
		}
		public override void SetDefaults() 
        {
			projectile.width = 14;               //The width of projectile hitbox
			projectile.height = 14;              //The height of projectile hitbox
			projectile.friendly = true;         //Can the projectile deal damage to enemies?
			projectile.penetrate = 1;           // Make the feather strike 1 time and get killed
			projectile.timeLeft = 150;
			projectile.extraUpdates = 2;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
		}

		public bool Spawned
		{
			get => projectile.ai[0] == 1f;
			set => projectile.ai[0] = value ? 1f : 0f;
		}

		public int Timer
		{
			get => (int)projectile.ai[1];
			set => projectile.ai[1] = value;
		}

		public override void AI() 
		{
			projectile.spriteDirection = projectile.direction;

			if (!Spawned)
			{
				Spawned = true;
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 17);
				for (int d = 0; d < 10; d++)
				{
					Dust.NewDust(projectile.Center, 10, 10, 16, 0.5f * projectile.velocity.X, 0.5f * projectile.velocity.Y, 0, default(Color), 0.75f);
				}
			}

			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

		}

		public override void Kill(int timeLeft) 
		{
			for (int d = 0; d < 10; d++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 42, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 0, default(Color), 0.75f);
			}
		}
	}
}