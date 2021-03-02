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

using System.Collections.Generic;
using OpenRA.Traits;

namespace OpenRA.Mods.Kknd.Traits.Resources
{
	[Desc("Selectable oil burn behavior in lobby.")]
	public class OilpatchBurnInfo : TraitInfo, ILobbyOptions
	{
		IEnumerable<LobbyOption> ILobbyOptions.LobbyOptions(Ruleset rules)
		{
			var values = new Dictionary<string, string>();

			for (var i = 0; i <= 100; i += 20)
				values.Add(i.ToString(), i + "%");

			yield return new LobbyOption(
				"OilpatchBurn",
				"Oil Burn",
				"Percent amount of oil to burn when shot.",
				true,
				0,
				new ReadOnlyDictionary<string, string>(values),
				"0",
				false);
		}

		public override object Create(ActorInitializer init) { return new OilpatchBurn(); }
	}

	public class OilpatchBurn : INotifyCreated
	{
		public int Amount { get; set; }

		void INotifyCreated.Created(Actor self)
		{
			Amount = int.Parse(self.World.LobbyInfo.GlobalSettings.OptionOrDefault("OilpatchBurn", "0"));
		}
	}
}
