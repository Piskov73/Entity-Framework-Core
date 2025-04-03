using Cadastre.Data.Enumerations;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Cadastre.Data.Models
{
    public class Citizen
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	FirstName – text with length[2, 30] (required)
        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string FirstName { get; set; } = null!;

        //•	LastName – text with length[2, 30] (required)
        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string LastName { get; set; } = null!;

        //•	BirthDate – DateTime(required)
        [Required]
        public DateTime BirthDate { get; set; }

        //•	MaritalStatus - MaritalStatus enum (Unmarried = 0, Married, Divorced, Widowed) (required)
        [Required]
        public MaritalStatus MaritalStatus { get; set; }

        //•	PropertiesCitizens - collection of type PropertyCitizen
        public ICollection<PropertyCitizen> PropertiesCitizens { get; set; } = new HashSet<PropertyCitizen>();

    }
}
