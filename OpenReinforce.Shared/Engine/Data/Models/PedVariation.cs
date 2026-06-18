using System.Xml.Serialization;

namespace OpenReinforce.Engine.Data.Models;

public sealed class PedVariation
{
    [XmlAttribute("id")]
    public int Slot { get; set; } 

    [XmlAttribute("drawable")]
    public int Drawable { get; set; }

    [XmlAttribute("texture")]
    public int Texture { get; set; }
}
