using Cadastre.Data.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Cadastre.Data.Models
{
    public class District
    {
        //        •	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	Name – text with length[2, 80] (required)
        [Required]
        [StringLength(80)]
        public string Name { get; set; } = null!;

        //•	PostalCode – text with length 8.
        //All postal codes must have the following structure:
        //starting with two capital letters, followed by e dash '-', followed by five digits.Example: SF-10000 (required)
        [Required]
        public string PostalCode { get; set; } = null!;

        //•	Region – Region enum (SouthEast = 0, SouthWest, NorthEast, NorthWest) (required)

        [Required]
        public Region Region { get; set; }

        //•	Properties - collection of type Property
        public ICollection<Property> Properties { get; set; }=new List<Property>();

    }
}
