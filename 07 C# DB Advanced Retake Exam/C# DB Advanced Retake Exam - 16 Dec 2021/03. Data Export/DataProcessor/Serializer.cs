
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shells = context.Shells
                .Where(s => s.ShellWeight > shellWeight)
                 .ToArray()
                .Select(s => new
                {
                    s.ShellWeight,
                    s.Caliber,
                    Guns = s.Guns.Where(g => g.GunType.ToString() == "AntiAircraftGun")
                    .ToArray()
                    .OrderByDescending(g => g.GunWeight)
                    .Select(g => new
                    {
                        GunType = g.GunType.ToString(),
                        g.GunWeight,
                        g.BarrelLength,
                        Range = g.Range > 3000 ? "Long-range" : "Regular range"
                    })

                    .ToArray()

                })
                .OrderBy(s => s.ShellWeight)
                .ToArray();



            return JsonSerializeText(shells);
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {

            //<Gun Manufacturer="Krupp" GunType="Mortar" GunWeight="1291272" BarrelLength="8.31" Range="14258">
            var guns = context.Guns.Where(g => g.Manufacturer.ManufacturerName == manufacturer)
                .ToArray()
                .Select(g => new
                {
                    g.Manufacturer.ManufacturerName,
                    GunType = g.GunType.ToString(),
                    g.GunWeight,
                    g.BarrelLength,
                    g.Range,
                    Countries = g.CountriesGuns.Where(cg => cg.Country.ArmySize > 4500000)
                    .OrderBy(cg => cg.Country.ArmySize)
                    .ToArray()
                    .Select(cg => new
                    {
                        //      <Country Country="Sweden" ArmySize="5437337" />
                        Country = cg.Country.CountryName,
                        cg.Country.ArmySize
                    })
                    .ToArray()
                })
                .OrderBy(c => c.BarrelLength)
                .ToArray();


            var gunsDto = guns.Select(g => new ExportGunDto
            {
                Manufacturer=g.ManufacturerName,
                GunType=g.GunType,
                GunWeight=g.GunWeight,
                BarrelLength=g.BarrelLength,
                Range = g.Range,
                Countries=g.Countries.Select(c=>new ExportCountryDto
                {
                    Country=c.Country,
                    ArmySize=c.ArmySize

                }).ToList()
            }).ToArray();

            string rootAtribut = "Guns";


          return XmlSerializeText(gunsDto, rootAtribut);
        }

        private static string JsonSerializeText(object text)
        {
            var result = JsonConvert.SerializeObject(text, Formatting.Indented);
            return result;
        }

        private static string XmlSerializeText<T>(ICollection<T> collection , string rootAttribute)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute(rootAttribute));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            using StringWriter sw = new StringWriter(sb);
            serializer.Serialize(sw, new List<T>(collection), namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
