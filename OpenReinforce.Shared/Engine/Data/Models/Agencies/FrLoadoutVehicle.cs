using System.Xml.Serialization;

namespace OpenReinforce.Engine.Data.Models.Agencies;

public sealed record FrLoadoutVehicle
{
    [XmlText]
    public string? Model { get; set; }

    [XmlAttribute]
    public int Chance { get; set; }

    [XmlAttribute]
    public int Livery { get; set; }

    [XmlAttribute]
    public string? Weapon { get; set; }

    [XmlAttribute("Is_Mp")]
    public bool IsMp { get; set; }

    [XmlAttribute("Use_Modkit_Liveries")]
    public bool UseModKitLiveries { get; set; }

    [XmlAttribute("Livery_Multi")]
    public string? LiveryMulti { get; set; }

    [XmlAttribute("extra_1")]
    public bool Extra1 { get; set; }

    [XmlAttribute("extra_2")]
    public bool Extra2 { get; set; }

    [XmlAttribute("extra_3")]
    public bool Extra3 { get; set; }

    [XmlAttribute("extra_4")]
    public bool Extra4 { get; set; }

    [XmlAttribute("extra_5")]
    public bool Extra5 { get; set; }

    [XmlAttribute("extra_6")]
    public bool Extra6 { get; set; }

    [XmlAttribute("extra_7")]
    public bool Extra7 { get; set; }

    [XmlAttribute("extra_8")]
    public bool Extra8 { get; set; }

    [XmlAttribute("extra_9")]
    public bool Extra9 { get; set; }

    [XmlAttribute("extra_10")]
    public bool Extra10 { get; set; }

    [XmlAttribute("extra_11")]
    public bool Extra11 { get; set; }

    [XmlAttribute("extra_12")]
    public bool Extra12 { get; set; }

    [XmlAttribute("extra_13")]
    public bool Extra13 { get; set; }

    [XmlAttribute("extra_14")]
    public bool Extra14 { get; set; }

    [XmlAttribute("extra_15")]
    public bool Extra15 { get; set; }
}
