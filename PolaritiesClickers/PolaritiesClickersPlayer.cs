using PolaritiesClickers.Buffs;
using PolaritiesClickers.Items;
using PolaritiesClickers.NPCs;
using PolaritiesClickers.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
namespace PolaritiesClickers
{
	public partial class PolaritiesClickersPlayer : ModPlayer
	{
		public override void PostUpdateEquips()
		{
			if (player.HasBuff(ModContent.BuffType<RiskyClicks>()))
			{
				Mod ClickerClass = ModLoader.GetMod("ClickerClass");
				//float radius = (ClickerClass.(ClickerRadiusReal)) * 0.5f;
			}
		}
	}
}