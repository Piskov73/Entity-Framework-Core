﻿using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto
{
    [XmlType("Client")]
    public class ClientExportDTO
    {
        [XmlAttribute("InvoicesCount")]
        public int InvoicesCount { get; set; }

        [XmlElement("ClientName")]
        public string ClientName { get; set; } = null!;

        [XmlElement("VatNumber")]
        public string VatNumber { get; set; } = null!;

        [XmlArray("Invoices")]
        public InvoiceExportDTO[] Invoices { get; set; }=null!;

    }
}
