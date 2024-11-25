namespace Invoices.DataProcessor
{
    using Invoices.Data;
    using Invoices.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            var clients = context.Clients
                .Where(c => c.Invoices.Any(i => i.IssueDate > date))
                .ToArray()
                .Select(c => new ClientExportDTO()
                {
                    InvoicesCount = c.Invoices.Count,
                    ClientName = c.Name,
                    VatNumber = c.NumberVat,
                    Invoices = c.Invoices
                    .OrderBy(i => i.IssueDate)
                    .ThenByDescending(i => i.DueDate)
                    .ToArray()
                    .Select(i => new InvoiceExportDTO()
                    {
                        InvoiceNumber = i.Number,
                        InvoiceAmount = i.Amount,
                        DueDate = i.DueDate.ToString("MM/dd/yyyy",CultureInfo.InvariantCulture),
                        Currency = i.CurrencyType.ToString()
                    })
                    .ToArray(),

                })
                .OrderByDescending(c => c.InvoicesCount)
                .ThenBy(c => c.ClientName)
                .ToArray();

            XmlRootAttribute attribute = new XmlRootAttribute("Clients");

            XmlSerializer serializer = new XmlSerializer(typeof(ClientExportDTO[]), attribute);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

            namespaces.Add("", "");

            StringBuilder sb= new StringBuilder();

            using StringWriter sw= new StringWriter(sb);

            serializer.Serialize(sw, clients,namespaces);

            return sb.ToString().Trim();
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {
            var products = context.Products.Where(p => p.ProductsClients.Any(pc => pc.Client.Name.Length >= nameLength))
                .ToArray()
                .Select(p => new
                {
                    p.Name,
                    p.Price,
                    Category = p.CategoryType.ToString(),
                    Clients = p.ProductsClients.Where(pc => pc.Client.Name.Length >= nameLength)
                    .ToArray()
                    .OrderBy(pc => pc.Client.Name)
                    .Select(pc => new
                    {
                        pc.Client.Name,
                        pc.Client.NumberVat
                    }).ToArray()
                }).OrderByDescending(p => p.Clients.Length).ThenBy(p => p.Name).Take(5).ToArray();




            return JsonConvert.SerializeObject(products, Formatting.Indented);
        }
    }
}