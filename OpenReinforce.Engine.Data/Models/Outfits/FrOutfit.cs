// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using System;
using System.Xml.Serialization;

namespace OpenReinforce.Engine.Data.Models.Outfits
{
    public class FrOutfit : IFrIdentified, IFrKeyed
    {
        public string? Name { get; set; }

        public string? ScriptName { get; set; }

        [XmlArray]
        [XmlArrayItem("Variation")]
        public FrOutfitVariation[]? Variations { get; set; }

        public string GetKey()
        {
            return ScriptName ?? throw new InvalidOperationException("ScriptName is null");
        }
    }
}
