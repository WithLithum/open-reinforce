// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Configuration;
using OpenReinforce.Engine.Data;
using OpenReinforce.Engine.Data.Models.Agencies;
using OpenReinforce.Engine.Data.Models.Regions;
using OpenReinforce.Engine.Data.Models.Response;
using OpenReinforce.Engine.Data.Utilities;
using OpenReinforce.Utilities;

namespace OpenReinforce.Engine;

internal static class Initializer
{
    public static bool Initialize()
    {
        // Determine data directory.
        var dataDir = Path.GetFullPath("lspdfr\\data");
        if (!Directory.Exists(dataDir))
        {
            return false;
        }

        LoadRegionsInternal(dataDir);

        var backups = FrDirectory.ReadData(dataDir,
            "backup",
            static r => ResponseTableReader.ReadTable(r),
            static (a, b) => a.Merge(b));
        if (backups == null)
        {
            Log.Info("Failed to load 'backup.xml' or any of its customs");
            return false;
        }

        if (!LoadAgenciesInternal(dataDir, backups))
        {
            return false;
        }

        return true;
    }

    private static bool LoadAgenciesInternal(string dataDir, ResponseTable backups)
    {
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
}
