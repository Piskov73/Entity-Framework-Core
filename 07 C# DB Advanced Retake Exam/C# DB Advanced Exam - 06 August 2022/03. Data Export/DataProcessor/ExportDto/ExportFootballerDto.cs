using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType("Footballer")]
    public class ExportFootballerDto
    {
        //  <Footballer>
        //  <Name>Bernardo Silva</Name>
        [XmlElement("Name")]
        public string Name { get; set; } = null!;
        //  <Position>Midfielder</Position>
        [XmlElement("Position")]
        public string Position { get; set; } = null!;
        //</Footballer>

    }
}