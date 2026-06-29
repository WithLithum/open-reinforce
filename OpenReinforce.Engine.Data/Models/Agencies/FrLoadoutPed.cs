using System.Xml;
using System.Xml.Serialization;

namespace OpenReinforce.Engine.Data.Models.Agencies
{
    public sealed class FrLoadoutPed
    {
        public const int MissingValue = -999;

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
        public int ComponentHead { get; set; } = MissingValue;
        [XmlElement("comp_tex_head")]
        public int TextureHead { get; set; } = MissingValue;

        [XmlElement("comp_berd")]
        public int ComponentBeard { get; set; } = MissingValue;
        [XmlElement("comp_tex_berd")]
        public int TextureBeard { get; set; } = MissingValue;

        [XmlElement("comp_hair")]
        public int ComponentHair { get; set; } = MissingValue;
        [XmlElement("comp_tex_hair")]
        public int TextureHair { get; set; } = MissingValue;

        [XmlElement("comp_uppr")]
        public int ComponentUpper { get; set; } = MissingValue;
        [XmlElement("comp_tex_uppr")]
        public int TextureUpper { get; set; } = MissingValue;

        [XmlElement("comp_lowr")]
        public int ComponentLower { get; set; } = MissingValue;
        [XmlElement("comp_tex_lowr")]
        public int TextureLower { get; set; } = MissingValue;

        [XmlElement("comp_hand")]
        public int ComponentHand { get; set; } = MissingValue;
        [XmlElement("comp_tex_hand")]
        public int TextureHand { get; set; } = MissingValue;

        [XmlElement("comp_feet")]
        public int ComponentFeet { get; set; } = MissingValue;
        [XmlElement("comp_tex_feet")]
        public int TextureFeet { get; set; } = MissingValue;

        [XmlElement("comp_teef")]
        public int ComponentTeeth { get; set; } = MissingValue;
        [XmlElement("comp_tex_teef")]
        public int TextureTeeth { get; set; } = MissingValue;

        [XmlElement("comp_accs")]
        public int ComponentAccessories { get; set; } = MissingValue;
        [XmlElement("comp_tex_accs")]
        public int TextureAccessories { get; set; } = MissingValue;

        [XmlElement("comp_task")]
        public int ComponentTask { get; set; } = MissingValue;
        [XmlElement("comp_tex_task")]
        public int TextureTask { get; set; } = MissingValue;

        [XmlElement("comp_decl")]
        public int ComponentDecal { get; set; } = MissingValue;
        [XmlElement("comp_tex_decl")]
        public int TextureDecal { get; set; } = MissingValue;

        [XmlElement("comp_jbib")]
        public int ComponentJbib { get; set; } = MissingValue;
        [XmlElement("comp_tex_jbib")]
        public int TextureJbib { get; set; } = MissingValue;

        [XmlElement("prop_head")]
        public int PropHead { get; set; } = MissingValue;

        [XmlElement("tex_prop_head")]
        public int TexturePropHead { get; set; } = MissingValue;

        [XmlElement("prop_eyes")]
        public int PropEyes { get; set; } = MissingValue;

        [XmlElement("tex_prop_eyes")]
        public int TexturePropEyes { get; set; } = MissingValue;

        [XmlElement("prop_ears")]
        public int PropEars { get; set; } = MissingValue;

        [XmlElement("tex_prop_ears")]
        public int TexturePropEars { get; set; } = MissingValue;

        [XmlElement("prop_lwrist")]
        public int PropLWrist { get; set; } = MissingValue;

        [XmlElement("tex_prop_lwrist")]
        public int TexturePropLWrist { get; set; } = MissingValue;

        [XmlElement("prop_rwrist")]
        public int PropRWrist { get; set; } = MissingValue;

        [XmlElement("tex_prop_rwrist")]
        public int TexturePropRWrist { get; set; } = MissingValue;
    }
}