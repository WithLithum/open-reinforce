// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

// Courtesy of xesdoog: https://github.com/xesdoog/JOAAT/blob/main/joaat.cs

namespace OpenReinforce.Utilities;

public static class Hasher
{
    public static uint Joaat(string key)
    {
        Span<char> buf = stackalloc char[key.Length];
        key.AsSpan().ToLowerInvariant(buf);

        uint hash = 0;

        for (int i = 0; i < buf.Length; i++)
        {
            hash += (uint)buf[i];
            hash += hash << 10;
            hash ^= hash >> 6;
        }

        hash += hash << 3;
        hash ^= hash >> 11;
        hash += hash << 15;

        return hash;
    }
}
