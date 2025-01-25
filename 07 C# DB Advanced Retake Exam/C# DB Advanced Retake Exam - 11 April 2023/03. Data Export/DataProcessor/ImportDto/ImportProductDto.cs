using Invoices.Data.Models.Enums;
using Invoices.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Invoices.DataProcessor.ImportDto
{
    internal class ImportProductDto
    {
        //•	Name – text with length[9…30] (required)
        [Required]
        [MinLength(9)]
        [MaxLength(30)]
        public string Name { get; set; } = null!;

        //•	Price – decimal in range[5.00…1000.00] (required)
        [Required]
        [Range(5.00,1000.00)]
        public decimal Price { get; set; }

        //•	CategoryType – enumeration of type CategoryType,
        //with possible values(ADR, Filters, Lights, Others, Tyres) (required)
        [Required]
        [Range(0,4)]
        public int CategoryType { get; set; }

     
        public List<int> Clients { get; set; } = new List<int>();
    }
}
