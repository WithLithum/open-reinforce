// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Data;
using OpenReinforce.Engine.Data.Models.Agencies;
using Rage;
using System.Net;

namespace OpenReinforce.Engine.Configuration;

internal sealed class ConfigurationReader
{
    private static readonly FrContainerDataLoader<FrAgency, FrAgencyFile> AgencyLoader =
        new();

    private readonly string _basePath;

    public ConfigurationReader(string basePath)
    {
        _basePath = basePath;
    }

    public void ReadLoadouts(LoadoutManager manager)
    {
        var data = AgencyLoader.Load(_basePath, "agency");
        if (data == null || data.Items == null)
        {
            throw new InvalidDataException("The agency data is invalid.");
        }

        var loadoutList = new List<LoadoutInfo>();
        var dict = ResolveAgencies(data.Items);
        foreach (var (key, item) in dict)
        {
            ConvertLoadouts(loadoutList, item);
        }
    }

    private void ConvertLoadouts(List<LoadoutInfo> loadoutList, FrAgency item)
    {
        if (item.Loadouts == null || item.Loadouts.Length == 0)
        {
            Game.LogTrivial($"Agency '{item.ScriptName}' has no loadouts");
            return;
        }

        foreach (var loadout in item.Loadouts)
        {
            if (!LoadoutInfo.TryConvertFrom(loadout, out var result))
            {
                continue;
            }

            loadoutList.Add(result);
        }
    }

    // Reserved
    private static string? ResolveString(Func<FrAgency, string?> func,
        IReadOnlyDictionary<string, FrAgency> sources,
        FrAgency item)
    {
        var value = func.Invoke(item);
        if (string.IsNullOrWhiteSpace(value))
        {
            if (string.IsNullOrWhiteSpace(item.Parent)
                || !sources.TryGetValue(item.Parent!, out var parent))
            {
                return null;
            }

            return func(parent);
        }

        return value;
    }

    private static IReadOnlyDictionary<string, FrAgency> ResolveAgencies(
        FrAgency[] agencies)
    {
        var dict = new Dictionary<string, FrAgency>(agencies.Length);

        for (int i = 0; i < agencies.Length; i++)
        {
            var item = agencies[i];
            if (item.ScriptName == null
                || dict.ContainsKey(item.ScriptName))
            {
                Game.LogTrivial($"Skipping invalid agency '{item.Name}' ('{item.ShortName}')");
                continue;
            }

            dict[item.ScriptName] = item;
        }

        return dict;
    }
}
