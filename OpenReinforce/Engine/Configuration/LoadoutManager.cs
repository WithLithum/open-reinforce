// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Data.Models.Regions;
using OpenReinforce.UI;
using OpenReinforce.Utilities;

namespace OpenReinforce.Engine.Configuration;

internal sealed class LoadoutManager
{
    private readonly Dictionary<string, string> _zoneNameToRegionName = [];

    private readonly Dictionary<string, Dictionary<ReinforceType, List<LoadoutInfo>>> _regionToLoadout = [];

    public void AddRegion(FrRegion region)
    {
        if (!region.IsValid())
        {
            throw new ArgumentException("Invalid region.", nameof(region));
        }

        foreach (var zone in region.Zones!)
        {
            if (_zoneNameToRegionName.ContainsKey(zone))
            {
                Log.WarnFormat(
                    $"Region '{region.Name}' attempting to register already registered '{zone}'");
                continue;
            }

            _zoneNameToRegionName[zone] = region.Name!;
        }
    }

    public void AddLoadout(string region, ReinforceType type, LoadoutInfo loadout)
    {
        if (!_regionToLoadout.TryGetValue(region, out Dictionary<ReinforceType, List<LoadoutInfo>> dict))
        {
            dict = [];
            _regionToLoadout[region] = dict;
        }

        if (!dict.TryGetValue(type, out List<LoadoutInfo> list))
        {
            list = [];
            dict[type] = list;
        }

        list.Add(loadout);
    }

    public LoadoutInfo? PickLoadout(string zone, ReinforceType type, bool transport = false)
    {
        if (!_zoneNameToRegionName.TryGetValue(zone, out var region)
            || !_regionToLoadout.TryGetValue(region, out var dict)
            || !dict.TryGetValue(type, out var list)
            || list.Count == 0)
        {
            return null;
        }

        return ItemSelector.PickByUniform(list,
            transport
            ? x => x.IsTransportUnit
            : x => x.IsBackupUnit);
    }
}
