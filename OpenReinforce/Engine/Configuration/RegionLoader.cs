// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Data;
using OpenReinforce.Engine.Data.Models.Agencies;
using OpenReinforce.Engine.Data.Models.Regions;
using OpenReinforce.Utilities;
using Rage;

namespace OpenReinforce.Engine.Configuration;

internal sealed class RegionLoader
{
    private static readonly FrContainerDataLoader<FrRegion, FrRegionFile> XmlLoader =
        new();

    private readonly string _basePath;

    public RegionLoader(string basePath)
    {
        _basePath = basePath;
    }

    public void Load(LoadoutManager manager)
    {
        var xml = XmlLoader.Load(_basePath, "regions");
        if (xml == null || xml.Items == null || xml.Items.Length == 0)
        {
            Log.Warn("Empty regions configuration");
            return;
        }

        foreach (var region in xml.Items)
        {
            manager.AddRegion(region);
        }

        Log.Info($"{xml.Items.Length} regions");
    }
}
