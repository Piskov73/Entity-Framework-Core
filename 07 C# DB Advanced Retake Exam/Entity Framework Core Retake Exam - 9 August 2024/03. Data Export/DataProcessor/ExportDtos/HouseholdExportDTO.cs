namespace NetPay.DataProcessor.ExportDtos
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Household")]
    public class HouseholdExportDTO
    {
        [XmlElement("ContactPerson")]
        public string ContactPerson { get; set; } = null!;

        [XmlElement("Email")]
        public string? Email { get; set; }
        [XmlElement("PhoneNumber")]
        public string PhoneNumber { get; set; } = null!;

        [XmlArray("Expenses")]
        [XmlArrayItem("Expense")]
        public List<ExpenseExportDTO> Expenses { get; set; }= new List<ExpenseExportDTO>();
    }
}
