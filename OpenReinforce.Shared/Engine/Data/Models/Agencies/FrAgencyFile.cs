using System.Xml.Serialization;

namespace OpenReinforce.Engine.Data.Models.Agencies;

[XmlRoot("Agencies")]
public sealed class FrAgencyFile : IFrDataRoot<FrAgency>
{
    [XmlElement("Agency")]
    public FrAgency[]? Items { get; set; }
}
