namespace VaporStore.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.ExportDto;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var genres = context.Genres.Where(g => genreNames.Contains(g.Name) && g.Games.Any(ga => ga.Purchases.Any()))
                .ToArray()
                .Select(g => new
                {
                    g.Id,
                    Genre = g.Name,
                    Games = g.Games.Where(x => x.Purchases.Any()).ToArray()
                    .Select(x => new
                    {

                        x.Id,
                        Title = x.Name,
                        Developer = x.Developer.Name,
                        Tags = string.Join(", ", x.GameTags.Select(gt => gt.Tag.Name).ToArray()),
                        Players = x.Purchases.Count
                    })
                    .OrderByDescending(x => x.Players)
                    .ThenBy(x => x.Id)
                    .ToArray(),
                    TotalPlayers = g.Games.Sum(x => x.Purchases.Count)
                })
                .OrderByDescending(g => g.TotalPlayers)
               .ThenBy(g => g.Id)
                .ToArray();


            return JsonSerializeText(genres);
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string purchaseType)
        {
            PurchaseType type = Enum.Parse<PurchaseType>(purchaseType);
            var users = context.Users.Where(u => u.Cards.Any(c => c.Purchases.Any(p => p.Type == type))).ToArray()
                .Select(u => new
                {
                    Username = u.Username,
                    Purchases = context.Purchases.Where(p => p.Card.User.Username == u.Username && p.Type == type).ToArray()
                    .Select(p => new
                    {
                        Card = p.Card.Number,
                        Cvc = p.Card.Cvc,
                        Date = p.Date,
                        Game = new
                        {
                            GameName = p.Game.Name,
                            Genre = p.Game.Genre.Name,
                            Price = p.Game.Price
                        }
                    })
                    .ToArray()

                })
                .ToArray();

            var usersDto = users.Where(u=>u.Purchases.Length>0)
                .Select(u => new ExportUserDto
                {
                    Username=u.Username,
                    Purchases=u.Purchases.OrderBy(p=>p.Date)
                    .Select(p=>new ExportPurchaseDto
                    {
                        Card=p.Card,
                        Cvc=p.Cvc,
                        Date=p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                        Game=new ExportGameDto
                        {
                            GameName=p.Game.GameName,
                            Genre=p.Game.Genre,
                            Price=p.Game.Price
                        }
                    })
                    .ToArray(),
                    TotalSpent=u.Purchases.Sum(s=>s.Game.Price)
                })
                .OrderByDescending(u=>u.TotalSpent)
                .ThenBy(u=>u.Username)
                .ToArray();
           
          


            return XmlSerializeText(usersDto, "Users");
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