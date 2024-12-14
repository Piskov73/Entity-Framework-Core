using Cadastre.Data.Enumerations;
using Cadastre.Data.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Cadastre.DataProcessor.ImportDtos
{
    [JsonObject(nameof(Citizen))]
    public class CitizensImportDto
    {
        //•	FirstName – text with length[2, 30] (required)
        [JsonProperty(nameof(FirstName))]
        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string FirstName { get; set; } = null!;

        //•	LastName – text with length[2, 30] (required)
        [JsonProperty(nameof(LastName))]
        [Required]
        [MaxLength(30)]
        [MinLength(2)]
        public string LastName { get; set; } = null!;

        //•	BirthDate – DateTime(required)
        [JsonProperty(nameof(BirthDate))]
        [Required]

        public string BirthDate { get; set; } = null!;

        //•	MaritalStatus - MaritalStatus enum (Unmarried = 0, Married, Divorced, Widowed) (required)
        [Required]
        [JsonProperty(nameof(MaritalStatus))]
        [EnumDataType(typeof(MaritalStatus))]
        public string MaritalStatus { get; set; } = null!;

        [Required]

        [JsonProperty(nameof(Properties))]
        public List<int> Properties { get; set; } = new List<int>();
    }
}
