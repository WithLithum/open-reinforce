using System.Xml.Serialization;

namespace OpenReinforce.Engine.Data.Models.Agencies;

public sealed record FrAgency : IFrIdentified
{
    public string? Name { get; init; }

    public string? ShortName { get; init; }

    public string? TextureDictionary { get; init; }

    public string? TextureName { get; init; }

    public string? ScriptName { get; set; }

    public string? Inventory { get; init; }

    public string? Parent { get; init; }

    [XmlElement("Loadout")]
    public FrLoadout[]? Loadouts { get; init; }

    public bool ExcludeFromBackupMenu { get; init; }

    public string? BadgeModel { get; init; }

    public string? ShieldModel { get; init; }

    public string? EvidenceMarkerModel { get; init; }
}
