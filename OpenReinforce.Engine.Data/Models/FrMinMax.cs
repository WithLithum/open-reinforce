// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Xml.Serialization;

namespace OpenReinforce.Engine.Data.Models
{
    public sealed class FrMinMax
    {
        [XmlAttribute("min")]
        public int Min { get; set; }

        [XmlAttribute("max")]
        public int Max { get; set; }
    }
}