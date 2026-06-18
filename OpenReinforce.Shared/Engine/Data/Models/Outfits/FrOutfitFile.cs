using System.Xml.Serialization;

namespace OpenReinforce.Engine.Data.Models.Outfits;

[XmlRoot]
public sealed class FrOutfitFile : IFrDataRoot<FrOutfit>
{
    [XmlArray("Outfits")]
    [XmlArrayItem("Outfit")]
    public FrOutfit[]? Items { get; set; }
}