using Cadastre.Data;
using Cadastre.Data.Enumerations;
using Cadastre.DataProcessor.ExportDtos;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor
{
    public class Serializer
    {
        public static string ExportPropertiesWithOwners(CadastreContext dbContext)
        {
            DateTime date = DateTime.ParseExact("01/01/2000", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var properties = dbContext.Properties
                .Where(p => p.DateOfAcquisition >= date)
                .OrderByDescending(p => p.DateOfAcquisition)
                .ThenBy(p => p.PropertyIdentifier)
                .ToList()
                .Select(p => new
                {
                    PropertyIdentifier = p.PropertyIdentifier,
                    Area = p.Area,
                    Address = p.Address,
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Owners = p.PropertiesCitizens
                    .OrderBy(pc => pc.Citizen.LastName)
                    .ToArray()
                    .Select(ps => new
                    {
                        LastName = ps.Citizen.LastName,
                        MaritalStatus = ps.Citizen.MaritalStatus.ToString()
                    })

                    .ToArray()

                })

                .ToList();
            var resul = JsonConvert.SerializeObject(properties, Formatting.Indented);

            return resul;
        }

        public static string ExportFilteredPropertiesWithDistrict(CadastreContext dbContext)
        {
            StringBuilder sb = new StringBuilder();

            var properties = dbContext.Properties.Where(p => p.Area >= 100)
                .OrderByDescending(p => p.Area)
                .ThenBy(p => p.DateOfAcquisition)
                .Select(p => new PropertyExportDto()
                {
                    PostalCode = p.District.PostalCode,
                    PropertyIdentifier = p.PropertyIdentifier,
                    Area=p.Area,
                    DateOfAcquisition=p.DateOfAcquisition.ToString("dd/MM/yyyy" ,CultureInfo.InvariantCulture)
                })
                .ToArray();

            XmlSerializer serializer =new XmlSerializer(typeof(PropertyExportDto[]),new XmlRootAttribute("Properties"));

            XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();

            xmlns.Add(string.Empty,string.Empty);

            using StringWriter writer = new StringWriter(sb);

            serializer.Serialize(writer, properties,xmlns);

            return sb.ToString().TrimEnd();
        }
    }
}
