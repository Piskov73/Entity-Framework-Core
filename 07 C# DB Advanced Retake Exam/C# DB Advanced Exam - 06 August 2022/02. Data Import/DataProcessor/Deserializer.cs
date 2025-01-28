namespace Footballers.DataProcessor
{
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            var coachesesDto = ImportDtoXml<ImportCoachDto[]>(xmlString, "Coaches");

            List<Coach> coaches = new List<Coach>();

            foreach (var c in coachesesDto)
            {
                if (!IsValid(c))
                {
                    sb.AppendLine(ErrorMessage);

                    continue;
                }

                Coach coach = new Coach()
                {
                    Name = c.Name,
                    Nationality = c.Nationality
                };

                foreach (var f in c.Footballers)
                {
                    if (!IsValid(f))
                    {
                        sb.AppendLine(ErrorMessage);

                        continue;
                    }

                    if (!DateTime.TryParseExact(f.ContractStartDate, "dd/MM/yyyy"
                        , CultureInfo.InvariantCulture
                        , DateTimeStyles.None
                        , out var contractStartDate))
                    {
                        sb.AppendLine(ErrorMessage);

                        continue;
                    }

                    if (!DateTime.TryParseExact(f.ContractEndDate, "dd/MM/yyyy"
                      , CultureInfo.InvariantCulture
                      , DateTimeStyles.None
                      , out var contractEndDate))
                    {
                        sb.AppendLine(ErrorMessage);

                        continue;
                    }

                    if (contractStartDate >= contractEndDate)
                    {
                        sb.AppendLine(ErrorMessage);

                        continue;
                    }

                    Footballer footballer = new Footballer()
                    {

                        Name = f.Name,
                        ContractStartDate = contractStartDate,
                        ContractEndDate = contractEndDate,
                        PositionType = (PositionType)f.PositionType,
                        BestSkillType = (BestSkillType)f.BestSkillType,
                        Coach = coach
                    };

                    coach.Footballers.Add(footballer);
                }
                coaches.Add(coach);

                sb.AppendLine(string.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count));
            }

            context.Coaches.AddRange(coaches);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var teamsDto = ImportDtoJson<ImportTeamDto[]>(jsonString);

            var valifFootballersId = context.Footballers.Select(f => f.Id).ToList();

            var teams = new List<Team>();

            foreach (var t in teamsDto)
            {
                if (!IsValid(t))
                {
                    sb.AppendLine(ErrorMessage);

                    continue;
                }

                Team team = new Team()
                {
                    Name=t.Name,
                    Nationality=t.Nationality,
                    Trophies=t.Trophies
                };

                foreach (var f in t.Footballers.Distinct())
                {
                    if (!valifFootballersId.Contains(f))
                    {
                        sb.AppendLine(ErrorMessage);

                        continue;
                    }

                    TeamFootballer teamFootballer = new TeamFootballer()
                    {
                        Team=team,
                        FootballerId=f
                    };

                    team.TeamsFootballers.Add(teamFootballer);
                }

                teams.Add(team);

                sb.AppendLine(string.Format(SuccessfullyImportedTeam,team.Name,team.TeamsFootballers.Count));

            }

            context.Teams.AddRange(teams);
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
