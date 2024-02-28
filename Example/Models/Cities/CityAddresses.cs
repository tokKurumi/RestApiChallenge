namespace Example.Models.Cities;

using System.Xml.Serialization;

[XmlRoot(ElementName = "ADDRESSOBJECTS")]
public class CityAddresses
{
    [XmlElement(ElementName = "OBJECT")]
    public List<City> Cities { get; set; } = [];
}