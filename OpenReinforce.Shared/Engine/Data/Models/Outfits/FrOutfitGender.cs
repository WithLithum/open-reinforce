using System.Xml.Serialization;

namespace OpenReinforce.Engine.Data.Models.Outfits;

public enum FrOutfitGender
{
    [XmlEnum("male")]
    Male,
    [XmlEnum("female")]
    Female
}