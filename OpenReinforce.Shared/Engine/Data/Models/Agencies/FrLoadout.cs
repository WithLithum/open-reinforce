using System.Xml.Serialization;

namespace OpenReinforce.Engine.Data.Models.Agencies;

public sealed record FrLoadout
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
