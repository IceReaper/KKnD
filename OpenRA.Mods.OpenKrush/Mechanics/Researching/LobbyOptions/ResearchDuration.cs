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

namespace OpenRA.Mods.OpenKrush.Mechanics.Researching.LobbyOptions
{
	using System.Collections.Generic;
	using OpenRA.Traits;

	[Desc("Selectable max tech level in lobby.")]
	public class ResearchDurationInfo : TraitInfo, ILobbyOptions
	{
		public const string Id = "ResearchDuration";

		IEnumerable<LobbyOption> ILobbyOptions.LobbyOptions(Ruleset rules)
		{
			var values = new Dictionary<string, string>();

			for (var i = 1; i <= 4; i++)
				values.Add(i.ToString(), $"{i * 100}%");

			yield return new LobbyOption(
				ResearchDurationInfo.Id,
				"Duration",
				"Research duration.",
				true,
				0,
				new ReadOnlyDictionary<string, string>(values),
				"1",
				false,
				ResearchUtils.LobbyOptionsCategory);
		}

		public override object Create(ActorInitializer init)
		{
			return new ResearchDuration();
		}
	}

	public class ResearchDuration : INotifyCreated
	{
		public int Duration { get; private set; }

		void INotifyCreated.Created(Actor self)
		{
			Duration = int.Parse(self.World.LobbyInfo.GlobalSettings.OptionOrDefault(ResearchDurationInfo.Id, "1"));
		}
	}
}
