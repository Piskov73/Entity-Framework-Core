using static System.Net.Mime.MediaTypeNames;
using System;
using System.ComponentModel.DataAnnotations;
using Invoices.Data.Models.Enums;

namespace Invoices.Data.Models
{
    public class Product
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	Name – text with length[9…30] (required)
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = null!;

        //•	Price – decimal in range[5.00…1000.00] (required)
        [Required]
        public decimal Price { get; set; }

        //•	CategoryType – enumeration of type CategoryType,
        //with possible values(ADR, Filters, Lights, Others, Tyres) (required)
        public CategoryType CategoryType { get; set; }

        //•	ProductsClients – collection of type ProductClient
        public ICollection<ProductClient> ProductsClients { get; set; } = new HashSet<ProductClient>();

    }
}
