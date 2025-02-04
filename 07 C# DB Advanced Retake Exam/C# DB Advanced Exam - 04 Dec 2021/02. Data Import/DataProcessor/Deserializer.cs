namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";



        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            var playsDto = ImportDtoXml<ImportPlayDto[]>(xmlString, "Plays");

            TimeSpan minDuration = TimeSpan.Parse("01:00:00");

            List<Play> plays = new List<Play>();

            foreach (var p in playsDto)
            {
                if (!IsValid(p))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if(!TimeSpan.TryParseExact
                    (p.Duration,"c",CultureInfo.InvariantCulture,TimeSpanStyles.None,out var duration))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (duration < minDuration)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!Enum.TryParse<Genre>(p.Genre,true,out var genre))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Play play = new Play()
                {
                    Title=p.Title,
                    Duration=duration,
                    Rating = p.Rating,
                    Genre=genre,
                    Description=p.Description,
                    Screenwriter=p.Screenwriter
                };

                plays.Add(play);
                //Successfully imported {playTitle} with genre {genreType} and a rating of {rating}!

                sb.AppendLine(string.Format(SuccessfulImportPlay, play.Title, play.Genre.ToString(), play.Rating));

            }

            context.Plays.AddRange(plays);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            var castsDto = ImportDtoXml<ImportCastDto[]>(xmlString, "Casts");

            List<Cast> casts = new List<Cast>();

            foreach (var c in castsDto)
            {
                if (!IsValid(c))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;

                }

                Cast cast = new Cast()
                {
                    FullName=c.FullName,
                    IsMainCharacter=bool.Parse(c.IsMainCharacter),
                    PhoneNumber=c.PhoneNumber,
                    PlayId=c.PlayId
                };

                casts.Add(cast);

                //Successfully imported actor {fullName} as a {main/lesser} character!

                string castType = cast.IsMainCharacter  ?  "main":"lesser";

                sb.AppendLine(string.Format(SuccessfulImportActor, cast.FullName, castType));
            }

            context.Casts.AddRange(casts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var theatresDto = ImportDtoJson<ImportTheatreDto[]>(jsonString);

            List<Theatre> theatres = new List<Theatre>();

            foreach (var t in theatresDto)
            {
                if (!IsValid(t))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Theatre theatre = new Theatre() 
                { 
                    Name=t.Name,
                    NumberOfHalls=t.NumberOfHalls,
                    Director=t.Director

                };

                foreach (var ti in t.Tickets)
                {
                    if (!IsValid(ti))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Ticket ticket = new Ticket()
                    {
                        Price=ti.Price,
                        RowNumber=ti.RowNumber,
                        PlayId=ti.PlayId,
                        Theatre=theatre
                    };

                    theatre.Tickets.Add(ticket);

                }

                theatres.Add(theatre);

                //Successfully imported theatre {theatreName} with #{totalNumber} tickets!

                sb.AppendLine(string.Format(SuccessfulImportTheatre,theatre.Name,theatre.Tickets.Count));
            }

            context.Theatres.AddRange(theatres);
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
