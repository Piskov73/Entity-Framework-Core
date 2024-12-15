using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Medicines.Data.Models
{
    public class Patient
    {
        //        Patient
        //•	Id – integer, Primary Key

        [Key]
        public int Id { get; set; }

        //•	FullName – text with length[5, 100] (required)

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = null!;

        //•	AgeGroup – AgeGroup enum (Child = 0, Adult, Senior) (required)

        [Required]
        public AgeGroup AgeGroup { get; set; }

        //•	Gender – Gender enum (Male = 0, Female) (required)

        public Gender Gender { get; set; }

        //•	PatientsMedicines - collection of type PatientMedicine
        public ICollection<PatientMedicine> PatientsMedicines { get; set; }= new HashSet<PatientMedicine>();

    }
}
