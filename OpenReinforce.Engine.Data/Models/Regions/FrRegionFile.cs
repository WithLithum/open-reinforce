// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Xml.Serialization;

namespace OpenReinforce.Engine.Data.Models.Regions
{
    [XmlRoot("Regions")]
    public sealed class FrRegionFile : IFrDataRoot<FrRegion>
    {
        [XmlElement("Region")]
        public FrRegion[]? Items { get; set; }
    }
}
