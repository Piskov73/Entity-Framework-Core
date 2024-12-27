namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            string xmlRoot = "Countries";

            ImportCountryDto[] dtos = ImportDtoXml<ImportCountryDto[]>(xmlString, xmlRoot);

            List<Country> countries = new List<Country>();

            foreach (ImportCountryDto d in dtos)
            {
                if (!IsValid(d))
                {
                    sb.AppendLine(ErrorMessage); continue;
                }

                Country country = new Country()
                {
                    CountryName = d.CountryName,
                    ArmySize = d.ArmySize,
                };
                countries.Add(country);
                sb.AppendLine(string.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
            }

            context.Countries.AddRange(countries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            ImportManufacturerDto[] dtos = ImportDtoXml<ImportManufacturerDto[]>(xmlString, "Manufacturers");

            List<Manufacturer> manufacturers = new List<Manufacturer>();

            foreach (ImportManufacturerDto d in dtos)
            {
                if (!IsValid(d))
                {
                    sb.AppendLine(ErrorMessage); continue;
                }
                if (manufacturers.Any(m => m.ManufacturerName == d.ManufacturerName))
                {
                    sb.AppendLine(ErrorMessage); continue;
                }
                Manufacturer manufacturer = new Manufacturer()
                {
                    ManufacturerName = d.ManufacturerName,
                    Founded = d.Founded,
                };
                manufacturers.Add(manufacturer);

                string[] infoManufacturer = manufacturer.Founded.Split(", ", StringSplitOptions.RemoveEmptyEntries).ToArray();

                string townName = infoManufacturer[infoManufacturer.Length - 2];

                string countryName = infoManufacturer[infoManufacturer.Length - 1];

                string founded = $"{townName}, {countryName}";
                sb.AppendLine(string.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, founded));

            }

            context.Manufacturers.AddRange(manufacturers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            ImportShellDto[] dtos = ImportDtoXml<ImportShellDto[]>(xmlString, "Shells");

            List<Shell> shells = new List<Shell>();

            foreach (var d in dtos)
            {
                if (!IsValid(d))
                {
                    sb.AppendLine(ErrorMessage); continue;
                }

                Shell shell = new Shell()
                {
                    ShellWeight = d.ShellWeight,
                    Caliber = d.Caliber,
                };
                shells.Add(shell);

                sb.AppendLine(string.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
            }

            context.Shells.AddRange(shells);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportGunDto[]? dtos = ImportDtoJson<ImportGunDto[]>(jsonString);
            List<Gun> guns = new List<Gun>();

            foreach (var d in dtos)
            {
                if (!IsValid(d))
                {
                    sb.AppendLine(ErrorMessage); continue;
                }

                if (!Enum.TryParse<GunType>(d.GunType, true, out GunType gunType))
                {
                    sb.AppendLine(ErrorMessage); continue;
                }

                Gun gun = new Gun()
                {
                    ManufacturerId=d.ManufacturerId,
                    GunWeight=d.GunWeight,
                    BarrelLength=d.BarrelLength,
                    NumberBuild = d.NumberBuild,
                    Range = d.Range,
                    GunType=gunType,
                    ShellId = d.ShellId
                };

                foreach(var c in d.Countries)
                {
                    CountryGun countryGun = new CountryGun()
                    {
                        CountryId = c.Id,
                        Gun = gun
                    };
                    gun.CountriesGuns.Add(countryGun);
                }

                guns.Add(gun);
                sb.AppendLine(string.Format(SuccessfulImportGun,gun.GunType.ToString(),gun.GunWeight,gun.BarrelLength));


            }

            context.Guns.AddRange(guns);    
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }

        private static T ImportDtoXml<T>(string xmlString, string xmlRoot)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(xmlRoot);

            XmlSerializer serializer = new XmlSerializer(typeof(T), xmlRootAttribute);

            using StringReader reader = new StringReader(xmlString);

            T? result = (T)serializer.Deserialize(reader);
            return result;


        }

        private static T ImportDtoJson<T>(string jsonString)
        {

            T? result = JsonConvert.DeserializeObject<T>(jsonString);


            return result;
        }
    }
}