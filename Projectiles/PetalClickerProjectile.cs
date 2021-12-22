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
	public class PetalClickerProjectile : ModProjectile
	{
		public int radiusIncrease = 0;
		public float rot = 0f;
		public Vector2 center = Vector2.Zero;
		public int Index
		{
			get => (int) projectile.ai[0];
			set => projectile.ai[0] = value;
		}

		public float Rotation
		{
			get => projectile.localAI[1];
			set => projectile.localAI[1] = value;
		}

		public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Dancing Petal");     //The English name of the projectile
		}

		public override void SetDefaults() 
        {
			projectile.width = 14;               //The width of projectile hitbox
			projectile.height = 14;              //The height of projectile hitbox
			projectile.friendly = true;         //Can the projectile deal damage to enemies?
			projectile.penetrate = 1;
			projectile.timeLeft = 250;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
		}

		public int Timer
		{
			get => (int)projectile.ai[1];
			set => projectile.ai[1] = value;
		}

		public override void AI() 
		{
			if (projectile.ai[1] == 0)
            {
				for (int d = 0; d < 5; d++)
				{
					Dust.NewDust(projectile.Center, projectile.width, projectile.height, 164, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f), 0, default(Color), 1f);
				}
			}
			projectile.rotation -= MathHelper.ToRadians(36*(Index-1)) + projectile.velocity.X > 0f ? 0.1f : -0.1f;

			Timer++;
			if (Timer % 5 == 0)
			{
				Rotation = projectile.rotation;
			}

			if (center == Vector2.Zero)
			{
				center = projectile.Center;
			}
	
			radiusIncrease += 1;
			rot += 0.05f;
			projectile.Center = center + new Vector2(0, 20 + radiusIncrease).RotatedBy(rot + (Index * (MathHelper.TwoPi / 5)));
		}

		public override void Kill(int timeLeft) 
		{
			for (int d = 0; d < 10; d++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 166, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 0, default(Color), 0.75f);
			}
		}
	}
}