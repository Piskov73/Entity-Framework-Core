namespace Cadastre.DataProcessor
{
    using Cadastre.Data;
    using Cadastre.Data.Enumerations;
    using Cadastre.Data.Models;
    using Cadastre.DataProcessor.ImportDtos;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
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

            XmlSerializer serializer = new XmlSerializer(typeof(DistrictImportDto[]), new XmlRootAttribute("Districts"));
            StringReader xmlReader = new StringReader(xmlDocument);
            DistrictImportDto[]? distructsDtos = serializer.Deserialize(xmlReader) as DistrictImportDto[];

            List<District> districts = new List<District>();

            foreach (DistrictImportDto dto in distructsDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (dbContext.Districts.Any(d => d.Name == dto.Name) || districts.Any(d => d.Name == dto.Name))
                {
                    continue;
                }

                if (!Enum.TryParse<Region>(dto.Region, true, out Region region))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                District district = new District()
                {
                    Name = dto.Name,
                    PostalCode = dto.PostalCode,
                    Region = region
                };

                foreach (var propertyImport in dto.Properties)
                {
                    if (!IsValid(propertyImport))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!DateTime.TryParseExact(propertyImport.DateOfAcquisition
                        , "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None
                        , out DateTime date))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (dbContext.Properties.Any(p => p.PropertyIdentifier == propertyImport.PropertyIdentifier)
                        || districts.Any(d => d.Properties.Any(p => p.PropertyIdentifier == propertyImport.PropertyIdentifier)))
                    {
                        continue;
                    }

                    if (dbContext.Properties.Any(p => p.Address == propertyImport.Address)
                       || districts.Any(d => d.Properties.Any(p => p.Address == propertyImport.Address)))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Property property = new Property()
                    {
                        PropertyIdentifier = propertyImport.PropertyIdentifier,
                        Area = propertyImport.Area,
                        Details = propertyImport.Details,
                        Address=propertyImport.Address,
                        DateOfAcquisition=date,
                        District=district
                        
                    };
                    district.Properties.Add(property);

                }
                districts.Add(district);
                sb.AppendLine(string.Format(SuccessfullyImportedDistrict,district.Name,district.Properties.Count));

            }

            dbContext.Districts.AddRange(districts);
            dbContext.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCitizens(CadastreContext dbContext, string jsonDocument)
        {
            StringBuilder sb = new StringBuilder();

            CitizensImportDto[]? citizensDtos= JsonConvert.DeserializeObject<CitizensImportDto[]> (jsonDocument);

            List <Citizen> citizens=new List<Citizen> ();
            List<int> validPropertyId=dbContext.Properties.Select(p=>p.Id).ToList();

            foreach (var dto in citizensDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!DateTime.TryParseExact(dto.BirthDate, "dd-MM-yyyy",CultureInfo.InvariantCulture
                    ,DateTimeStyles.None,out DateTime date))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if(!Enum.TryParse<MaritalStatus>(dto.MaritalStatus,true,out MaritalStatus result))
                {
                    continue;
                }
                Citizen citizen= new Citizen()
                {
                    FirstName=dto.FirstName,
                    LastName=dto.LastName,
                    BirthDate=date,
                    MaritalStatus=result,
                };
                foreach (var pId in dto.Properties)
                {
                    if (!validPropertyId.Contains(pId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;   
                    }
                    PropertyCitizen propertyCitizen = new PropertyCitizen()
                    {
                        PropertyId=pId,
                        Citizen=citizen
                    };
                    citizen.PropertiesCitizens.Add(propertyCitizen);
                }

                citizens.Add(citizen);
                sb.AppendLine(string.Format(SuccessfullyImportedCitizen,citizen.FirstName,citizen.LastName,citizen.PropertiesCitizens.Count));

            }
            dbContext.AddRange(citizens);
            dbContext.SaveChanges();
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
