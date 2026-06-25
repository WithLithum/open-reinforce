// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using System;
using System.Xml.Serialization;
using OpenReinforce.Engine.Data.Utilities;

namespace OpenReinforce.Engine.Data.Models.Regions
{
    public sealed class FrRegion : IFrKeyed
    {
        private static readonly Func<string, bool> ZoneValidator =
            x => string.IsNullOrWhiteSpace(x) || x.Length > 20;

        public string? Name { get; set; }

        [XmlArrayItem("Zone")]
        public string[]? Zones { get; set; }

        public string GetKey()
        {
            return Name ?? throw new InvalidOperationException("Name is null");
        }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name)
                && Zones != null
                && Zones.Length > 0
                && !Zones.ContainsAny(ZoneValidator);
        }
    }
}
