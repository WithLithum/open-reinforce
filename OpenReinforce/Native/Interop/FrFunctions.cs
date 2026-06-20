// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later
#if GTA
#if LSPDFR
using LSPD_First_Response.Mod.API;
#endif
using Rage;

namespace OpenReinforce.Native.Interop;

internal static class FrFunctions
{
#if !LSPDFR
    private static NotSupportedException CreateUnsupported()
    {
        return new NotSupportedException("This operation is not supported.");
    }
#endif

    internal static FrHandle CreatePursuit()
    {
#if LSPDFR
        return new FrHandle(Functions.CreatePursuit());
#else
        throw CreateUnsupported();
#endif
    }

    internal static FrHandle GetActivePursuit()
    {
#if LSPDFR
        return new FrHandle(Functions.GetActivePursuit());
#else
        throw CreateUnsupported();
#endif
    }

    internal static bool IsPursuitStillRunning(FrHandle pursuit)
    {
#if LSPDFR
        return Functions.IsPursuitStillRunning(pursuit.Inner);
#else
        throw CreateUnsupported();
#endif
    }

    internal static void PlayScannerAudioUsingPosition(string audioString,
        Vector3 position)
    {
#if LSPDFR
        Functions.PlayScannerAudioUsingPosition(audioString, position);
#else
        throw CreateUnsupported();
#endif
    }

    internal static void SetPedAsCop(Ped ped)
    {
#if LSPDFR
        Functions.SetPedAsCop(ped);
#else
        throw CreateUnsupported();
#endif
    }

    internal static void RandomizeMultiplayerModelAppearance(Ped ped)
    {
#if LSPDFR
        Functions.RandomizeMultiplayerModelAppearance(ped);
#else
        throw CreateUnsupported();
#endif
    }


    internal static bool IsPedInPursuit(Ped ped)
    {
#if LSPDFR
        return Functions.IsPedInPursuit(ped);
#else
        throw CreateUnsupported();
#endif
    }
}

#endif