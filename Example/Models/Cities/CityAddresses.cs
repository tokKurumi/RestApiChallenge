using System.Xml.Serialization;

namespace Example.Models.Cities;

[XmlRoot(ElementName = "ADDRESSOBJECTS")]
public class CityAddresses
{
    [XmlElement(ElementName = "OBJECT")]
    public List<City> Cities { get; set; } = new List<City>();
}