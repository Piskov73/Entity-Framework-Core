namespace Trucks.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            var despathers = context.Despatchers.Where(d => d.Trucks.Any()).ToArray()
                .Select(d => new
                {
                    DespatcherName = d.Name,
                    Trucks = d.Trucks.Select(t => new
                    {
                        RegistrationNumber = t.RegistrationNumber,
                        Make = t.MakeType.ToString()
                    })
                    .OrderBy(t => t.RegistrationNumber)
                    .ToArray()
                })
                .OrderByDescending(d => d.Trucks.Length)
                .ThenBy(d => d.DespatcherName)
                .ToArray();

            var despatcharsDto = despathers.Select(d => new ExportDespatcherDto
            {
                TrucksCount = d.Trucks.Length,
                DespatcherName = d.DespatcherName,
                Trucks = d.Trucks.Select(t => new ExportTruckDto
                {
                    RegistrationNumber=t.RegistrationNumber,
                    Make=t.Make
                })
                .ToList()
            }).ToArray();

            return XmlSerializeText(despatcharsDto, "Despatchers");
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var clients = context.Clients.Where(c => c.ClientsTrucks.Any(ct => ct.Truck.TankCapacity >= capacity)).ToArray()
                .Select(c => new
                {
                    Name = c.Name,
                    Trucks = c.ClientsTrucks.Where(ct => ct.Truck.TankCapacity >= capacity).ToArray()
                    .Select(ct => new
                    {
                        TruckRegistrationNumber = ct.Truck.RegistrationNumber,
                        VinNumber = ct.Truck.VinNumber,
                        TankCapacity = ct.Truck.TankCapacity,
                        CargoCapacity = ct.Truck.CargoCapacity,
                        CategoryType = ct.Truck.CategoryType.ToString(),
                        MakeType = ct.Truck.MakeType.ToString()
                    })
                    .OrderBy(ct => ct.MakeType)
                    .ThenByDescending(ct => ct.CargoCapacity)
                    .ToArray()
                })
                .OrderByDescending(c => c.Trucks.Length)
                .ThenBy(c => c.Name)
                .ToArray().Take(10);

            return JsonSerializeText(clients);
        }

        private static string JsonSerializeText(object text)
        {
            var result = JsonConvert.SerializeObject(text, Formatting.Indented);
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
