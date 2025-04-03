using Cadastre.Data;
using Cadastre.Data.Enumerations;
using Cadastre.DataProcessor.ExportDtos;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor
{
    public class Serializer
    {
        public static string ExportPropertiesWithOwners(CadastreContext dbContext)
        {
            DateTime date = DateTime.Parse("01/01/2000");

            var properties = dbContext.Properties.Where(p => p.DateOfAcquisition >= date)
                .OrderByDescending(p => p.DateOfAcquisition)
                .ThenBy(p => p.PropertyIdentifier)
                .Select(p => new
                {
                    //"PropertyIdentifier": "SF-10000.004.002.002",
                    p.PropertyIdentifier,
                    //"Area": 150,
                    p.Area,
                    //"Address": "Penthouse 2, 55 High Tower Road, Sofia",
                    p.Address,
                    //"DateOfAcquisition": "10/02/2023",
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    //"Owners": [
                    Owners = p.PropertiesCitizens.Select(c => new
                    {
                        //"LastName": "Petrov",
                        c.Citizen.LastName,
                        //"MaritalStatus": "Married"
                        MaritalStatus=c.Citizen.MaritalStatus.ToString()

                    })
                    .OrderBy(c=>c.LastName)
                    .ToArray()
                })
                .ToArray();

            return JsonSerializeText(properties);
        }

        public static string ExportFilteredPropertiesWithDistrict(CadastreContext dbContext)
        {
            var properties = dbContext.Properties.Where(p => p.Area >= 100)
                .OrderByDescending(p => p.Area)
                .ThenBy(p => p.DateOfAcquisition)
                .Select(p => new ExportPropertyDto
                {
                    PostalCode = p.District.PostalCode,
                    PropertyIdentifier = p.PropertyIdentifier,
                    Area = p.Area,
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                }).ToArray();

            return XmlSerializeText(properties, "Properties");
        }

        private static string JsonSerializeText(object obj)
        {
            var result = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return result;
        }
        private static string XmlSerializeText<T>(ICollection<T> collection, string rootAttribute)
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
