using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace Example.Models.Cities;

[XmlRoot(ElementName = "OBJECT")]
public class City
{
    [XmlAttribute(AttributeName = "ID")]
    public int Id { get; set; }

    [XmlAttribute(AttributeName = "OBJECTID")]
    public int ObjectId { get; set; }

    [XmlAttribute(AttributeName = "OBJECTGUID")]
    public string ObjectGuid { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "CHANGEID")]
    public int ChangeId { get; set; }

    [XmlAttribute(AttributeName = "NAME")]
    public string Name { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "TYPENAME")]
    public string TypeName { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "LEVEL")]
    public int Level { get; set; }

    [XmlAttribute(AttributeName = "OPERTYPEID")]
    public int OperTypeId { get; set; }

    [XmlAttribute(AttributeName = "PREVID")]
    public int PrevID { get; set; }

    [XmlAttribute(AttributeName = "NEXTID")]
    public int NextId { get; set; }

    [XmlIgnore]
    [NotMapped]
    public DateTime UpdateDate { get; set; }

    [XmlAttribute(AttributeName = "UPDATEDATE")]
    public DateTimeOffset UpdateDateOffset
    {
        get => new DateTimeOffset(UpdateDate, TimeSpan.Zero);
        set => UpdateDate = value.UtcDateTime;
    }

    [XmlIgnore]
    [NotMapped]
    public DateTime StartDate { get; set; }

    [XmlAttribute(AttributeName = "STARTDATE")]
    public DateTimeOffset StartDateOffset
    {
        get => new DateTimeOffset(StartDate, TimeSpan.Zero);
        set => StartDate = value.UtcDateTime;
    }

    [XmlIgnore]
    [NotMapped]
    public DateTime EndDate { get; set; }

    [XmlAttribute(AttributeName = "ENDDATE")]
    public DateTimeOffset EndDateOffset
    {
        get => new DateTimeOffset(EndDate, TimeSpan.Zero);
        set => EndDate = value.UtcDateTime;
    }

    [XmlAttribute(AttributeName = "ISACTUAL")]
    public int IsActual { get; set; }

    [XmlAttribute(AttributeName = "ISACTIVE")]
    public int IsActive { get; set; }
}
