// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Configuration;
using OpenReinforce.Engine.Configuration.Outfits;
using OpenReinforce.Engine.Data;
using OpenReinforce.Engine.Data.Models.Agencies;
using OpenReinforce.Engine.Data.Models.Outfits;
using OpenReinforce.Engine.Data.Models.Regions;
using OpenReinforce.Engine.Data.Models.Response;
using OpenReinforce.Engine.Data.Utilities;
using OpenReinforce.Engine.Entities;
using OpenReinforce.Utilities;

namespace OpenReinforce.Engine;

internal static class Initializer
{
    public static bool Initialize()
    {
        Log.Info("Initializing OpenReinforce");

        // Determine data directory.
        var dataDir = Path.GetFullPath("lspdfr\\data");
        if (!Directory.Exists(dataDir))
        {
            return false;
        }

        Log.Info($"LSPDFR data: {dataDir}");

        if (!LoadRegionsInternal(dataDir))
        {
            return false;
        }

        Log.Info("Loading backups");
        var backups = FrDirectory.ReadData(dataDir,
            "backup",
            static r => ResponseTableReader.ReadTable(r),
            static (a, b) => a.Merge(b));
        if (backups == null)
        {
            Log.Error("Failed to load 'backup.xml' or any of its customs");
            return false;
        }

        if (!LoadAgenciesInternal(dataDir, backups)
            || !LoadOutfitsInternal(dataDir))
        {
            return false;
        }

        return true;
    }

    private static bool LoadAgenciesInternal(string dataDir, ResponseTable backups)
    {
        Log.Info("Loading agencies");
        FrAgencyFile? agencyFile;

        try
        {
            var loader = new FrContainerDataLoader<FrAgency, FrAgencyFile>();
            agencyFile = loader.Load(dataDir, "agency");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to load 'agency.xml' or any of its customs");
            return false;
        }

        AgencyMapper.MapAgencies(backups, agencyFile.Items ?? []);
        return true;
    }

    private static bool LoadRegionsInternal(string dataDir)
    {
        Log.Info("Loading regions");
        try
        {
            var regionLoader = new FrContainerDataLoader<FrRegion, FrRegionFile>();
            var regionFile = regionLoader.Load(dataDir, "regions");

            foreach (var region in regionFile.Items ?? [])
            {
                OpenReinforcePlugin.LoadoutManager.AddRegion(region);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to load 'backup.xml' or any of its customs");
            return false;
        }

        return true;
    }

    private static bool LoadOutfitsInternal(string dataDir)
    {
        Log.Info("Loading outfits");
        try
        {
            var loader = new FrContainerDataLoader<FrOutfit, FrOutfitFile>();
            var outfits = loader.Load(dataDir, "outfits");

            foreach (var outfit in outfits.Items ?? [])
            {
                var groupName = outfit.ScriptName;
                if (string.IsNullOrWhiteSpace(groupName)
                    || !OutfitReference.IsOutfitNameValid(groupName!))
                {
                    Log.Warn($"Missing or invalid outfit group name '{groupName ?? "[N/A]"}'");
                    continue;
                }

                // Convert all variants
                var counter = 0;
                foreach (var variant in outfit.Variations ?? [])
                {
                    var varName = variant.ScriptName;
                    if (string.IsNullOrWhiteSpace(varName)
                        || !OutfitReference.IsOutfitNameValid(varName!))
                    {
                        // Assign a name that cannot be used to reference the variation.
                        varName = $"\0__UNIFORM_ONLY__{counter++}";
                    }

                    OpenReinforcePlugin.OutfitManager.Add(varName!,
                        groupName!,
                        PedOutfit.ConvertFrom(variant));
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to load 'backup.xml' or any of its customs");
            return false;
        }

        return true;
    }
}
