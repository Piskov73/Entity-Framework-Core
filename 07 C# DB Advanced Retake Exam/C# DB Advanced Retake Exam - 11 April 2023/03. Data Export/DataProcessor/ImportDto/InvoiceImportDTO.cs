namespace Invoices.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    using Data.Models.Enums;
    using static Invoices.Constants.Constants;
    public class InvoiceImportDTO
    {
        [Required]
        [Range(NumberMinRange,NumberMaxRange)]
        public int Number { get; set; }
        [Required]
        public DateTime IssueDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [Range(0,2)]
        public CurrencyType CurrencyType { get; set; }

        [Required]
        public int ClientId { get; set; }

    }
}
