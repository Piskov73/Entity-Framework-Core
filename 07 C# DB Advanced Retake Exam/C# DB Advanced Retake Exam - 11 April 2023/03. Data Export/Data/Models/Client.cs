namespace Invoices.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Constants.Constants;
    public class Client
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(ClientNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(NumberVatMaxLength)]
        public string NumberVat { get; set; } = null!;

        public ICollection<Invoice> Invoices { get; set; }=new HashSet<Invoice>();  

        public ICollection<Address> Addresses { get; set; } = new HashSet<Address>();
        public ICollection<ProductClient> ProductsClients { get; set; } = new HashSet<ProductClient>();

    }
}
