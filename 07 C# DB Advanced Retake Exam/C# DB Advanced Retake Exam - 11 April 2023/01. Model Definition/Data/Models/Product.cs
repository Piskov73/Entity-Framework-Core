namespace Invoices.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Data.Models.Enums;
    using static Constants.Constants;
    public class Product
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(MaxLengthProductName)]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        public CategoryType CategoryType { get; set; }

        public ICollection<ProductClient > ProductsClients { get; set; }=new HashSet<ProductClient>();

    }
}
