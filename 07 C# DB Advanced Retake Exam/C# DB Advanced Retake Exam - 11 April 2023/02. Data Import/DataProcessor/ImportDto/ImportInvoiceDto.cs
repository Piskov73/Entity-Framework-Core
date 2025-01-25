using System.ComponentModel.DataAnnotations;

namespace Invoices.DataProcessor.ImportDto
{
    public class ImportInvoiceDto
    {
        //•	Number – integer in range[1, 000, 000, 000…1, 500, 000, 000] (required)
        [Required]
        [Range(1_000_000_000, 1_500_000_000)]
        public int Number { get; set; }

        //•	IssueDate – DateTime(required)
        [Required]
        public string IssueDate { get; set; } = null!;

        //•	DueDate – DateTime(required)
        [Required]
        public string DueDate { get; set; } = null!;

        //•	Amount – decimal (required)
        [Required]
        public decimal Amount { get; set; }

        //•	CurrencyType – enumeration of type CurrencyType, with possible values(BGN, EUR, USD) (required)
        [Required]
        [Range(0, 2)]
        public int CurrencyType { get; set; }

        //•	ClientId – integer, foreign key(required)
        [Required]
        public int ClientId { get; set; }
    }
}
