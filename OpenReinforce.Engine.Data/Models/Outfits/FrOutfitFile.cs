// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Xml.Serialization;

namespace OpenReinforce.Engine.Data.Models.Outfits
{
    [XmlRoot("Outfits")]
    public sealed class FrOutfitFile : IFrDataRoot<FrOutfit>
    {
        [XmlElement("Outfit")]
        public FrOutfit[]? Items { get; set; }
    }
}