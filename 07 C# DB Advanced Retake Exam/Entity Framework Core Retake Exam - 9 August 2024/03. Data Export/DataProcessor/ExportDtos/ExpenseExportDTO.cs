using System.Diagnostics.Metrics;
using System.Xml.Serialization;

namespace NetPay.DataProcessor.ExportDtos
{
    [XmlType("Expense")]
    public class ExpenseExportDTO
    {
        //<ExpenseName>Water Home</ExpenseName>
        [XmlElement("ExpenseName")]
        public string ExpenseName { get; set; } = null!;

        //<Amount>50.50</Amount>
        [XmlElement("Amount")]
        public string Amount { get; set; }=null!;

        //<PaymentDate>2024-08-20</PaymentDate>
        [XmlElement("PaymentDate")]
        public string PaymentDate { get; set; } = null!;

        //<ServiceName>Water</ServiceName>
        [XmlElement("ServiceName")]
        public string ServiceName { get; set; } = null!;

    }
}
