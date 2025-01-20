using System.ComponentModel.DataAnnotations;

namespace Medicines.DataProcessor.ImportDtos
{
    public class ImportPatientDto
    {
        //•	FullName – text with length[5, 100] (required)
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        //•	AgeGroup – AgeGroup enum (Child = 0, Adult, Senior) (required)
        [Required]
        [Range(0,2)]
        public int AgeGroup { get; set; }

        //•	Gender – Gender enum (Male = 0, Female) (required)
        [Required]
        [Range(0,1)]
        public int Gender { get; set; }
        public List<int> Medicines { get; set; } = new List<int>();
    }
}
