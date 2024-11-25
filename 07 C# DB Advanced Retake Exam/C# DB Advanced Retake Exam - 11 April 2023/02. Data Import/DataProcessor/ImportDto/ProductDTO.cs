namespace Invoices.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    using Data.Models.Enums;
    using static Constants.Constants;
    public class ProductDTO
    {
        [Required]
        [MinLength(MinLengthProductName)]
        [MaxLength(MaxLengthProductName)]
        public string Name { get; set; } = null!;

        [Required]
        [Range(ProductPriceMin,ProductPriceMax)]
        public decimal Price { get; set; }

        [Required]
        [Range(0,4)]
        public CategoryType CategoryType { get; set; }

       public ICollection <int> Clients = new List<int> ();
    }
}
