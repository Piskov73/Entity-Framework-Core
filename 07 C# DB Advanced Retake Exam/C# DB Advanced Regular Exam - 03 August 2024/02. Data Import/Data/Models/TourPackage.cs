namespace TravelAgency.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Shared.ValidatingConstants;
    public class TourPackage
    {
        //•	Id
        [Key]
        public int Id { get; set; }
        //•	PackageName – text with length[2, 40] (required)
        [Required]
        [StringLength(PackageNameMaxLeghth)]
        public string PackageName { get; set; } = null!;

        //•	Description – text with max length 200 (not required)
        [StringLength(DescriptionLeghth)]
        public string? Description { get; set; }
        //•	Price – a positive decimal value(required)
        [Required]
        public decimal Price { get; set; }

        //•	Bookings - a collection of type Booking
        public ICollection<Booking> Bookings { get; set; }=new HashSet<Booking>();

        //•	TourPackagesGuides - collection of type TourPackageGuide
        public ICollection<TourPackageGuide> TourPackagesGuides { get; set; } = new HashSet<TourPackageGuide>();

    }
}
