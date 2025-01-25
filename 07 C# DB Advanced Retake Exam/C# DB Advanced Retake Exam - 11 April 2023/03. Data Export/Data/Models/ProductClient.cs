using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoices.Data.Models
{
    public class ProductClient
    {
        //•	ProductId – integer, Primary Key, foreign key(required)
        [Required]
        public int ProductId { get; set; }

        //•	Product – Product
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        //•	ClientId – integer, Primary Key, foreign key(required)
        [Required]
        public int ClientId { get; set; }

        //•	Client – Client
        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; } = null!;

    }
}