using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cadastre.Data.Models
{
    public class Property
    {
        //        •	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	PropertyIdentifier – text with length[16, 20] (required)
        [Required]
        [StringLength(20)]
        public string PropertyIdentifier { get; set; } = null!;

        //•	Area – int not negative(required)
        [Required]
        public int Area { get; set; }

        //•	Details - text with length[5, 500] (not required)
        [StringLength(500)]
        public string? Details { get; set; }

        //•	Address – text with length[5, 200] (required)
        [Required]
        [StringLength(200)]
        public string Address { get; set; } = null!;

        //•	DateOfAcquisition – DateTime(required)
        [Required]
        public DateTime DateOfAcquisition { get; set; }

        //•	DistrictId – integer, foreign key(required)
        [Required]
        public int DistrictId { get; set; }

        //•	District – District
        [ForeignKey(nameof(DistrictId))]
        public District District { get; set; } = null!;

        //•	PropertiesCitizens - collection of type PropertyCitizen
        public List<PropertyCitizen> PropertiesCitizens { get; set; } = new List<PropertyCitizen>();

    }
}
