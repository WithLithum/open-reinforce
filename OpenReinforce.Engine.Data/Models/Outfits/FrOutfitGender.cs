// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Xml.Serialization;

namespace OpenReinforce.Engine.Data.Models.Outfits
{
    public enum FrOutfitGender
    {
        [XmlEnum("male")]
        Male,
        [XmlEnum("female")]
        Female
    }
}