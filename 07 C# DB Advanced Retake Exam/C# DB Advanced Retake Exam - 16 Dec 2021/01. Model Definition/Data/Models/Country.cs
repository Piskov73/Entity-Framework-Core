using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Artillery.Data.Models
{
    public class Country
    {
        public Country()
        {
            this.CountriesGuns = new HashSet<CountryGun>();
        }
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	CountryName – text with length[4, 60] (required)
        [Required]
        [StringLength(60)]
        public string CountryName { get; set; } = null!;

        //•	ArmySize – integer in the range[50_000….10_000_000] (required)
        [Required]
        public int ArmySize { get; set; }

        //•	CountriesGuns – a collection of CountryGun

        public ICollection<CountryGun> CountriesGuns { get; set; }

    }
}
