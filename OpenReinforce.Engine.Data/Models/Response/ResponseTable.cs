// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using System;
using System.Collections.Generic;

namespace OpenReinforce.Engine.Data.Models.Response
{
    public sealed class ResponseTable
    {
        public IReadOnlyDictionary<string, string>? LocalPatrol { get; set; }

        public IReadOnlyDictionary<string, string>? StatePatrol { get; set; }

        public IReadOnlyDictionary<string, string>? LocalSWAT { get; set; }

        public IReadOnlyDictionary<string, string>? NooseSWAT { get; set; }

        public IReadOnlyDictionary<string, string>? LocalAir { get; set; }

        public IReadOnlyDictionary<string, string>? NooseAir { get; set; }

        public IReadOnlyDictionary<string, string>? Ambulance { get; set; }

        public IReadOnlyDictionary<string, string>? Firetruck { get; set; }

        public ResponseTable Merge(ResponseTable other)
        {
            return new ResponseTable
            {
                LocalPatrol = MergeOne(LocalPatrol, other.LocalPatrol),
                StatePatrol = MergeOne(StatePatrol, other.StatePatrol),
                LocalSWAT = MergeOne(LocalSWAT, other.LocalSWAT),
                NooseSWAT = MergeOne(NooseSWAT, other.NooseSWAT),
                LocalAir = MergeOne(LocalAir, other.LocalAir),
                NooseAir = MergeOne(NooseAir, other.NooseAir),
                Ambulance = MergeOne(Ambulance, other.Ambulance),
                Firetruck = MergeOne(Firetruck, other.Firetruck)
            };
        }

        private static IReadOnlyDictionary<string, string>? MergeOne(
            IReadOnlyDictionary<string, string>? a,
            IReadOnlyDictionary<string, string>? b)
        {
            if (a == null)
            {
                return b;
            }

            if (b == null)
            {
                return a;
            }

            var dict = new Dictionary<string, string>(a.Count);
            foreach (var pair in a)
            {
                if (!b.TryGetValue(pair.Key, out string? writeValue))
                {
                    writeValue = pair.Value;
                }

                dict.Add(pair.Key, writeValue);
            }

            return dict;
        }
    }
}
