using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Prisoner")]
    public class ImportPrisonerIdDto
    {
        // <Prisoner id = "15" />
        [XmlAttribute("id")]
       public int Id { get; set; }
    }
}