using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            using SoftUniContext context = new SoftUniContext();

            Console.WriteLine(RemoveTown(context));
        }

        //03.Employees Full Information
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary
                }).OrderBy(e => e.EmployeeId).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        //4.Employees with Salary Over 50 000

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees.Select(e => new
            {
                e.FirstName,
                e.Salary
            }).Where(e => e.Salary > 50000).OrderBy(e => e.FirstName).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //05. Employees from Research and Development

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees.Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.Department,
                e.Salary

            }).Where(d => d.Department.Name == "Research and Development")
            .OrderBy(e => e.Salary)
            .ThenByDescending(e => e.FirstName)
            .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.Department.Name} - ${e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //06. Adding a New Address and Updating Employee

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {

            Address newAddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };



            var nakov = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");

            if (nakov != null)
            {
                nakov.Address = newAddress;
                context.SaveChanges();
            }

            var emploees = context.Employees.OrderByDescending(e => e.AddressId).Select(e => e.Address!.AddressText).Take(10).ToList();



            StringBuilder sb = new StringBuilder();

            foreach (var e in emploees)
            {
                sb.AppendLine(e);
            }

            return sb.ToString().TrimEnd();


        }

        // 07. Employees and Projects

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees.Select(e => new
            {
                employeesName = $"{e.FirstName} {e.LastName}",
                managerName = $"{e.Manager!.FirstName} {e.Manager.LastName}",
                projects = e.EmployeesProjects.Select(ep => new
                {
                    ep.Project.StartDate,
                    EndDate = ep.Project.EndDate.HasValue ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt") : "not finished",
                    projektsName = ep.Project.Name
                }).Where(ep => ep.StartDate.Year >= 2001 && ep.StartDate.Year <= 2003)

            }).Take(10);



            StringBuilder sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.employeesName} - Manager: {e.managerName}");
                foreach (var p in e.projects)
                {
                    sb.AppendLine($"--{p.projektsName} - {p.StartDate:M/d/yyyy h:mm:ss tt} - {p.EndDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //08. Addresses by Town

        public static string GetAddressesByTown(SoftUniContext context)
        {

            var addresses = context.Addresses.Select(a => new
            {
                a.AddressText,
                TownName = a.Town!.Name,
                EmployeesCount = a.Employees.Count
            }).OrderByDescending(a => a.EmployeesCount).ThenBy(a => a.TownName).ThenBy(a => a.AddressText).Take(10).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var a in addresses)
            {
                sb.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeesCount} employees");
            }

            return sb.ToString().TrimEnd();
        }

        //09. Employee 147

        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees.Find(147);



            StringBuilder sb = new StringBuilder();
            if (employee != null)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

                var projects = context.Projects.Where(ep => ep.EmployeesProjects.Any(e => e.EmployeeId == 147)).Select(ep => ep.Name).ToList();

                foreach (var item in projects.OrderBy(p => p))
                {
                    sb.AppendLine(item);
                }
            }

            return sb.ToString().TrimEnd();
        }

        //  10. Departments with More Than 5 Employees

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments.Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count).ThenBy(d => d.Name)
                .Select(d => new
                {
                    infoDepartment = $"{d.Name} - {d.Manager.FirstName} {d.Manager.LastName}",
                    employees = d.Employees.Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle
                    }).OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList(),
                }).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var d in departments)
            {
                sb.AppendLine(d.infoDepartment);
                foreach (var e in d.employees)
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //11. Find Latest 10 Projects

        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects.OrderByDescending(p => p.StartDate).Take(10).Select(p => new
            {
                p.Name,
                p.Description,
                p.StartDate
            }).OrderBy(p => p.Name).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var p in projects)
            {
                sb.AppendLine(p.Name);
                sb.AppendLine(p.Description);
                sb.AppendLine($"{p.StartDate:M/d/yyyy h:mm:ss tt}");
            }

            return sb.ToString().TrimEnd();
        }

        //12. Increase Salaries

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employees = context.Employees
               .Where(e => e.Department.Name == "Engineering"
                   || e.Department.Name == "Tool Design"
                   || e.Department.Name == "Marketing"
                   || e.Department.Name == "Information Services");


            foreach (var e in employees)
            {
                e.Salary *= 1.12M;
            }
            context.SaveChanges();


            var employeesIncreasedSalary = employees.OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => new
                {
                    Output = $"{e.FirstName} {e.LastName} (${e.Salary:f2})"
                }).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employeesIncreasedSalary)
            {
                sb.AppendLine(e.Output);
            }

            return sb.ToString().TrimEnd();

        }

        //13. Find Employees by First Name Starting With Sa

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees.Where(e => e.FirstName.ToLower().StartsWith("sa"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => new
                {
                    Output = $"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})"
                }).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine(e.Output);
            }

            return sb.ToString().TrimEnd();
        }

        //14. Delete Project by Id

        public static string DeleteProjectById(SoftUniContext context)
        {
            var project = context.Projects.Find(2);
            var employProject = context.EmployeesProjects.Where(ep => ep.ProjectId == 2);
            if (project != null)
            {
                foreach (var ep in employProject)
                {
                    context.EmployeesProjects.Remove(ep);
                }
                context.Projects.Remove(project);
                context.SaveChanges();
            }

            var projects=context.Projects.Take(10).Select(p=> new
            {
                p.Name
            }).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var p in projects)
            {
                sb.AppendLine(p.Name);
            }

            return sb.ToString().TrimEnd();
        }

        //15. Remove Town

        public static string RemoveTown(SoftUniContext context)
        {
            string townName = "Seattle";

            var town=context.Towns.FirstOrDefault(t=>t.Name == townName);
            int count = 0;
            if (town != null)
            {
                var employees = context.Employees.Where(e => e.Address!.Town!.Name == townName);
                foreach (var employee in employees)
                {
                    employee.AddressId = null;
                }

                var adresses = context.Addresses.Where(a => a.TownId == town.TownId);

                foreach (var a in adresses)
                {
                    context.Addresses.Remove(a);
                    count++;
                }

                context.Towns.Remove(town);
                context.SaveChanges();
            }
            return $"{count} addresses in Seattle were deleted";
        }
    }
}
