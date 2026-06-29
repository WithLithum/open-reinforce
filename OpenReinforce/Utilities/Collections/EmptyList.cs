// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Collections;

namespace OpenReinforce.Utilities.Collections;

internal sealed class EmptyList<T> : IReadOnlyList<T>
{
    public static readonly EmptyList<T> Instance = new();

    private static readonly IEnumerable<T> EmptyEnumerator = [];

    public T this[int index] => throw new ArgumentOutOfRangeException(nameof(index));

    public int Count => 0;

    public IEnumerator<T> GetEnumerator()
    {
        return EmptyEnumerator.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public static IReadOnlyList<T> Or(IReadOnlyList<T>? list)
    {
        return list ?? Instance;
    }
}
