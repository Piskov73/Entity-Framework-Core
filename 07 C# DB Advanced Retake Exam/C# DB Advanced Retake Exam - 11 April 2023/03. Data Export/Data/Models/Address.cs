namespace Invoices.Data.Models
{
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static Constants.Constants;
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(MaxLengthStreetName)]
        public string StreetName { get; set; } = null!;

        [Required]
        public int StreetNumber { get; set; }

        [Required]
        public string PostCode { get; set; } = null!;

        [Required]
        [StringLength(MaxLenghtCityName)]
        public string City { get; set; }=null!;

        [Required]
        [StringLength(MaxLenghtCountryName)]
        public string Country { get; set; } = null!;

        [Required]
        public int ClientId { get; set; }
        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; } = null!;

    }
}
