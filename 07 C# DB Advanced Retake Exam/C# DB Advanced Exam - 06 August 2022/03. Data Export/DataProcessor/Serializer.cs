namespace Footballers.DataProcessor
{
    using Data;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            var coaches = context.Coaches.Where(c => c.Footballers.Any()).ToArray()
                .Select(c => new
                {
                    CoachName=c.Name,
                    Footballers=c.Footballers.ToArray()
                    .Select(f=>new
                    {
                        Name=f.Name,
                        Position=f.PositionType.ToString()
                    })
                    .OrderBy(f=>f.Name)
                    .ToArray()
                })
                .OrderByDescending(c=>c.Footballers.Length)
                .ThenBy(c=>c.CoachName)
                .ToArray();

            var coachesDto = coaches.Select(c => new ExportCoachDto
            {
                FootballersCount=c.Footballers.Length,
                CoachName=c.CoachName,
                Footballers=c.Footballers.Select(f=> new ExportFootballerDto
                {
                    Name=f.Name,
                    Position=f.Position
                }).ToList()
            }).ToArray();

            return XmlSerializeText(coachesDto, "Coaches");
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {

            var teams = context.Teams.Where(t => t.TeamsFootballers.Any(tf => tf.Footballer.ContractStartDate >= date))
                .ToArray()
                .Select(t => new
                {
                    Name = t.Name,
                    Footballers = t.TeamsFootballers.Where(tf => tf.Footballer.ContractStartDate >= date)
                    .OrderByDescending(tf => tf.Footballer.ContractEndDate)
                    .ThenBy(tf => tf.Footballer.Name)
                    .ToArray()
                    .Select(tf => new
                    {
                        FootballerName = tf.Footballer.Name,
                        ContractStartDate = tf.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                        ContractEndDate = tf.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                        BestSkillType = tf.Footballer.BestSkillType.ToString(),
                        PositionType = tf.Footballer.PositionType.ToString()
                    }).ToArray()

                })
                .OrderByDescending(t => t.Footballers.Length)
                .ThenBy(t => t.Name)
                .ToArray().Take(5);

            return JsonSerializeText(teams);
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
