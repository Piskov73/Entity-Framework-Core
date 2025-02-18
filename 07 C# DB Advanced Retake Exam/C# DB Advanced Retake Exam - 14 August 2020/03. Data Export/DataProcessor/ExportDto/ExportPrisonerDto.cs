using System.Xml.Linq;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Prisoner")]
    public class ExportPrisonerDto
    {
        //<Prisoner>

        //<Id>3</Id>
        [XmlElement("Id")]
        public int Id { get; set; }

        //<Name>Binni Cornhill</Name>
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        //<IncarcerationDate>1967-04-29</IncarcerationDate>
        [XmlElement("IncarcerationDate")]
        public string IncarcerationDate { get; set; } = null!;

        //<EncryptedMessages>
        //  <Message>
        [XmlArray("EncryptedMessages")]
        [XmlArrayItem("Message")]
        public ExportMessageDto[] EncryptedMessages { get; set; } = new ExportMessageDto[] { };
    }
}
