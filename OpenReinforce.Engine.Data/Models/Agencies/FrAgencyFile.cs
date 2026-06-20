// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Xml.Serialization;

namespace OpenReinforce.Engine.Data.Models.Agencies
{
    [XmlRoot("Agencies")]
    public sealed class FrAgencyFile : IFrDataRoot<FrAgency>
    {
        [XmlElement("Agency")]
        public FrAgency[]? Items { get; set; }
    }
}
