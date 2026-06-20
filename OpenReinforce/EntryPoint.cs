// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

#if DEBUG

using Rage.Attributes;
using WithLithum.NativeWrapper;

namespace OpenReinforce;

public static class EntryPoint
{
    internal static bool IsPursuitInProgress { get; private set; }

    public static void Main()
    {
        OpenReinforcePlugin.Initialize(true);
    }

    public static void OnUnload(bool force)
    {
        OpenReinforcePlugin.Finally();
    }

    [ConsoleCommand]
    public static void TpSetPursuit(bool value)
    {
        IsPursuitInProgress = value;

        if (IsPursuitInProgress)
        {
            Natives.SetFakeWantedLevel(2);
        }
        else
        {
            Natives.SetFakeWantedLevel(0);
        }
    }
}
#endif