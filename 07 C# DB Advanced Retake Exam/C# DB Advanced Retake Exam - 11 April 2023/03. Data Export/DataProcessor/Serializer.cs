namespace Invoices.DataProcessor
{
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.DataProcessor.ExportDto;
    using Microsoft.VisualBasic;
    using Newtonsoft.Json;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            var clients = context.Clients.Where(c => c.Invoices.Any(i => i.IssueDate >= date)).ToArray()
                .Select(c => new
                {
                    ClientName = c.Name,
                    VatNumber = c.NumberVat,
                    Invoices = c.Invoices
                    //.Where(i => i.IssueDate >= date)
                    .OrderBy(i => i.IssueDate)
                    .ThenByDescending(i => i.DueDate)
                    .ToArray()
                    .Select(i => new
                    {
                        InvoiceNumber = i.Number,
                        InvoiceAmount = i.Amount,
                        DueDate = i.DueDate.ToString("d",CultureInfo.InvariantCulture),
                        Currency = i.CurrencyType.ToString()
                    })
                    .ToList()
                })
                .OrderByDescending(c => c.Invoices.Count)
                .ThenBy(c => c.ClientName)
                .ToArray();


            var clientsDto = clients.Select(c => new ExportClientDto
            {
                InvoicesCount = c.Invoices.Count,
                ClientName = c.ClientName,
                VatNumber = c.VatNumber,
                Invoices = c.Invoices.Select(i => new ExportInvoiceDto
                {
                    InvoiceNumber = i.InvoiceNumber,
                    InvoiceAmount = i.InvoiceAmount,
                    DueDate = i.DueDate,
                    Currency = i.Currency
                }).ToList()
            }).ToArray();

            return XmlSerializeText(clientsDto, "Clients");
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {
            var products = context.Products.Where(p => p.ProductsClients.Any() &&
            p.ProductsClients.Any(pc => pc.Client.Name.Length >= nameLength)).ToArray()
            .Select(p => new
            {
                Name = p.Name,
                Price = p.Price,
                Category = p.CategoryType.ToString(),
                Clients = p.ProductsClients.Where(pc => pc.Client.Name.Length >= nameLength).ToArray()
                .Select(pc => new
                {
                    Name = pc.Client.Name,
                    NumberVat = pc.Client.NumberVat
                })
                .OrderBy(pc => pc.Name)
                .ToArray()
            })
            .OrderByDescending(p => p.Clients.Length)
            .ThenBy(p => p.Name)
            .ToArray().Take(5);

            return JsonSerializeText(products);
        }

        private static string JsonSerializeText(object text)
        {
            var result = JsonConvert.SerializeObject(text, Formatting.Indented);
            return result;
        }
        private static string XmlSerializeText<T>(ICollection<T> collection, string rootAttribute)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute(rootAttribute));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            using StringWriter sw = new StringWriter(sb);
            serializer.Serialize(sw, new List<T>(collection), namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}