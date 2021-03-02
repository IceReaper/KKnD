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

using System.Linq;
using OpenRA.Mods.Common.Widgets;
using OpenRA.Mods.Kknd.Mechanics.Researching.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.Kknd.Widgets.Ingame.Buttons
{
	public class RadarButtonWidget : ButtonWidget
	{
		private bool hasRadar;

		public RadarButtonWidget(SidebarWidget sidebar)
			: base(sidebar, "button")
		{
			TooltipTitle = "Radar";
		}

		public override bool HandleKeyPress(KeyInput e)
		{
			if (IsUsable() && !e.IsRepeat && e.Event == KeyInputEvent.Down
				&& e.Key == Game.ModData.Hotkeys["Radar"].GetValue().Key && e.Modifiers == Game.ModData.Hotkeys["Radar"].GetValue().Modifiers)
			{
				Active = !Active;
				sidebar.IngameUi.Radar.Visible = Active;
				return true;
			}

			return false;
		}

		protected override bool HandleLeftClick(MouseInput mi)
		{
			if (base.HandleLeftClick(mi))
			{
				sidebar.IngameUi.Radar.Visible = Active;

				return true;
			}

			return false;
		}

		protected override bool IsUsable()
		{
			return hasRadar;
		}

		public override void Tick()
		{
			hasRadar = false;
			var showStances = PlayerRelationship.None;

			foreach (var e in sidebar.IngameUi.World.ActorsHavingTrait<ProvidesResearchableRadar>().Where(a => a.Owner == sidebar.IngameUi.World.LocalPlayer))
			{
				var providesResearchableRadarInfo = e.Info.TraitInfo<ProvidesResearchableRadarInfo>();
				var researchable = e.Trait<Researchable>();

				if (researchable.Level < providesResearchableRadarInfo.Level)
					continue;

				hasRadar = true;

				if (researchable.Level >= providesResearchableRadarInfo.AllyLevel)
					showStances |= PlayerRelationship.Ally;

				if (researchable.Level >= providesResearchableRadarInfo.EnemyLevel)
					showStances |= PlayerRelationship.Enemy;
			}

			sidebar.IngameUi.Radar.ShowStances = showStances;

			if (!hasRadar)
				sidebar.IngameUi.Radar.Visible = Active = false;
		}

		protected override void DrawContents()
		{
			sidebar.Buttons.PlayFetchIndex("radar", () => 0);
			WidgetUtils.DrawSHPCentered(sidebar.Buttons.Image, center + new int2(0, Active ? 1 : 0), sidebar.IngameUi.Palette);
		}
	}
}
