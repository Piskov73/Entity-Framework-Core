namespace VaporStore.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using AutoMapper.Execution;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.ImportDto;

    public static class Deserializer
    {
        public const string ErrorMessage = "Invalid Data";

        public const string SuccessfullyImportedGame = "Added {0} ({1}) with {2} tags";

        public const string SuccessfullyImportedUser = "Imported {0} with {1} cards";

        public const string SuccessfullyImportedPurchase = "Imported {0} for {1}";

        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var gamesDto = ImportDtoJson<ImportGameDto[]>(jsonString);

            List<Game> games = new List<Game>();
            List<Developer> developers = new List<Developer>();
            List<Tag> tags = new List<Tag>();
            List<Genre> genres = new List<Genre>();

            foreach (var gDto in gamesDto)
            {
                if (!IsValid(gDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (gDto.Tags.Length == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!DateTime.TryParseExact
                    (gDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var releaseDate))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Developer? developer = developers.FirstOrDefault(x => x.Name == gDto.Developer);
                if (developer == null)
                {
                    developer = new Developer()
                    {
                        Name = gDto.Developer

                    };

                    developers.Add(developer);
                }

                Genre? genre = genres.FirstOrDefault(x => x.Name == gDto.Genre);

                if (genre == null)
                {
                    genre = new Genre()
                    {
                        Name = gDto.Genre
                    };
                    genres.Add(genre);
                }


                Game game = new Game()
                {
                    Name = gDto.Name,
                    Price = gDto.Price,
                    ReleaseDate = releaseDate,
                    Developer = developer,
                    Genre = genre
                };

                foreach (var t in gDto.Tags)
                {

                    Tag? tag = tags.FirstOrDefault(x => x.Name == t);

                    if (tag == null!)
                    {
                        tag = new Tag()
                        {
                            Name = t
                        };
                    }

                    if (!IsValid(tag))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    tags.Add(tag);

                    GameTag gameTag = new GameTag()
                    {
                        Game = game,
                        Tag = tag
                    };
                    game.GameTags.Add(gameTag);

                }

                if (game.GameTags.Count == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                games.Add(game);

                //Upon successful import you should print "Added {gameName} ({gameGenre}) with {tagsCount} tags"!

                sb.AppendLine(string.Format(SuccessfullyImportedGame, game.Name, game.Genre.Name, game.GameTags.Count));
            }
            context.Tags.AddRange(tags);
            context.Games.AddRange(games);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var usersDto = ImportDtoJson<ImportUserDto[]>(jsonString);

            List<User> users = new List<User>();

            foreach (var u in usersDto)
            {
                if (!IsValid(u))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (u.Cards.Length == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                User user = new User()
                {
                    Username = u.Username,
                    FullName = u.FullName,
                    Email = u.Email,
                    Age = u.Age
                };



                foreach (var c in u.Cards)
                {
                    if (!IsValid(c))
                    {

                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!Enum.TryParse<CardType>(c.Type, true, out var cardTyp))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Card card = new Card()
                    {
                        Number = c.Number,
                        Cvc = c.Cvc,
                        Type = cardTyp,
                        User = user
                    };

                    user.Cards.Add(card);
                }

                if (user.Cards.Count == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                users.Add(user);

                //print "Imported {username} with {cardsCount} cards"!

                sb.AppendLine(string.Format(SuccessfullyImportedUser, user.Username, user.Cards.Count));

            }
            context.Users.AddRange(users);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            var purchasesDto = ImportDtoXml<ImportPurchaseDto[]>(xmlString, "Purchases");

            List<Purchase> purchases = new List<Purchase>();
            List<Card> cards = context.Cards.ToList();
            List<Game> games = context.Games.ToList();

            foreach (var pDto in purchasesDto)
            {
                if (!IsValid(pDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!Enum.TryParse<PurchaseType>(pDto.Type,true,out var purchaseType))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (!DateTime.TryParseExact
                    (pDto.Date, "dd/MM/yyyy HH:mm",CultureInfo.InvariantCulture,DateTimeStyles.None,out var date))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Card? card = cards.FirstOrDefault(x => x.Number == pDto.Card);
                if (card == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Game? game = games.FirstOrDefault(x => x.Name == pDto.Game);
                if (game == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Purchase p = new Purchase() 
                {
                    Type=purchaseType,
                    ProductKey=pDto.ProductKey,
                    Date=date,
                    Card=card,
                    Game=game
                };

                purchases.Add(p);

                //Upon successful import you should print "Imported {gameName} for {username}"!

                sb.AppendLine(string.Format(SuccessfullyImportedPurchase,p.Game.Name,p.Card.User.Username));

            }

            context.Purchases.AddRange(purchases);
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