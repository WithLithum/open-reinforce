using System.Xml.Serialization;

namespace OpenReinforce.Engine.Data.Models.Outfits;

public class FrOutfit : IFrIdentified
{
    public string? Name { get; set; }

    public string? ScriptName { get; set; }

    [XmlArray]
    [XmlArrayItem("Variation")]
    public FrOutfitVariation[]? Variations { get; set; }
}