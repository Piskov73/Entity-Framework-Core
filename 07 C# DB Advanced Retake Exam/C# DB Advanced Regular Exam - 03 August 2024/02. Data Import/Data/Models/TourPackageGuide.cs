using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgency.Data.Models
{
    public class TourPackageGuide
    {
        //•	TourPackageId – integer, Primary Key, foreign key(required)
        [Required]
        public int TourPackageId { get; set; }
        //•	TourPackage – TourPackage
        [ForeignKey(nameof(TourPackageId))]
        public TourPackage TourPackage { get; set; } = null!;

        //•	GuideId – integer, Primary Key, foreign key(required)
        [Required]
        public int GuideId { get; set; }
        //•	Guide – Guide
        [ForeignKey(nameof(GuideId))]
        public Guide Guide { get; set; } = null!;


    }
}
