namespace Theatre.DataProcessor
{
    using Microsoft.EntityFrameworkCore.Query.Internal;
    using Newtonsoft.Json;
    using System;
    using System.Diagnostics;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatres = context.Theatres.Where(t => t.NumberOfHalls >= numbersOfHalls && t.Tickets.Count >= 20).ToArray()
                .Select(t => new
                {
                    Name = t.Name,
                    Halls = t.NumberOfHalls,
                    Tickets = t.Tickets.Where(ti => ti.RowNumber >= 1 && ti.RowNumber <= 5).ToArray()
                    .Select(ti => new
                    {
                        Price = ti.Price,
                        RowNumber = ti.RowNumber
                    })
                    .ToArray()

                }).ToArray();

            var theatresDto = theatres.Select(t => new ExportTheatersDto
            {
                Name = t.Name,
                Halls = t.Halls,
                TotalIncome = t.Tickets.Sum(t => t.Price),
                Tickets = t.Tickets.Select(ti => new ExportTicketsDto
                {
                    Price = ti.Price,
                    RowNumber = ti.RowNumber
                }
                )
                .OrderByDescending(ti => ti.Price)
                .ToArray()
            }
            )
                .OrderByDescending(t => t.Halls)
                .ThenBy(t => t.Name)
                .ToArray();

            return JsonSerializeText(theatresDto);
        }

        public static string ExportPlays(TheatreContext context, double raiting)
        {
            var plays = context.Plays.Where(p => p.Rating <= raiting)
                  .OrderBy(p => p.Title)
                .ThenByDescending(p => p.Genre)
                .Select(p => new ExportPlaysDto
                {
                    Title = p.Title,
                    Duration = p.Duration.ToString("c"),
                    Rating = p.Rating == 0 ? "Premier" : p.Rating.ToString(),
                    Genre = p.Genre.ToString(),
                    Actors = p.Casts.Where(c => c.IsMainCharacter )
                    .Select(c => new ExportActorDto
                    {
                        FullName = c.FullName,
                        MainCharacter = $"Plays main character in '{p.Title}'."
                    })
                    .OrderByDescending(c => c.FullName)
                    .ToArray()

                })
                .ToArray();

            return XmlSerializeText(plays, "Plays");
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
