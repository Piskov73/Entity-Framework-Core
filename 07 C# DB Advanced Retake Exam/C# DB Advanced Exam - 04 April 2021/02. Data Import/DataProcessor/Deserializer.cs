// ReSharper disable InconsistentNaming

namespace TeisterMask.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using Newtonsoft.Json;
    using System.Xml.Serialization;
    using System.Text;
    using TeisterMask.DataProcessor.ImportDto;
    using TeisterMask.Data.Models;
    using System.Globalization;
    using System.Xml.Linq;
    using TeisterMask.Data.Models.Enums;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            List<ImportProjectDto> projectDtos = ImportDtoXml<List<ImportProjectDto>>(xmlString, "Projects");

            List<Project> projects = new List<Project>();
            foreach (var pDto in projectDtos)
            {
                if (!IsValid(pDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (!DateTime.TryParseExact(pDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture
                    , DateTimeStyles.None, out var openDate))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime? dueDate = null;
                if (!string.IsNullOrEmpty(pDto.DueDate))
                {

                    if (!DateTime.TryParseExact(pDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture
                        , DateTimeStyles.None, out var dueDateOut))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    dueDate=dueDateOut;
                }

                Project project = new Project()
                {
                    Name = pDto.Name,
                    OpenDate = openDate,
                    DueDate = dueDate,
                };

                foreach (var tDto in pDto.Tasks)
                {
                    if (!IsValid(tDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!DateTime.TryParseExact(tDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture
                  , DateTimeStyles.None, out var open))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    

                    if (!DateTime.TryParseExact(tDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture
                        , DateTimeStyles.None, out var due))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (open >= due)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (open < project.OpenDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (project.DueDate != null && due >= project.DueDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }


                    Task task = new Task()
                    {
                        Name = tDto.Name,
                        OpenDate = openDate,
                        DueDate = due,
                        ExecutionType = (ExecutionType)tDto.ExecutionType,
                        LabelType = (LabelType)tDto.LabelType,
                    };
                    project.Tasks.Add(task);

                }

                projects.Add(project);

                sb.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
            }

            context.Projects.AddRange(projects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var employeesDto = ImportDtoJson<ImportEmployeesDto[]>(jsonString);
            List<Employee> employees = new List<Employee>();

            List<int> validTaskId=context.Tasks.Select(t=>t.Id).ToList();
            foreach (var eDto in employeesDto)
            {
                if (!IsValid(eDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Employee employee = new Employee()
                {
                    Username=eDto.Username,
                    Email=eDto.Email,
                    Phone=eDto.Phone,
                };
                foreach (var t in eDto.Tasks.Distinct())
                {
                    if (!validTaskId.Contains(t))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    EmployeeTask employeeTask = new EmployeeTask()
                    {
                        Employee = employee,
                        TaskId = t
                    };

                    employee.EmployeesTasks.Add(employeeTask);
                    
                }
                employees.Add(employee);

                sb.AppendLine(string.Format(SuccessfullyImportedEmployee,employee.Username,employee.EmployeesTasks.Count));

            }

            context.Employees.AddRange(employees);
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