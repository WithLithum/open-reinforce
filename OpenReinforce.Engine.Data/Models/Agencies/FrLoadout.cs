// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Xml.Serialization;

namespace OpenReinforce.Engine.Data.Models.Agencies
{
    public sealed class FrLoadout
    {
        [XmlAttribute("chance")]
        public int Chance { get; set; }

        public string? Name { get; set; }

        [XmlArrayItem("Vehicle")]
        public FrLoadoutVehicle[]? Vehicles { get; set; }

        [XmlArrayItem("Ped")]
        public FrLoadoutPed[]? Peds { get; set; }

        public FrMinMax? NumPeds { get; set; }

        [XmlArrayItem("Flag")]
        public FrLoadoutOptions[]? Flags { get; set; }
    }
}