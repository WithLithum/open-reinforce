// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Xml.Serialization;

namespace OpenReinforce.Engine.Data.Models
{
    public sealed class PedVariation
    {
        [XmlAttribute("id")]
        public int Slot { get; set; }

        [XmlAttribute("drawable")]
        public int Drawable { get; set; }

        [XmlAttribute("texture")]
        public int Texture { get; set; }
    }
}
