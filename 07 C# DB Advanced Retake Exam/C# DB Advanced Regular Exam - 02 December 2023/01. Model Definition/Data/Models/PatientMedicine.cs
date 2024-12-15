using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medicines.Data.Models
{
    public class PatientMedicine
    {
        //        PatientMedicine
        //•	PatientId – integer, Primary Key, foreign key(required)

        [Required]
        public int PatientId { get; set; }

        //•	Patient – Patient
        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; } = null!;

        //•	MedicineId – integer, Primary Key, foreign key(required)

        [Required]
        public int MedicineId { get; set; }

        //•	Medicine – Medicine
        [ForeignKey(nameof(MedicineId))]
        public Medicine Medicine { get; set; }= null!;

    }
}
