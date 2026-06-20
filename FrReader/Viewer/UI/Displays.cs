// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace OpenReinforce.Viewer.UI;

internal static class Displays
{
    internal static void Run(IDisplay display,
        CancellationToken cancellationToken = default)
    {
        while (!display.Quit && !cancellationToken.IsCancellationRequested)
        {
            Console.Clear();
            display.Run();
        }
    }
}
