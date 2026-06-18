// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

#if GTA

using OpenReinforce.Engine;
using OpenReinforce.UI;
using Rage;
using RAGENativeUI;
using System.Windows.Forms;

namespace OpenReinforce;

internal static class OpenReinforcePlugin
{
    private static readonly CancellationTokenSource CancelSource = new();

    public static void Initialize()
    {
        ReinforceMenu.InitializeComponents();

        GameFiber.StartNew(static () =>
        {
            while (!CancelSource.IsCancellationRequested)
            {
                GameFiber.Yield();
                ReinforceMenu.Process();
            }
        }, "OpenReinforce - UI Thread");
        GameFiber.StartNew(static () =>
        {
            while (!CancelSource.IsCancellationRequested)
            {
                GameFiber.Yield();
                ResponseManager.Process();
            }
        }, "OpenReinforce - Game Thread");

        Game.DisplayHelp($"Press ~{Keys.B.GetInstructionalId()}~ to open Open Reinforce main menu.");
    }

    public static void Finally()
    {
        try
        {
            CancelSource.Cancel();
        }
        catch (Exception ex)
        {
            Game.LogTrivial($"OpenReinforce: Having problems terminating: {ex}");
        }

        ResponseManager.Cleanup();

        CancelSource.Dispose();
    }
}
#endif