using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System.Text;
using System.Xml;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();
            //var empl1 = context.Employees.Find(1);
            //Console.WriteLine(empl1.FirstName);

            //3ex
            //Console.WriteLine(GetEmployeesFullInformation(context));

            //4ex
            //Console.WriteLine(GetEmployeesWithSalaryOver50000(context));

            //5ех
            //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));

            //6ex
            //Console.WriteLine(AddNewAddressToEmployee(context));

            //7ex
            //Console.WriteLine(GetEmployeesInPeriod(context));

            //8ex
            //Console.WriteLine(GetAddressesByTown(context));

            //9ex
            // Console.WriteLine(GetEmployee147(context));

            //10ex
            // Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));

            //11ex
            //Console.WriteLine(GetLatestProjects(context));

            //12ex
            //Console.WriteLine(IncreaseSalaries(context));

            //13ex
            //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(context));

            //14ex
            //Console.WriteLine(DeleteProjectById(context));

            //15ex
            Console.WriteLine(RemoveTown(context));
        }
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var empl = context.Employees;
            foreach (var e in empl.OrderBy(e => e.EmployeeId))
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();

        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees;

            foreach (var employee in employees.OrderBy(e => e.FirstName).Where(e => e.Salary > 50000))
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var empl = context.Employees.Where(e => e.Department.Name == "Research and Development");

            foreach (var e in empl.OrderBy(e => e.Salary).ThenByDescending(e => e.FirstName))
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from Research and Development - ${e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Address newAdress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var employee = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");

            employee.Address = newAdress;

            context.SaveChanges();

            var employees = context.Employees
                .Select(e => new { e.AddressId, e.Address.AddressText })
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .ToList();

            return string.Join(Environment.NewLine, employees.Select(e => $"{e.AddressText}"));
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var employees = context.Employees.Include(e => e.Manager).Include(e => e.EmployeesProjects).ThenInclude(e => e.Project);


            foreach (var employee in employees.Take(10))
            {
                stringBuilder.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.Manager.FirstName} {employee.Manager.LastName}");
                foreach (var proj in employee.EmployeesProjects.Where(e => e.Project.StartDate.Year >= 2001 && e.Project.StartDate.Year <= 2003))
                {
                    var startDate = proj.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt");
                    if (proj.Project.EndDate is null)
                    {
                        stringBuilder.AppendLine($"--{proj.Project.Name} - {startDate} - not finished");
                    }
                    else
                    {
                        var endDate = proj.Project.EndDate?.ToString("M/d/yyyy h:mm:ss tt");
                        stringBuilder.AppendLine($"--{proj.Project.Name} - {startDate} - {endDate}");
                    }
                }
            }

            return stringBuilder.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var addresses = context.Addresses.Include(a => a.Employees).Include(a => a.Town);
            foreach (var address in addresses.OrderByDescending(a => a.Employees.Count()).ThenBy(a => a.Town.Name).ThenBy(a => a.AddressText).Take(10))
            {
                sb.AppendLine($"{address.AddressText}, {address.Town.Name} - {address.Employees.Count} employees");
            }

            return sb.ToString().TrimEnd();

        }

        public static string GetEmployee147(SoftUniContext context)
        {

            StringBuilder sb = new StringBuilder();

            var employees = context.Employees.Include(e => e.EmployeesProjects).ThenInclude(e => e.Project);
            var employee147 = employees.FirstOrDefault(e => e.EmployeeId == 147);

            sb.AppendLine($"{employee147.FirstName} {employee147.LastName} - {employee147.JobTitle}");
            foreach (var emplProj in employee147.EmployeesProjects.OrderBy(p => p.Project.Name))
            {
                sb.AppendLine($"{emplProj.Project.Name}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departments = context.Departments.Include(d => d.Employees).ThenInclude(d => d.Manager);

            foreach (var department in departments.Where(d => d.Employees.Count() > 5).OrderBy(d => d.Employees.Count()).OrderBy(d => d.Name))
            {
                var manager = department.Employees.First().Manager;
                sb.AppendLine($"{department.Name} - {manager.FirstName} {manager.LastName}");
                foreach (var employee in department.Employees.OrderBy(e => e.FirstName))
                {
                    sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var last10projects = context.Projects.OrderByDescending(p => p.StartDate).Take(10);

            
            foreach (var project in last10projects.OrderBy(p => p.Name))
            {
                sb.AppendLine($"{project.Name}");
                sb.AppendLine($"{project.Description}");
                var date = project.StartDate.ToString("M/d/yyyy h:mm:ss tt");
                sb.AppendLine($"{date}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees=context.Employees.Include(e=>e.Department);

            foreach (var empl in employees.Where(e=>e.Department.Name== "Engineering"|| e.Department.Name == "Marketing" || e.Department.Name == "Tool Design" || e.Department.Name == "Information Services").OrderBy(e=>e.FirstName).ThenBy(e=>e.LastName))
            {
                empl.Salary *= 1.12m;
                sb.AppendLine($"{empl.FirstName} {empl.LastName} (${empl.Salary:f2})");
            }

            return sb.ToString().TrimEnd();

        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb=new StringBuilder();

            var employees = context.Employees;

            foreach (var empl in employees.Where(e=>e.FirstName.StartsWith("sa")).OrderBy(e=>e.FirstName).ThenBy(e=>e.LastName))
            {
                sb.AppendLine($"{empl.FirstName} {empl.LastName} - {empl.JobTitle} - (${empl.Salary:f2})");
            }

            return sb.ToString().TrimEnd();

        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var employeesProjectsToDelete = context.EmployeesProjects.Where(ep => ep.ProjectId == 2);
            context.EmployeesProjects.RemoveRange(employeesProjectsToDelete);

            var projectToDelete = context.Projects.Where(p => p.ProjectId == 2);
            context.Projects.RemoveRange(projectToDelete);

            context.SaveChanges();

            string[] projectsNames = context.Projects
                .Take(10)
                .Select(p => p.Name)
                .ToArray();

            return string.Join(Environment.NewLine, projectsNames);

        }

        public static string RemoveTown(SoftUniContext context)
        {
            Town townToDelete = context.Towns
             .Where(t => t.Name == "Seattle")
             .FirstOrDefault();

            Address[] addressesToDelete = context.Addresses
                .Where(a => a.TownId == townToDelete.TownId)
                .ToArray();

            Employee[] employeesToRemoveAddressFrom = context.Employees
                .Where(e => addressesToDelete
                .Contains(e.Address))
                .ToArray();

            foreach (Employee e in employeesToRemoveAddressFrom)
            {
                e.AddressId = null;
            }

            context.Addresses.RemoveRange(addressesToDelete);
            context.Towns.Remove(townToDelete);
            context.SaveChanges();

            return $"{addressesToDelete.Count()} addresses in Seattle were deleted";
        }
    }


}