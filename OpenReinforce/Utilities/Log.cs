// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using Rage;

namespace OpenReinforce.Utilities;

internal static class Log
{
    internal static void Info(string message)
    {
        Game.LogTrivial($"[OpenReinforce/INFO] {message}");
    }

    internal static void Warn(string message)
    {
        Game.LogTrivial($"[OpenReinforce/WARN] {message}");
    }

    internal static void WarnFormat(FormattableString message)
    {
        Game.LogTrivial($"[OpenReinforce/WARN] {message}");
    }
    internal static void Error(string message)
    {
        Game.LogTrivial($"[OpenReinforce/ERROR] {message}");
    }

    internal static void Error(Exception ex, string message)
    {
        Game.LogTrivial($"[OpenReinforce/ERROR] {message}");
        Game.LogTrivial(ex.ToString());
    }
}
