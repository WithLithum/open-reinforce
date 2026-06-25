// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Data.Models.Agencies;
using OpenReinforce.Engine.Data.Models.Response;
using OpenReinforce.UI;

namespace OpenReinforce.Engine.Configuration;

internal static class AgencyMapper
{
    internal static void MapAgencies(ResponseTable table,
        IEnumerable<FrAgency> agencies)
    {
        foreach (var agency in agencies)
        {
            if (agency.Loadouts == null)
            {
                continue;
            }

            // Ensure the appropriate backup type is mapped to this agency.
            MapLoadoutPass(table.LocalPatrol, ReinforceType.LocalPatrol, agency);
            MapLoadoutPass(table.StatePatrol, ReinforceType.StatePatrol, agency);
            // TODO all others
        }
    }

    private static void MapLoadoutPass(IReadOnlyDictionary<string, string>? mapping,
        ReinforceType type,
        FrAgency agency)
    {
        if (mapping == null)
        {
            return;
        }

        foreach (var pair in mapping)
        {
            if (pair.Value != agency.ScriptName)
            {
                continue;
            }

            foreach (var loadout in agency.Loadouts!)
            {
                if (!LoadoutInfo.TryConvertFrom(loadout, out var info))
                {
                    continue;
                }

                OpenReinforcePlugin.LoadoutManager.AddLoadout(pair.Key, type, info);
            }
        }
    }

}
