using Artillery.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artillery.Data.Models
{
    public class Gun
    {
        public Gun()
        {
            this.CountriesGuns=new HashSet<CountryGun>();
        }
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	ManufacturerId – integer, foreign key(required)
        [Required]
        public int ManufacturerId { get; set; }

        [ForeignKey(nameof(ManufacturerId))]
        public Manufacturer Manufacturer { get; set; } = null!;

        //•	GunWeight– integer in range[100…1_350_000] (required)
        [Required]
        public int GunWeight { get; set; }

        //•	BarrelLength – double in range[2.00….35.00] (required)
        [Required]
        public double BarrelLength { get; set; }

        //•	NumberBuild – integer
        public int? NumberBuild {  get; set; }

        //•	Range – integer in range[1….100_000] (required)
        [Required]
        public int Range {  get; set; }

        //•	GunType – enumeration of GunType,
        //with possible values(Howitzer, Mortar, FieldGun, AntiAircraftGun, MountainGun, AntiTankGun) (required)
        [Required]
        public GunType GunType { get; set; }

        //•	ShellId – integer, foreign key(required)
        [Required]
        public int ShellId {  get; set; }
        [ForeignKey(nameof(ShellId))]
        public Shell Shell { get; set; }=null!;

        //•	CountriesGuns – a collection of CountryGun
        public ICollection<CountryGun> CountriesGuns { get; set; }

    }
}