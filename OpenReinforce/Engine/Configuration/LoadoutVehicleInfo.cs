// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using OpenReinforce.Engine.Data.Models.Agencies;
using OpenReinforce.Utilities;
using Rage;

namespace OpenReinforce.Engine.Configuration;

internal sealed record LoadoutVehicleInfo : IChanced
{
    public int Chance { get; init; }

    public required uint ModelHash { get; init; }

    public bool ModkitLiveries { get; init; }

    public IReadOnlyList<int>? Liveries { get; init; }

    public IReadOnlyList<int>? Extras { get; init; }

    public string? Weapon { get; init; }

    #region Conversion Helpers

    private static IReadOnlyList<int> EvaluateExtras(FrLoadoutVehicle fr)
    {
        Span<int> extraList = stackalloc int[15];
        var counter = 0;

        EvaluateOneExtra(ref extraList, ref counter, 1, fr.Extra1);
        EvaluateOneExtra(ref extraList, ref counter, 2, fr.Extra2);
        EvaluateOneExtra(ref extraList, ref counter, 3, fr.Extra3);
        EvaluateOneExtra(ref extraList, ref counter, 4, fr.Extra4);
        EvaluateOneExtra(ref extraList, ref counter, 5, fr.Extra5);
        EvaluateOneExtra(ref extraList, ref counter, 6, fr.Extra6);
        EvaluateOneExtra(ref extraList, ref counter, 7, fr.Extra7);
        EvaluateOneExtra(ref extraList, ref counter, 8, fr.Extra8);
        EvaluateOneExtra(ref extraList, ref counter, 9, fr.Extra9);
        EvaluateOneExtra(ref extraList, ref counter, 10, fr.Extra10);
        EvaluateOneExtra(ref extraList, ref counter, 11, fr.Extra11);
        EvaluateOneExtra(ref extraList, ref counter, 12, fr.Extra12);
        EvaluateOneExtra(ref extraList, ref counter, 13, fr.Extra13);
        EvaluateOneExtra(ref extraList, ref counter, 14, fr.Extra14);
        EvaluateOneExtra(ref extraList, ref counter, 15, fr.Extra15);

        var resultArray = new int[counter];
        extraList[..counter].CopyTo(resultArray);
        return resultArray;
    }

    private static void EvaluateOneExtra(ref Span<int> buf, ref int counter, int number, bool flag)
    {
        if (flag)
        {
            buf[counter++] = number;
        }
    }

    private static bool Validate(FrLoadoutVehicle fr)
    {
        if (string.IsNullOrWhiteSpace(fr.Model) || fr.Model!.Length >= 30)
        {
            return false;
        }

        return true;
    }

    #endregion

    public static bool TryConvertFrom(FrLoadoutVehicle fr, [NotNullWhen(true)] out LoadoutVehicleInfo? result)
    {
        if (!Validate(fr))
        {
            Game.LogTrivial($"Skipping vehicle (model '{fr.Model ?? "N/A"}'), invalid configuration");
            result = null;
            return false;
        }

        result = new LoadoutVehicleInfo
        {
            Chance = fr.Chance,
            ModelHash = Hasher.Joaat(fr.Model ?? throw new ArgumentException("Model is null",
                nameof(fr))),
            ModkitLiveries = fr.UseModKitLiveries,
            Liveries = fr.LiveryMulti?.Split(',')?.Select(int.Parse)?.ToList() ?? [fr.Livery],
            Extras = EvaluateExtras(fr),
            Weapon = fr.Weapon,
        };
        return true;
    }
}
