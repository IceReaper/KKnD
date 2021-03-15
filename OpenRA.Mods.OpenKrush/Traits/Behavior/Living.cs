#region Copyright & License Information

/*
 * Copyright 2007-2021 The OpenKrush Developers (see AUTHORS)
 * This file is part of OpenKrush, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */

#endregion

namespace OpenRA.Mods.OpenKrush.Traits.Behavior
{
	using Common.Traits;
	using Common.Traits.Render;
	using OpenRA.Traits;

	[Desc("Makes infantry feel more alive by randomly rotating or playing an animation when idle.")]
	public class LivingInfo : TraitInfo, Requires<MobileInfo>, Requires<WithSpriteBodyInfo>
	{
		[Desc("Chance per tick the actor rotates to a random direction.")]
		public readonly int RotationChance = 1000;

		[Desc("Chance per tick the actor moves to a different free subcell in its cell.")]
		public readonly int SubcellMoveChance = 1000;

		[Desc("Chance per tick the actor triggers its bored sequence.")]
		public readonly int BoredChance = 5000;

		[Desc("Sequence to play when idle.")]
		public readonly string BoredSequence = "bored";

		public override object Create(ActorInitializer init)
		{
			return new Living(init, this);
		}
	}

	public class Living : ITick
	{
		private readonly LivingInfo info;
		private readonly Mobile mobile;
		private readonly WithSpriteBody withSpriteBody;

		public Living(ActorInitializer init, LivingInfo info)
		{
			this.info = info;
			mobile = init.Self.Trait<Mobile>();
			withSpriteBody = init.Self.Trait<WithSpriteBody>();
		}

		void ITick.Tick(Actor self)
		{
			if (self.CurrentActivity != null)
				return;

			if (info.BoredSequence != null && self.World.SharedRandom.Next(0, info.BoredChance) == 0)
				withSpriteBody.PlayCustomAnimation(self, info.BoredSequence);
			else if (info.RotationChance > 0 && self.World.SharedRandom.Next(0, info.RotationChance) == 0)
				mobile.Facing = WAngle.FromFacing(self.World.SharedRandom.Next(0x00, 0xff));
			else if (mobile.Info.LocomotorInfo.SharesCell && info.SubcellMoveChance > 0 && self.World.SharedRandom.Next(0, info.SubcellMoveChance) == 0)
			{
				// TODO openra does not support Move from subcell to subcell in the same cell.
			}
		}
	}
}
