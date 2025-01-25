using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto
{
    [XmlType("Invoice")]
    public class ExportInvoiceDto
    {
        //<InvoiceNumber>1063259096</InvoiceNumber>
        [XmlElement("InvoiceNumber")]
        public int InvoiceNumber { get; set; }

        //<InvoiceAmount>167.22</InvoiceAmount>
        [XmlElement("InvoiceAmount")]
        public decimal InvoiceAmount { get; set; }

        //<DueDate>02/19/2023</DueDate>
        [XmlElement("DueDate")]
        public string DueDate { get; set; } = null!;

        //<Currency>EUR</Currency>
        [XmlElement("Currency")]
        public string Currency { get; set; } = null!;

    }
}