using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto
{
    [XmlType("Client")]
    public class ExportClientDto
    {
        //<Client InvoicesCount = "9" >
        [XmlAttribute("InvoicesCount")]
        public int InvoicesCount { get; set; }

        //< ClientName > SPEDOX, SRO</ClientName>
        [XmlElement("ClientName")]
        public string ClientName { get; set; } = null!;

        //<VatNumber>SK2023911087</VatNumber>
        [XmlElement("VatNumber")]
        public string VatNumber { get; set; } = null!;

        //<Invoices>
        //  <Invoice>
        [XmlArray("Invoices")]
        [XmlArrayItem("Invoice")]
        public List<ExportInvoiceDto> Invoices { get; set; } = new List<ExportInvoiceDto>();

    }
}
