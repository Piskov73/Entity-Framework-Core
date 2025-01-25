using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace Invoices.Data.Models
{
    public class Client
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	Name – text with length[10…25] (required)
        [Required]
        [MaxLength(25)]
        public string Name { get; set; } = null!;

        //•	NumberVat – text with length[10…15] (required)
        [Required]
        [MaxLength(15)]
        public string NumberVat { get; set; } = null!;

        //•	Invoices – collection of type Invoicе
        public ICollection<Invoice> Invoices { get; set; } = new HashSet<Invoice>();

        //•	Addresses – collection of type Address
        public ICollection<Address> Addresses { get; set; } = new HashSet<Address>();

        //•	ProductsClients – collection of type ProductClient
        
        public ICollection<ProductClient> ProductsClients { get; set; } = new HashSet<ProductClient>();

    }
}