// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Xml.Serialization;

namespace OpenReinforce.Engine.Data.Models.Agencies
{
    public sealed class FrAgency : IFrIdentified
    {
        public string? Name { get; set; }

        public string? ShortName { get; set; }

        public string? TextureDictionary { get; set; }

        public string? TextureName { get; set; }

        public string? ScriptName { get; set; }

        public string? Inventory { get; set; }

        public string? Parent { get; set; }

        [XmlElement("Loadout")]
        public FrLoadout[]? Loadouts { get; set; }

        public bool ExcludeFromBackupMenu { get; set; }

        public string? BadgeModel { get; set; }

        public string? ShieldModel { get; set; }

        public string? EvidenceMarkerModel { get; set; }
    }

}