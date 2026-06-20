// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace OpenReinforce.Viewer.UI;

public interface IDisplay
{
    bool Quit { get; }

    void Run();
}
