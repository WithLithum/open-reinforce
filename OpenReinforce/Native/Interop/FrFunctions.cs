// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using LSPD_First_Response.Mod.API;
using Rage;

namespace OpenReinforce.Native.Interop;

internal static class FrFunctions
{
    internal static FrHandle CreatePursuit()
    {
        return new FrHandle(Functions.CreatePursuit());
    }

    internal static FrHandle GetActivePursuit()
    {
        return new FrHandle(Functions.GetActivePursuit());
    }

    internal static bool IsPursuitStillRunning(FrHandle pursuit)
    {
        return pursuit.Inner != null && Functions.IsPursuitStillRunning((LHandle)pursuit.Inner);
    }

    internal static void PlayScannerAudioUsingPosition(string audioString,
        Vector3 position)
    {
        Functions.PlayScannerAudioUsingPosition(audioString, position);
    }

    internal static void SetPedAsCop(Ped ped)
    {
        Functions.SetPedAsCop(ped);
    }

    internal static void RandomizeMultiplayerModelAppearance(Ped ped)
    {
        Functions.RandomizeMultiplayerModelAppearance(ped);
    }

    internal static bool IsPedInPursuit(Ped ped)
    {
        return Functions.IsPedInPursuit(ped);
    }
}
