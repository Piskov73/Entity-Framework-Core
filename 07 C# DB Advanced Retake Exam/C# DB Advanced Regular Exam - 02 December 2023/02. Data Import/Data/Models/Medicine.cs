using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace Medicines.Data.Models
{
    public class Medicine
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	Name – text with length[3, 150] (required)
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = null!;

        //•	Price – decimal in range[0.01…1000.00] (required)
        [Required]
        public decimal Price { get; set; }

        //•	Category – Category enum (Analgesic = 0, Antibiotic, Antiseptic, Sedative, Vaccine) (required)
        [Required]
        public Category Category { get; set; }

        //•	ProductionDate – DateTime(required)
        [Required]
        public DateTime ProductionDate { get; set; }

        //•	ExpiryDate – DateTime(required)
        [Required]
        public DateTime ExpiryDate { get; set; }

        //•	Producer – text with length[3, 100] (required)
        [Required]
        [MaxLength(100)]
        public string Producer { get; set; } = null!;

        //•	PharmacyId – integer, foreign key(required)
        [Required]
        public int PharmacyId { get; set; }

        //•	Pharmacy – Pharmacy
        [ForeignKey(nameof(PharmacyId))]
        public Pharmacy Pharmacy { get; set; }=null!;

        //•	PatientsMedicines - collection of type PatientMedicine
        public ICollection<PatientMedicine> PatientsMedicines { get; set; }=new HashSet<PatientMedicine>();

    }
}