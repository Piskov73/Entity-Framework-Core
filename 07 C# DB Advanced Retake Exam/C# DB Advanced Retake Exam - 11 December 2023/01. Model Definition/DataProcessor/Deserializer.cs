namespace Cadastre.DataProcessor
{
    using Cadastre.Data;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid Data!";
        private const string SuccessfullyImportedDistrict =
            "Successfully imported district - {0} with {1} properties.";
        private const string SuccessfullyImportedCitizen =
            "Succefully imported citizen - {0} {1} with {2} properties.";

        public static string ImportDistricts(CadastreContext dbContext, string xmlDocument)
        {
            StringBuilder sb = new StringBuilder();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCitizens(CadastreContext dbContext, string jsonDocument)
        {
            StringBuilder sb = new StringBuilder();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
        private static T? ImportDtoXml<T>(string xmlString, string xmlRoot)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(xmlRoot);

            XmlSerializer serializer = new XmlSerializer(typeof(T), xmlRootAttribute);

            using StringReader reader = new StringReader(xmlString);

            T? result = (T?)serializer.Deserialize(reader);
            return result;


        }

        private static T? ImportDtoJson<T>(string jsonString)
        {

            T? result = JsonConvert.DeserializeObject<T>(jsonString);


            return result;
        }
    }
}
