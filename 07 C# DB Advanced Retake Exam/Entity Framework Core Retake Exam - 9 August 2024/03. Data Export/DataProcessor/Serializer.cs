using NetPay.Data;
using NetPay.Data.Models;
using NetPay.Data.Models.Enums;
using NetPay.DataProcessor.ExportDtos;
using NetPay.DataProcessor.ImportDtos;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace NetPay.DataProcessor
{
    public class Serializer
    {
        public static string ExportHouseholdsWhichHaveExpensesToPay(NetPayContext context)
        {

            var households = context.Households
                .Where(h => h.Expenses.Any(e => e.PaymentStatus != PaymentStatus.Paid))
                .Select(h => new
                {
                    ContactPerson = h.ContactPerson,
                    Email = h.Email,
                    PhoneNumber = h.PhoneNumber,
                    Expenses = h.Expenses.Where(e => e.PaymentStatus != PaymentStatus.Paid)

                    .Select(e => new
                    {
                        ExpenseName = e.ExpenseName,
                        Amount = e.Amount,
                        PaymentDate = e.DueDate,
                        ServiceName = e.Service.ServiceName
                    })
                     .OrderBy(e => e.PaymentDate)
                     .ThenBy(e=>e.Amount)
                    .ToList()

                })
                .OrderBy(h => h.ContactPerson)
                .ToList();

            var householdsDTO = households.Select(h => new HouseholdExportDTO
            {
                ContactPerson = h.ContactPerson,
                Email = h.Email,
                PhoneNumber = h.PhoneNumber,
                Expenses = h.Expenses.Select(e => new ExpenseExportDTO
                {
                    ExpenseName = e.ExpenseName,
                    Amount = $"{e.Amount:f2}",
                    PaymentDate = e.PaymentDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    ServiceName = e.ServiceName
                })
                .OrderBy(e => e.PaymentDate)
                .ThenBy(e => e.Amount)
                .ToList(),
            })
                .OrderBy(h => h.ContactPerson)
                .ToList();

            XmlSerializer serializer
                = new XmlSerializer(typeof(List<HouseholdExportDTO>), new XmlRootAttribute("Households"));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new StringBuilder();

            using StringWriter writer = new StringWriter(sb);

            serializer.Serialize(writer, householdsDTO, namespaces);

            return sb.ToString().TrimEnd();

        }

        public static string ExportAllServicesWithSuppliers(NetPayContext context)
        {
            var services = context.Services
                .Select(s=> new
                {
                    s.ServiceName,
                    Suppliers=s.SuppliersServices.Select(ss => new
                    {
                        SupplierName= ss.Supplier.SupplierName
                    })
                    .OrderBy(ss=> ss.SupplierName)
                    .ToList()

                })
                .OrderBy(s=>s.ServiceName)
                .ToList();

            return JsonConvert.SerializeObject(services,Formatting.Indented);
        }
    }
}
