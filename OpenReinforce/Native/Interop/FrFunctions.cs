// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

#if LSPDFR
using LSPD_First_Response.Mod.API;
#endif
using Rage;

namespace OpenReinforce.Native.Interop;

internal static class FrFunctions
{
    internal static FrHandle CreatePursuit()
    {
#if LSPDFR
        return new FrHandle(Functions.CreatePursuit());
#else
        return FrHandle.Null;
#endif
    }

    internal static FrHandle GetActivePursuit()
    {
#if LSPDFR
        return new FrHandle(Functions.GetActivePursuit());
#else
        return FrHandle.Null;
#endif
    }

    internal static bool IsPursuitStillRunning(FrHandle pursuit)
    {
#if LSPDFR
        return pursuit.Inner != null && Functions.IsPursuitStillRunning((LHandle)pursuit.Inner);
#else
        return false;
#endif
    }

    internal static void PlayScannerAudioUsingPosition(string audioString,
        Vector3 position)
    {
#if LSPDFR
        Functions.PlayScannerAudioUsingPosition(audioString, position);
#endif
    }

    internal static void SetPedAsCop(Ped ped)
    {
#if LSPDFR
        Functions.SetPedAsCop(ped);
#endif
    }

    internal static void RandomizeMultiplayerModelAppearance(Ped ped)
    {
#if LSPDFR
        Functions.RandomizeMultiplayerModelAppearance(ped);
#endif
    }

    internal static bool IsPedInPursuit(Ped ped)
    {
#if LSPDFR
        return Functions.IsPedInPursuit(ped);
#else
        return false;
#endif
    }
}
