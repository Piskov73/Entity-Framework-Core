namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Diagnostics.Metrics;
    using System.Globalization;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using AutoMapper.Execution;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            var clientsDto = ImportDtoXml<ImportClientDto[]>(xmlString, "Clients");

            List<Client> clients = new List<Client>();
            foreach (var c in clientsDto)
            {
                if (!IsValid(c))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client client = new Client()
                {
                    Name = c.Name,
                    NumberVat = c.NumberVat
                };

                foreach (var a in c.Addresses)
                {
                    if (!IsValid(a))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Address address = new Address()
                    {
                        StreetName = a.StreetName,
                        StreetNumber = a.StreetNumber,
                        PostCode = a.PostCode,
                        City = a.City,
                        Country = a.Country,
                        Client = client
                    };

                    client.Addresses.Add(address);
                }

                clients.Add(client);
                sb.AppendLine(string.Format(SuccessfullyImportedClients, client.Name));
            }
            context.Clients.AddRange(clients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var invoicesDto = ImportDtoJson<ImportInvoiceDto[]>(jsonString);

            var validClientsId = context.Clients.Select(c => c.Id).ToList();
            List<Invoice> invoices = new List<Invoice>();

            foreach (var i in invoicesDto)
            {
                if (!IsValid(i))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!DateTime.TryParseExact(i.IssueDate, "yyyy-MM-ddTHH:mm:ss",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var issueDate))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!DateTime.TryParseExact(i.DueDate, "yyyy-MM-ddTHH:mm:ss",
                 CultureInfo.InvariantCulture, DateTimeStyles.None, out var dueDate))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (issueDate >= dueDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!validClientsId.Contains(i.ClientId))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Invoice invoice = new Invoice()
                {
                    Number = i.Number,
                    IssueDate = issueDate,
                    DueDate = dueDate,
                    Amount = i.Amount,
                    CurrencyType = (CurrencyType)i.CurrencyType,
                    ClientId = i.ClientId
                };

                invoices.Add(invoice);
                sb.AppendLine(string.Format(SuccessfullyImportedInvoices, invoice.Number));

            }

            context.Invoices.AddRange(invoices);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {

            StringBuilder sb = new StringBuilder();

            var productsDto = ImportDtoJson<ImportProductDto[]>(jsonString);

            var validClientsId = context.Clients.Select(c => c.Id).ToList();

            List<Product> products = new List<Product>();

            foreach (var p in productsDto)
            {
                if (!IsValid(p))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Product product = new Product()
                {
                    Name = p.Name,
                    Price = p.Price,
                    CategoryType = (CategoryType)p.CategoryType,
                };

                foreach (var c in p.Clients.Distinct())
                {
                    if (!validClientsId.Contains(c))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ProductClient productClient = new ProductClient()
                    {
                        Product = product,
                        ClientId = c
                    };
                    product.ProductsClients.Add(productClient);
                }

                products.Add(product);

                sb.AppendLine(string.Format(SuccessfullyImportedProducts, product.Name, product.ProductsClients.Count));

            }

            context.Products.AddRange(products);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
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
