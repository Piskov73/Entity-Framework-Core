namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            var dispetchersDto = ImportDtoXml<ImportDespatcherDto[]>(xmlString, "Despatchers");

            List<Despatcher> despatchers = new List<Despatcher>();

            foreach (var d in dispetchersDto)
            {
                if (!IsValid(d))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Despatcher despatcher = new Despatcher()
                {
                    Name = d.Name,
                    Position = d.Position
                };

                foreach (var t in d.Trucks)
                {
                    if (!IsValid(t))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Truck truck = new Truck()
                    {
                        RegistrationNumber = t.RegistrationNumber,
                        VinNumber = t.VinNumber,
                        TankCapacity = t.TankCapacity,
                        CargoCapacity = t.CargoCapacity,
                        CategoryType = (CategoryType)t.CategoryType,
                        MakeType = (MakeType)t.MakeType,
                        Despatcher = despatcher
                    };

                    despatcher.Trucks.Add(truck);

                }

                despatchers.Add(despatcher);
                sb.AppendLine(string.Format(SuccessfullyImportedDespatcher, despatcher.Name, despatcher.Trucks.Count));

            }

            context.Despatchers.AddRange(despatchers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var clientsDto = ImportDtoJson<ImportClientDto[]>(jsonString);
            List<Client> clients = new List<Client>();

            int[] validTrucksId = context.Trucks.Select(t => t.Id).ToArray();

            foreach (var c in clientsDto)
            {
                if (!IsValid(c))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if(c.Type== "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client client = new Client()
                {
                    Name = c.Name,
                    Nationality = c.Nationality,
                    Type=c.Type
                };

                foreach (var t in c.Trucks.Distinct())
                {
                    if (!validTrucksId.Contains(t))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ClientTruck clientTruck = new ClientTruck()
                    {
                        Client=client,
                        TruckId=t
                    };
                    client.ClientsTrucks.Add(clientTruck);
                }

                clients.Add(client);
                sb.AppendLine(string.Format(SuccessfullyImportedClient,client.Name,client.ClientsTrucks.Count));

            }

            context.Clients.AddRange(clients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
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