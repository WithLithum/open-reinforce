// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Collections;

namespace OpenReinforce.Utilities.Collections;

internal sealed class EmptyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
{
    public static readonly EmptyDictionary<TKey, TValue> Instance = new();

    private static readonly IEnumerable<KeyValuePair<TKey, TValue>> EmptyEnumerator
        = [];

    public TValue this[TKey key] => throw new NotSupportedException();

    public int Count => 0;

    public IEnumerable<TKey> Keys => throw new NotImplementedException();

    public IEnumerable<TValue> Values => throw new NotImplementedException();

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return EmptyEnumerator.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public static IReadOnlyDictionary<TKey, TValue> Or(IReadOnlyDictionary<TKey, TValue>? list)
    {
        return list ?? Instance;
    }

    public bool ContainsKey(TKey key)
    {
        return false;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        value = default!;
        return false;
    }
}
