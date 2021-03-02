#region Copyright & License Information
/*
 * Copyright 2007-2021 The KKnD Developers (see AUTHORS)
 * This file is part of KKnD, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using OpenRA.Mods.Common.Orders;
using OpenRA.Mods.Kknd.Mechanics.Researching.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.Kknd.Mechanics.Researching.Orders
{
	public class ResearchOrderTargeter : UnitOrderTargeter
	{
		public const string Id = "Research";

		private readonly string cursorAllowed;
		private readonly string cursorForbidden;

		public ResearchOrderTargeter(string cursorAllowed, string cursorForbidden)
			: base(Id, 6, cursorAllowed, false, true)
		{
			this.cursorAllowed = cursorAllowed;
			this.cursorForbidden = cursorForbidden;
		}

		public override bool CanTargetActor(Actor self, Actor target, TargetModifiers modifiers, ref string cursor)
		{
			var action = ResearchUtils.GetAction(self, target);

			if (action == ResearchAction.Start)
			{
				cursor = cursorAllowed;
				return true;
			}

			if (action == ResearchAction.Stop)
			{
				cursor = cursorForbidden;
				return true;
			}

			cursor = null;
			return false;
		}

		public override bool CanTargetFrozenActor(Actor self, FrozenActor target, TargetModifiers modifiers, ref string cursor)
		{
			return false;
		}
	}
}
