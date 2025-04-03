namespace Cadastre.DataProcessor
{
    using Cadastre.Data;
    using Cadastre.Data.Enumerations;
    using Cadastre.Data.Models;
    using Cadastre.DataProcessor.ImportDtos;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Net;
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

            var districtsDB = dbContext.Districts.ToHashSet();
            var propertiesDB = dbContext.Properties.ToHashSet();

            var districtsDto = ImportDtoXml<ImportDistrictDto[]>(xmlDocument, "Districts");

            if (districtsDto == null)
            {
                return "";
            }

            var districts = new List<District>();

            foreach (var dDto in districtsDto)
            {
                if (!IsValid(dDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (districtsDB.Any(d => d.Name == dDto.Name) || districts.Any(d => d.Name == dDto.Name))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!Enum.TryParse<Region>(dDto.Region, true, out var region))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                District district = new District
                {
                    Name = dDto.Name,
                    PostalCode = dDto.PostalCode,
                    Region = region
                };

                foreach (var pDto in dDto.Properties)
                {
                    if (!IsValid(pDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (district.Properties.Any(p => p.PropertyIdentifier == pDto.PropertyIdentifier
                    || propertiesDB.Any(p => p.PropertyIdentifier == pDto.PropertyIdentifier)))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (district.Properties.Any(p => p.Address == pDto.Address
                    || propertiesDB.Any(p => p.Address == pDto.Address)))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!DateTime.TryParseExact(pDto.DateOfAcquisition, "dd/MM/yyyy"
                        , CultureInfo.InvariantCulture,DateTimeStyles.None,out var date))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Property property = new Property
                    {
                        PropertyIdentifier=pDto.PropertyIdentifier,
                        Area=pDto.Area,
                        Details=pDto.Details,
                        Address=pDto.Address,
                        DateOfAcquisition=date,
                        District=district
                    };

                    district.Properties.Add(property);

                }

                districts.Add(district);
                //Successfully imported district - {districtName} with {propertiesCount} properties.
                sb.AppendLine(string.Format(SuccessfullyImportedDistrict,district.Name,district.Properties.Count));
            }

            dbContext.Districts.AddRange(districts);
            dbContext.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCitizens(CadastreContext dbContext, string jsonDocument)
        {
            StringBuilder sb = new StringBuilder();

            var citizensDto = ImportDtoJson<ImportCitizenDto[]>(jsonDocument);
            if (citizensDto == null)
            {
                return "";
            }

            var citizens =new  List<Citizen>();

            var validpropertisId = dbContext.Properties.Select(p => p.Id).ToHashSet();

            foreach (var cDto in citizensDto)
            {
                if (!IsValid(cDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (!DateTime.TryParseExact(cDto.BirthDate, "dd-MM-yyyy",CultureInfo.InvariantCulture
                    ,DateTimeStyles.None,out var birthDate))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!Enum.TryParse<MaritalStatus>(cDto.MaritalStatus,true,out var maritalStatus))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Citizen citizen = new Citizen
                {
                    FirstName=cDto.FirstName,
                    LastName=cDto.LastName,
                    BirthDate=birthDate,
                    MaritalStatus=maritalStatus
                };

                foreach (var id in cDto.Properties.Distinct())
                {
                    if (!validpropertisId.Contains(id))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    PropertyCitizen propertyCitizen = new PropertyCitizen
                    {
                        PropertyId=id,
                        Citizen=citizen
                    };

                    citizen.PropertiesCitizens.Add(propertyCitizen);
                }

                citizens.Add(citizen);

                //Successfully imported citizen - {citizenFirstName} {citizenLastName} with {propertiesCount} properties.
                sb.AppendLine(string.Format(SuccessfullyImportedCitizen,citizen.FirstName
                    ,citizen.LastName,citizen.PropertiesCitizens.Count));
            }
            dbContext.Citizens.AddRange(citizens);
            dbContext.SaveChanges();

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
