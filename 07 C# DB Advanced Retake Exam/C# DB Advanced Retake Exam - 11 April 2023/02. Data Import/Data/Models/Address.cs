using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace Invoices.Data.Models
{
    public class Address
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	StreetName – text with length[10…20] (required)
        [Required]
        [MaxLength(20)]
        public string StreetName { get; set; } = null!;

        //•	StreetNumber – integer(required)
        [Required]
        public int StreetNumber { get; set; }

        //•	PostCode – text(required)
        [Required]
        public string PostCode { get; set; } = null!;

        //•	City – text with length[5…15] (required)
        [Required]
        [MaxLength(15)]
        public string City { get; set; } = null!;

        //•	Country – text with length[5…15] (required)
        [Required]
        [MaxLength(15)]
        public string Country { get; set; } = null!;

        //•	ClientId – integer, foreign key(required)
        [Required]
        public int ClientId { get; set; }

        //•	Client – Client
        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; } = null!;

    }
}
