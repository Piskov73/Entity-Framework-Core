using Artillery.Data.Models.Enums;
using Artillery.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artillery.DataProcessor.ImportDto
{
    public class ImportGunDto
    {
        //•	ManufacturerId – integer, foreign key(required)
        [Required]
        public int ManufacturerId { get; set; }

       

        //•	GunWeight– integer in range[100…1_350_000] (required)
        [Required]
        [Range(100,1350000)]
        public int GunWeight { get; set; }

        //•	BarrelLength – double in range[2.00….35.00] (required)
        [Required]
        [Range(2.00,35.00)]
        public double BarrelLength { get; set; }

        //•	NumberBuild – integer
        public int? NumberBuild { get; set; }

        //•	Range – integer in range[1….100_000] (required)
        [Required]
        [Range(1,100000)]
        public int Range { get; set; }

        //•	GunType – enumeration of GunType,
        //with possible values(Howitzer, Mortar, FieldGun, AntiAircraftGun, MountainGun, AntiTankGun) (required)
        [Required]
        public string GunType { get; set; } = null!;

        //•	ShellId – integer, foreign key(required)
        [Required]
        public int ShellId { get; set; }
        

        //•	CountriesGuns – a collection of CountryGun
        public List<ImportCountriDto> Countries { get; set; }=new List<ImportCountriDto>();
    }
}
