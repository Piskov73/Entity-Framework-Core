namespace SoftJail.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";

        private const string SuccessfullyImportedDepartment = "Imported {0} with {1} cells";

        private const string SuccessfullyImportedPrisoner = "Imported {0} {1} years old";

        private const string SuccessfullyImportedOfficer = "Imported {0} ({1} prisoners)";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var departmentsDto = ImportDtoJson<ImportDepartmentDto[]>(jsonString);

            List<Department> departments = new List<Department>();

            foreach (var d in departmentsDto)
            {
                if (!IsValid(d))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (d.Cells.Length == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Department department = new Department()
                {
                    Name=d.Name
                };

                bool flag = false;
                foreach (var c in d.Cells)
                {
                    if (!IsValid(c))
                    {
                        flag = true;
                        break;
                    }

                    Cell cell = new Cell()
                    {
                        CellNumber=c.CellNumber,
                        Department=department
                    };
                    department.Cells.Add(cell);
                }
                if (flag)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                departments.Add(department);

                //Imported {department name} with {cells count} cells

                sb.AppendLine(string.Format(SuccessfullyImportedDepartment,department.Name,department.Cells.Count));

            }

            context.Departments.AddRange(departments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var prisonerDto = ImportDtoJson<ImportPrisonerDto[]>(jsonString);
            List<Prisoner> prisoners = new List<Prisoner>();

            var validCellId = context.Cells.Select(x => x.Id).ToList();

            foreach (var p in prisonerDto)
            {
                if (!IsValid(p))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!DateTime.TryParseExact(p.IncarcerationDate, "dd/MM/yyyy"
                    ,CultureInfo.InvariantCulture,DateTimeStyles.None,out var incarcerationDate))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                DateTime? releaseDate = null;

                if (p.ReleaseDate != null)
                {
                    if (!DateTime.TryParseExact(p.ReleaseDate, "dd/MM/yyyy"
                   , CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    releaseDate = dateTime;
                }

                Prisoner prisoner = new Prisoner()
                {
                    FullName=p.FullName,
                    Nickname=p.Nickname,
                    Age=p.Age,
                    IncarcerationDate=incarcerationDate,
                    ReleaseDate=releaseDate,
                    Bail=p.Bail,
                    CellId=p.CellId
                };
                bool flag = false;

                foreach (var m in p.Mails)
                {
                    if (!IsValid(m))
                    {
                        flag = true;
                        break;
                    }

                    Mail mail = new Mail()
                    {
                        Description=m.Description,
                        Sender=m.Sender,
                        Address=m.Address,
                        Prisoner=prisoner
                    };

                    prisoner.Mails.Add(mail);
                }

                if (flag)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                prisoners.Add(prisoner);

                //Imported {prisoner name} {prisoner age} years old

                sb.AppendLine(string.Format(SuccessfullyImportedPrisoner,prisoner.FullName,prisoner.Age));
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            var officeresDto = ImportDtoXml<ImportOfficerDto[]>(xmlString, "Officers");

            List<Officer> officers = new List<Officer>();

            foreach (var o in officeresDto)
            {
                if (!IsValid(o))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if(!Enum.TryParse<Position>(o.Position,true,out var position))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if(!Enum.TryParse<Weapon>(o.Weapon,true,out var weapon))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Officer officer = new Officer()
                {
                    FullName=o.FullName,
                    Salary=o.Salary,
                    Position=position,
                    Weapon=weapon,
                    DepartmentId=o.DepartmentId
                };

                foreach (var pId in o.Prisoners)
                {
                    OfficerPrisoner officerPrisoner = new OfficerPrisoner()
                    {
                        Officer=officer,
                        PrisonerId=pId.Id
                    };
                    officer.OfficerPrisoners.Add(officerPrisoner);
                }

                officers.Add(officer);

                //Imported {officer name} ({prisoners count} prisoners)

                sb.AppendLine(string.Format
                    (SuccessfullyImportedOfficer,officer.FullName,officer.OfficerPrisoners.Count));
            }

            context.Officers.AddRange(officers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
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