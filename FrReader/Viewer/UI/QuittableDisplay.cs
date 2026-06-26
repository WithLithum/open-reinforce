// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace OpenReinforce.Viewer.UI;

internal abstract class QuittableDisplay : IDisplay
{
    public bool Quit { get; protected set; }

    public virtual void Run()
    {
        if (Console.ReadKey(true).Key == ConsoleKey.Q)
        {
            Quit = true;
        }
    }
}
