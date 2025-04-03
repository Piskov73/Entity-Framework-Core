using Cadastre.Data.Enumerations;
using Cadastre.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Cadastre.DataProcessor.ImportDtos
{
    public class ImportCitizenDto
    {
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
        [MaxLength(10)]
        public string BirthDate { get; set; } = null!;

        //•	MaritalStatus - MaritalStatus enum (Unmarried = 0, Married, Divorced, Widowed) (required)
        [Required]
        public string MaritalStatus { get; set; } = null!;

        //•	PropertiesCitizens - collection of type PropertyCitizen
        public int[] Properties { get; set; } = new int[] { };
    }
}
