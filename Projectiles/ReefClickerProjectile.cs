using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace PolaritiesClickers.Projectiles
{
	public class ReefClickerProjectile : ModProjectile
	{
		public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Coral Spikes");     //The English name of the projectile
		}
		public override void SetDefaults() 
        {
			projectile.width = 18;               //The width of projectile hitbox
			projectile.height = 18;              //The height of projectile hitbox
			projectile.friendly = true;         //Can the projectile deal damage to enemies?
			projectile.penetrate = 6;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 30;
			projectile.timeLeft = 1800;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 12;
			height = 12;
			return true;
		}
		// Code from ExampleJavelin
		public bool IsStickingToTarget
		{
			get => projectile.ai[0] == 1f;
			set => projectile.ai[0] = value ? 1f : 0f;
		}

		// Index of the current target
		public int TargetWhoAmI
		{
			get => (int)projectile.ai[1];
			set => projectile.ai[1] = value;
		}

		private const int CoralLimit = 9; // This is the max. amount of corals being able to attach
		private readonly Point[] _stickingCorals = new Point[CoralLimit]; // The point array holding for sticking corals

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
			IsStickingToTarget = true; // we are sticking to a target
			TargetWhoAmI = target.whoAmI; // Set the target whoAmI
			projectile.velocity = (target.Center - projectile.Center) * 0.75f;
			// Change velocity based on delta center of targets (difference between entity centers)
			projectile.netUpdate = true; // netUpdate coral
			// It is recommended to split your code into separate methods to keep code clean and clear
			UpdateCorals(target);
		}

		/*
		 * The following code handles the sticking to the enemy hit.
		 */
		private void UpdateCorals(NPC target)
		{
			int currentCorals = 0; // The index

			for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
			{
				Projectile currentProjectile = Main.projectile[i];
				if (i != projectile.whoAmI // Make sure the looped projectile is not the current javelin
				    && currentProjectile.active // Make sure the projectile is active
				    && currentProjectile.owner == Main.myPlayer // Make sure the projectile's owner is the client's player
				    && currentProjectile.type == projectile.type // Make sure the projectile is of the same type
				    && currentProjectile.modProjectile is ReefClickerProjectile coralProjectile // Use a pattern match cast so we can access the projectile like an ExampleJavelinProjectile
				    && coralProjectile.IsStickingToTarget // the previous pattern match allows us to use our properties
				    && coralProjectile.TargetWhoAmI == target.whoAmI)
				{

					_stickingCorals[currentCorals++] = new Point(i, currentProjectile.timeLeft); // Add the current projectile's index and timeleft to the point array
					if (currentCorals >= _stickingCorals.Length)  // If the javelin's index is bigger than or equal to the point array's length, break
						break;
				}
			}

			// Remove the oldest if we exceeded the maximum
			if (currentCorals >= CoralLimit)
			{
				int oldCorals = 0;
				// Loop our point array
				for (int i = 1; i < CoralLimit; i++)
				{
					// Remove the already existing javelin if it's timeLeft value (which is the Y value in our point array) is smaller than the new javelin's timeLeft
					if (_stickingCorals[i].Y < _stickingCorals[oldCorals].Y)
					{
						oldCorals = i; // Remember the index of the removed javelin
					}
				}
				// Remember that the X value in our point array was equal to the index of that javelin, so it's used here to kill it.
				Main.projectile[_stickingCorals[oldCorals].X].Kill();
			}
		}

		public override void AI()
		{
			// Run either the Sticky AI or Normal AI
			// Separating into different methods helps keeps your AI clean
			if (IsStickingToTarget) StickyAI();
			else NormalAI();
		}

		private void NormalAI()
		{
			TargetWhoAmI++;
			projectile.velocity.X = projectile.velocity.X * 0.97f;
			projectile.velocity.Y = projectile.velocity.Y + 0.4f; // 0.1f for arrow gravity, 0.4f for knife gravity
			if (projectile.velocity.Y > 16f) // This check implements "terminal velocity". We don't want the projectile to keep getting faster and faster. Past 16f this projectile will travel through blocks, so this check is useful.
			{
				projectile.velocity.Y = 16f;
			}
			// Make sure to set the rotation accordingly to the velocity, and add some to work around the sprite's rotation
			// Please notice the MathHelper usage, offset the rotation by 90 degrees (to radians because rotation uses radians) because the sprite's rotation is not aligned!
			projectile.rotation += projectile.velocity.X * 0.1f;
		}

		private void StickyAI()
		{
			// These 2 could probably be moved to the ModifyNPCHit hook, but in vanilla they are present in the AI
			projectile.ignoreWater = true; // Make sure the projectile ignores water (coral)
			projectile.tileCollide = false; // Make sure the projectile doesn't collide with tiles anymore
			const int aiFactor = 15; // Change this factor to change the 'lifetime' of this coral
			projectile.localAI[0] += 1f;

			// Every 30 ticks, the coral will perform a hit effect
			bool hitEffect = projectile.localAI[0] % 30f == 0f;
			int projTargetIndex = TargetWhoAmI;
			if (projectile.localAI[0] >= 60 * aiFactor || projTargetIndex < 0 || projTargetIndex >= 200)
			{ // If the index is past its limits, kill it
				projectile.Kill();
			}
			else if (Main.npc[projTargetIndex].active && !Main.npc[projTargetIndex].dontTakeDamage)
			{ // If the target is active and can take damage
				// Set the projectile's position relative to the target's center
				projectile.Center = Main.npc[projTargetIndex].Center - projectile.velocity * 2f;
				projectile.gfxOffY = Main.npc[projTargetIndex].gfxOffY;
				if (hitEffect)
				{ // Perform a hit effect here
					Main.npc[projTargetIndex].HitEffect(0, 1.0);
				}
			}
			else
			{ // Otherwise, kill the projectile
				projectile.Kill();
			}
		}

		public override bool OnTileCollide(Microsoft.Xna.Framework.Vector2 oldVelocity)
		{
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X;
			}
			if (projectile.velocity.Y != oldVelocity.Y)
			{
				projectile.velocity.Y = -oldVelocity.Y;
			}
			projectile.velocity *= 0.4f;
			return false;
		}

		public override void Kill(int timeLeft)
        {
			for (int d = 0; d < 10; d++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 225, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 0, default(Color), 0.75f);
			}
		}
	}
}