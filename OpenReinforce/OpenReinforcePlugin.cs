// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using OpenReinforce.Engine;
using OpenReinforce.Engine.Configuration;
using OpenReinforce.Engine.Scene;
using OpenReinforce.UI;
using Rage;

namespace OpenReinforce;

internal static class OpenReinforcePlugin
{
    private static readonly CancellationTokenSource CancelSource = new();

    public static bool IsTestPlugin { get; private set; }

    public static DismissManager DismissManager { get; } = new();
    public static WatchManager WatchManager { get; } = new();
    public static LoadoutManager LoadoutManager { get; } = new();

    public static void Initialize(bool testPlugin)
    {
        IsTestPlugin = testPlugin;

        Initializer.Initialize();
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
