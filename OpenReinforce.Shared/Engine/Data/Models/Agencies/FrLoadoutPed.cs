using System.Xml;
using System.Xml.Serialization;

namespace OpenReinforce.Engine.Data.Models.Agencies;

public sealed record FrLoadoutPed
{
    [XmlText]
    public string? Model { get; set; }

    [XmlAttribute("chance")]
    public int Chance { get; set; }

    [XmlAttribute("outfit")]
    public string? Outfit { get; set; }

    [XmlAttribute("inventory")]
    public string? Inventory { get; set; }

    [XmlAttribute("randomizeProps")]
    public bool RandomizeProps { get; set; }

    [XmlElement("comp_head")]
    public int ComponentHead { get; set; } = -999;
    [XmlElement("comp_tex_head")]
    public int TextureHead { get; set; } = -999;

    [XmlElement("comp_berd")]
    public int ComponentBeard { get; set; } = -999;
    [XmlElement("comp_tex_berd")]
    public int TextureBeard { get; set; } = -999;

    [XmlElement("comp_hair")]
    public int ComponentHair { get; set; } = -999;
    [XmlElement("comp_tex_hair")]
    public int TextureHair { get; set; } = -999;

    [XmlElement("comp_uppr")]
    public int ComponentUpper { get; set; } = -999;
    [XmlElement("comp_tex_uppr")]
    public int TextureUpper { get; set; } = -999;

    [XmlElement("comp_lowr")]
    public int ComponentLower { get; set; } = -999;
    [XmlElement("comp_tex_lowr")]
    public int TextureLower { get; set; } = -999;

    [XmlElement("comp_hand")]
    public int ComponentHand { get; set; } = -999;
    [XmlElement("comp_tex_hand")]
    public int TextureHand { get; set; } = -999;

    [XmlElement("comp_feet")]
    public int ComponentFeet { get; set; } = -999;
    [XmlElement("comp_tex_feet")]
    public int TextureFeet { get; set; } = -999;

    [XmlElement("comp_teef")]
    public int ComponentTeeth { get; set; } = -999;
    [XmlElement("comp_tex_teef")]
    public int TextureTeeth { get; set; } = -999;

    [XmlElement("comp_accs")]
    public int ComponentAccessories { get; set; } = -999;
    [XmlElement("comp_tex_accs")]
    public int TextureAccessories { get; set; } = -999;

    [XmlElement("comp_task")]
    public int ComponentTask { get; set; } = -999;
    [XmlElement("comp_tex_task")]
    public int TextureTask { get; set; } = -999;

    [XmlElement("comp_decl")]
    public int ComponentDecal { get; set; } = -999;
    [XmlElement("comp_tex_decl")]
    public int TextureDecal { get; set; } = -999;

    [XmlElement("comp_jbib")]
    public int ComponentJbib { get; set; } = -999;
    [XmlElement("comp_tex_jbib")]
    public int TextureJbib { get; set; } = -999;
}
