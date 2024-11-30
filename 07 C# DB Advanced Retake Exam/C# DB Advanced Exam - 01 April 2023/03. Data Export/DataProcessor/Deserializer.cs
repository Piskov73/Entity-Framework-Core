namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(List<CreatorImportDTO>), new XmlRootAttribute("Creators"));

            using StringReader reader = new StringReader(xmlString);

            List<CreatorImportDTO>? creatorsDto = serializer.Deserialize(reader) as List<CreatorImportDTO>;

            List<Creator> creatorList = new List<Creator>();

            foreach (var c in creatorsDto)
            {
                if (!IsValid(c))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Creator creator = new Creator()
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                };

                foreach (var b in c.Boardgames)
                {
                    if (!IsValid(b))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Boardgame boardgame = new Boardgame()
                    {
                        Name = b.Name,
                        Rating = b.Rating,
                        YearPublished = b.YearPublished,
                        CategoryType = (CategoryType)b.CategoryType,
                        Mechanics = b.Mechanics,
                    };
                    creator.Boardgames.Add(boardgame);
                }

                creatorList.Add(creator);


                sb.AppendLine(string
                    .Format(SuccessfullyImportedCreator, creator.FirstName, creator.LastName, creator.Boardgames.Count));
            }
            context.Creators.AddRange(creatorList);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            List<SellersImportDTO>? sellersDTO = new();

            sellersDTO = JsonConvert.DeserializeObject<List<SellersImportDTO>>(jsonString);

            List<Seller> sellers = new List<Seller>();

            List<int> BoardgamesValidID = context.Boardgames.Select(b => b.Id).ToList();
            foreach (var sDto in sellersDTO)
            {
                if (!IsValid(sDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Seller seller = new Seller()
                {
                    Name = sDto.Name,
                    Address = sDto.Address,
                    Country = sDto.Country,
                    Website = sDto.Website,
                };

                foreach (var bId in sDto.Boardgames.Distinct())
                {
                    if (!BoardgamesValidID.Contains(bId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    BoardgameSeller boardgameSeller = new BoardgameSeller()
                    {
                        BoardgameId = bId,
                        Seller = seller
                    };

                    seller.BoardgamesSellers.Add(boardgameSeller);

                }

                sellers.Add(seller);



                sb.AppendLine(string.Format(SuccessfullyImportedSeller, seller.Name, seller.BoardgamesSellers.Count));
            }

            context.Sellers.AddRange(sellers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
