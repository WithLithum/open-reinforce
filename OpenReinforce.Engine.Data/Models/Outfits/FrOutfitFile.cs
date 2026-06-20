// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Xml.Serialization;

namespace OpenReinforce.Engine.Data.Models.Outfits
{
    [XmlRoot]
    public sealed class FrOutfitFile : IFrDataRoot<FrOutfit>
    {
        [XmlArray("Outfits")]
        [XmlArrayItem("Outfit")]
        public FrOutfit[]? Items { get; set; }
    }
}