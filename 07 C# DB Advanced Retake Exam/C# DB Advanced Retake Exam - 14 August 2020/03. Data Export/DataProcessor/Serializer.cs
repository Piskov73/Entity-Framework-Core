namespace SoftJail.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ExportDto;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners.Where(p => ids.Contains(p.Id)).ToArray()
                .Select(p => new
                {
                    //"Id": 3,
                    Id = p.Id,
                    //"Name": "Binni Cornhill",
                    Name = p.FullName,
                    //"CellNumber": 503,
                    CellNumber = p.Cell.CellNumber,

                    //"Officers": [
                    Officers = p.PrisonerOfficers.Select(o => new
                    {
                        //"OfficerName": "Theo Carde",
                        OfficerName = o.Officer.FullName,

                        //"Department": "Blockchain"
                        Department = o.Officer.Department.Name

                    })
                    .OrderBy(o => o.OfficerName)
                    .ToArray(),

                    //p.PrisonerOfficers.Sum(s=>s.Officer.Salary)
                    TotalOfficerSalary = Math.Round(p.PrisonerOfficers.Sum(s => s.Officer.Salary), 2)

                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();

            return JsonSerializeText(prisoners);
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var validPrisoners = prisonersNames.Split(',', StringSplitOptions.RemoveEmptyEntries).ToArray();

            var prisners = context.Prisoners.Where(p => validPrisoners.Contains(p.FullName))
                .Select(p => new ExportPrisonerDto
                {
                    Id=p.Id,
                    Name=p.FullName,
                    IncarcerationDate=p.IncarcerationDate.ToString("yyyy-MM-dd"),
                    EncryptedMessages=p.Mails.Select(m=> new ExportMessageDto
                    {
                        Description=new string(m.Description.Reverse().ToArray())
                    }).ToArray()

                })
                .OrderBy(p=>p.Name)
                .ThenBy(p=>p.Id)
                .ToArray();

            return XmlSerializeText(prisners, "Prisoners");
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