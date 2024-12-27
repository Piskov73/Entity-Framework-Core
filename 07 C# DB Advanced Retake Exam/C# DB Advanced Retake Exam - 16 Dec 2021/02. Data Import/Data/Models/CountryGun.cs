using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artillery.Data.Models
{
    public class CountryGun
    {
        //•	CountryId – Primary Key integer, foreign key(required)
        [Required]
        public int CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; } = null!;

        //•	GunId – Primary Key integer, foreign key(required)
        [Required]
        public int GunId { get; set; }

        [ForeignKey(nameof(GunId))]
        public Gun Gun { get; set; }=null!;

    }
}