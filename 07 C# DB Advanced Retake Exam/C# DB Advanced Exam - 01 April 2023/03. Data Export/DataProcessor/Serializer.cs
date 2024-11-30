namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.DataProcessor.ExportDto;
    using Boardgames.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {

            var creators = context.Creators.Where(c => c.Boardgames.Any())
                .ToArray()
               .Select(c => new CreatorExportDTO
               {
                   BoardgamesCount=c.Boardgames.Count,
                   CreatorName=$"{c.FirstName} {c.LastName}",
                   Boardgames= c.Boardgames.Select(b=> new BoardgameExportDTO
                   {
                       BoardgameName=b.Name,
                       BoardgameYearPublished=b.YearPublished
                   })
                   .OrderBy(b => b.BoardgameName)
                   .ToArray() 
               })
               .OrderByDescending(c=>c.Boardgames.Length)
               .ThenBy(c=>c.CreatorName)
               .ToArray();


            XmlSerializer xml= new XmlSerializer(typeof(CreatorExportDTO[]),new XmlRootAttribute("Creators"));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

            namespaces.Add(string.Empty,string.Empty);

            StringBuilder sb=new StringBuilder();

            using StringWriter sw=new StringWriter(sb);

            xml.Serialize(sw, creators, namespaces);

            return  sb.ToString().TrimEnd();
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers
                .Where(s => context.BoardgamesSellers
                .Any(bs => bs.Boardgame.YearPublished >= year
                 && bs.Boardgame.Rating <= rating))
                .OrderByDescending(s => s.BoardgamesSellers.Count())
                .ThenBy(s => s.Name)
                .ToArray()
                .Select(s => new
                {
                    s.Name,
                    s.Website,
                    Boardgames = s.BoardgamesSellers.Where(bs => bs.Boardgame.YearPublished >= year
                    && bs.Boardgame.Rating <= rating)
                    .ToArray()
                    .OrderByDescending(bg => bg.Boardgame.Rating)
                    .ThenBy(bg => bg.Boardgame.Name)
                    .Select(bg => new
                    {
                        Name = bg.Boardgame.Name,
                        Rating = bg.Boardgame.Rating,
                        Mechanics = bg.Boardgame.Mechanics,
                        Category = bg.Boardgame.CategoryType.ToString()
                    }).ToArray()
                })
                 .OrderByDescending(s => s.Boardgames.Length)
                 .ThenBy(s => s.Name)
                 .Take(5)
                 .ToArray();

            var formst = Formatting.Indented;

            return JsonConvert.SerializeObject(sellers, formst);
        }
    }
}