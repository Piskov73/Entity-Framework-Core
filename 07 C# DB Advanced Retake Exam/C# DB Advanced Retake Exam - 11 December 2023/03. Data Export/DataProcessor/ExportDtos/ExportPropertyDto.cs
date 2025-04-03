using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ExportDtos
{
    [XmlType("Property")]
    public class ExportPropertyDto
    {
        //   <Property postal-code="VA-90000">
        [XmlAttribute("postal-code")]
        public string PostalCode { get; set; } = null!;

        //  <PropertyIdentifier>VA-90000.003.005.005</PropertyIdentifier>
        [XmlElement("PropertyIdentifier")]
        public string PropertyIdentifier { get; set; } = null!;

        //  <Area>2300</Area>
        [XmlElement("Area")]
        public int Area { get; set; }

        //  <DateOfAcquisition>28/08/2008</DateOfAcquisition>
        [XmlElement("DateOfAcquisition")]
        public string DateOfAcquisition { get; set; } = null!;


    }
}
