// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

#if GTA

using OpenReinforce.Engine;
using OpenReinforce.Shared.Engine.Scene;
using OpenReinforce.UI;
using Rage;
using RAGENativeUI;
using System.Windows.Forms;

namespace OpenReinforce;

internal static class OpenReinforcePlugin
{
    private static readonly CancellationTokenSource CancelSource = new();

    public static DismissManager DismissManager { get; } = new();
    public static WatchManager WatchManager { get; } = new();

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
                DismissManager.Process();
                WatchManager.Process();
            }
        }, "OpenReinforce - Game Thread");
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
        DismissManager.Cleanup();
        WatchManager.Cleanup();

        CancelSource.Dispose();
    }
}
#endif