namespace TravelAgency.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using TravelAgency.Data.Models.Enums;
    using static Shared.ValidatingConstants;

    public class Guide
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	FullName – text with length[4, 60] (required)
        [Required]
        [StringLength(GuideNameMaxLehgth)]
        public string FullName { get; set; } = null!;

        //•	Language – Language enum (English = 0, German, French, Spanish, Russian) (required)
        [Required]
        public Language Language { get; set; }


        //•	TourPackagesGuides - collection of type TourPackageGuide
        public ICollection<TourPackageGuide> TourPackagesGuides { get; set; }=new HashSet<TourPackageGuide>();

    }
}
