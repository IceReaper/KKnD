﻿#region Copyright & License Information
/*
 * Copyright 2007-2021 The KKnD Developers (see AUTHORS)
 * This file is part of KKnD, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using OpenRA.Mods.Kknd.Traits.Docking;
using OpenRA.Mods.Kknd.Traits.Resources;

namespace OpenRA.Mods.Kknd.Activities
{
	class TankerCycle : Docking
	{
		private bool abortByCancel;

		[ObjectCreator.UseCtor]
		public TankerCycle(Actor dockableActor, Dockable dockable, Actor dockActor, Dock dock)
			: base(dockableActor, dockable, dockActor, dock)
		{
			var tanker = (Tanker)dockable;

			if (dockActor == null)
			{
				DockingState = DockingState.Undocked;
				return;
			}

			if (dockActor.Info.HasTraitInfo<DrillrigInfo>())
				tanker.Drillrig = dockActor;
			else
				tanker.PowerStation = dockActor;
		}

		public override bool Tick(Actor self)
		{
			var result = base.Tick(self);

			if (!abortByCancel && shouldCancel)
				shouldCancel = false;

			if (!result)
				return false;

			if (shouldCancel)
				return true;

			var tanker = (Tanker)Dockable;

			if (tanker.Current == tanker.Maximum)
			{
				if (!tanker.IsValidPowerStation(tanker.PowerStation))
					tanker.AssignNearestPowerStation();

				if (tanker.PowerStation == null)
					return true;

				DockActor = tanker.PowerStation;
				Dock = DockActor.Trait<Dock>();
				DockingState = DockingState.Approaching;
			}
			else
			{
				if (!tanker.IsValidDrillrig(tanker.Drillrig))
					tanker.AssignNearestDrillrig();

				if (tanker.Drillrig == null)
					return true;

				DockActor = tanker.Drillrig;
				Dock = DockActor.Trait<Dock>();
				DockingState = DockingState.Approaching;
			}

			return false;
		}

		public override void Cancel(Actor self, bool keepQueue = false)
		{
			abortByCancel = true;
			base.Cancel(self, keepQueue);
		}
	}
}
