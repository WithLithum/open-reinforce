using System.Xml.Serialization;

namespace OpenReinforce.Engine.Data.Models.Outfits;

public sealed class FrOutfitVariation : IFrIdentified
{
    public string? Name { get; set; }

    public string? ScriptName { get; set; }

    public string? Base { get; set; }

    public FrOutfitGender Gender { get; set; }

    [XmlArray]
    [XmlArrayItem("Component")]
    public PedVariation[]? Components { get; set; }

    [XmlArray]
    [XmlArrayItem("Prop")]
    public PedVariation[]? Props { get; set; }
}