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
	public class LeafClickerProjectile : ModProjectile
	{
		public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Leaves");     //The English name of the projectile
		}
		public override void SetDefaults() 
        {
			projectile.width = 12;               //The width of projectile hitbox
			projectile.height = 12;              //The height of projectile hitbox
			projectile.friendly = true;         //Can the projectile deal damage to enemies?
			projectile.penetrate = 1; 			// Make the leaf strike 1 time and get killed
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough) 
        {
			width = 6; 
			height = 6;
			return true;
		}

		public override void AI() 
		{
			projectile.spriteDirection = projectile.direction;

            if (projectile.velocity.Y > 2f) // This check implements "terminal velocity". We don't want the projectile to keep getting faster and faster. Past 16f this projectile will travel through blocks, so this check is useful.
			{
				projectile.velocity.Y = 2f;
			}
			
			if (projectile.velocity.X > 3f) // This check implements "terminal velocity". We don't want the projectile to keep getting faster and faster. Past 16f this projectile will travel through blocks, so this check is useful.
			{
				projectile.velocity.X = 3f;
			}

			if (projectile.velocity.X < -3f) // This check implements "terminal velocity". We don't want the projectile to keep getting faster and faster. Past 16f this projectile will travel through blocks, so this check is useful.
			{
				projectile.velocity.X = -3f;
			}

            if (projectile.ai[1] == 0)
            {
                projectile.ai[0] += 1f;
				projectile.scale = 1;
				projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f); 
            }

            if (projectile.ai[0] >= 20f)
            {
            	// third of a second has passed. Reset timer, etc.
				projectile.velocity.X = projectile.velocity.X * 0.97f;
				projectile.velocity.Y = projectile.velocity.Y + 0.3f;
            	// Do something here, maybe change to a new state.
            }

            if (projectile.velocity.Y >= 0f)
            {
            	// Half a second has passed. Reset timer, etc.
				projectile.ai[1] = 1;
				projectile.ai[0] = 0;
            	// Do something here, maybe change to a new state.
            }

            if (projectile.ai[1] == 1)
            {
                Microsoft.Xna.Framework.Vector2 direction = new Microsoft.Xna.Framework.Vector2(1, 0).RotatedBy(projectile.rotation);

				//gravity
				projectile.velocity.Y += 0.3f;

				//drag:
				float drag = (float)Math.Abs(Math.Sin(projectile.velocity.ToRotation() - direction.ToRotation()));
				float sidewaysForce = (float)Math.Sin(8 * (projectile.velocity.ToRotation() - direction.ToRotation()));

				projectile.velocity -= 0.1f * (projectile.velocity * drag + projectile.velocity.RotatedBy(MathHelper.PiOver2) * sidewaysForce);

				//you're probably hitting the front more or somehting which affects angular momentum
				projectile.ai[0] -= 0.001f * projectile.velocity.Length() * sidewaysForce;

				//angular momentum drag:
				projectile.ai[0] -= 0.1f * projectile.ai[0];

				projectile.rotation += projectile.ai[0];
            }
		}

		public override void Kill(int timeLeft) 
		{
			Main.PlaySound(SoundID.Grass, projectile.position);
			for (int d = 0; d < 10; d++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 3, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 0, default(Color), 0.75f);
			}
		}
	}
}