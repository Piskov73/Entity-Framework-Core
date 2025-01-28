using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType("Coach")]
    public class ExportCoachDto
    {
        //  <Coach FootballersCount = "5" >
        [XmlAttribute("FootballersCount")]
        public int FootballersCount { get; set; }

        //< CoachName > Pep Guardiola</CoachName>
        [XmlElement("CoachName")]
        public string CoachName { get; set; } = null!;

        //<Footballers>
        //  <Footballer>
        [XmlArray("Footballers")]
        [XmlArrayItem("Footballer")]
        public List<ExportFootballerDto> Footballers { get; set; } = new List<ExportFootballerDto>();



    }
}
