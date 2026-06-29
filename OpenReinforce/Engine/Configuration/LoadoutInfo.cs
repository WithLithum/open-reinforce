// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine.Data.Models.Agencies;
using OpenReinforce.Utilities;
using Rage;
using System.Diagnostics.CodeAnalysis;

namespace OpenReinforce.Engine.Configuration;

internal sealed record LoadoutInfo : IChanced
{
    public int Chance { get; init; }

    public int MinimumPeds { get; init; }

    public int MaximumPeds { get; init; }

    public required IReadOnlyList<LoadoutVehicleInfo> Vehicles { get; init; }

    public required IReadOnlyList<LoadoutPedInfo> Peds { get; init; }

    public bool IsBackupUnit { get; init; }

    public bool IsTransportUnit { get; init; }

    public static bool TryConvertFrom(FrLoadout fr, [NotNullWhen(true)] out LoadoutInfo? result)
    {
        // TODO swat flag
        if (fr.NumPeds == null
            || fr.Vehicles == null
            || fr.Peds == null
            || fr.NumPeds.Min <= 0
            || fr.NumPeds.Min > fr.NumPeds.Max)
        {
            Game.LogTrivial($"Skipping invalid loadout '{fr.Name}'");
            result = null;
            return false;
        }

        // Flags support
        var isBackup = true;
        var isTransport = false;
        if (fr.Flags != null)
        {
            foreach (var flag in fr.Flags)
            {
                isBackup = isBackup || flag == FrLoadoutOptions.RespondsAsBackup;
                isTransport = isTransport || flag == FrLoadoutOptions.RespondsAsTransport;
            }
        }

        result = new LoadoutInfo
        {
            Chance = fr.Chance,
            MinimumPeds = fr.NumPeds.Min,
            MaximumPeds = fr.NumPeds.Max,
            Vehicles = ConvertLoadoutVehicles(fr.Vehicles),
            Peds = ConvertLoadoutPeds(fr.Peds),
            IsBackupUnit = isBackup,
            IsTransportUnit = isTransport
        };
        return true;
    }

    private static IReadOnlyList<LoadoutVehicleInfo> ConvertLoadoutVehicles(FrLoadoutVehicle[] vehicles)
    {
        var list = new List<LoadoutVehicleInfo>(vehicles.Length);
        foreach (var vehicle in vehicles)
        {
            if (LoadoutVehicleInfo.TryConvertFrom(vehicle, out var loadoutVeh))
            {
                list.Add(loadoutVeh);
            }
        }
        return list;
    }

    private static IReadOnlyList<LoadoutPedInfo> ConvertLoadoutPeds(FrLoadoutPed[] peds)
    {
        var list = new List<LoadoutPedInfo>(peds.Length);
        foreach (var ped in peds)
        {
            if (LoadoutPedInfo.TryConvert(ped, out var result))
            {
                list.Add(result);
            }
        }
        return list;
    }

}
