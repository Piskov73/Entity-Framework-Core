namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Text.Json.Nodes;
    using System.Xml.Serialization;
    using Invoices.Data;
    using Invoices.Data.Models;
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
            XmlSerializer serializer = new XmlSerializer(typeof(List<ClientImportDTO>), new XmlRootAttribute("Clients"));



            using StringReader reader = new StringReader(xmlString);
            List<ClientImportDTO>? clientsDTO = new();
            clientsDTO = serializer.Deserialize(reader) as List<ClientImportDTO>;

            List<Client> clients = new List<Client>();

            StringBuilder sb = new StringBuilder();

            foreach (var clientDTO in clientsDTO)
            {
                if (!IsValid(clientDTO))
                {

                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Client client = new Client()
                {
                    Name = clientDTO.Name,
                    NumberVat = clientDTO.NumberVat

                };
                clients.Add(client);
                sb.AppendLine(string.Format(SuccessfullyImportedClients, client.Name));

                foreach (var addressDTO in clientDTO.Addresses)
                {
                    if (!IsValid(addressDTO))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Address address = new Address()
                    {
                        StreetName = addressDTO.StreetName,
                        StreetNumber = addressDTO.StreetNumber,
                        PostCode = addressDTO.PostCode,
                        City = addressDTO.City,
                        Country = addressDTO.Country
                    };
                    client.Addresses.Add(address);
                }
            }
            context.Clients.AddRange(clients);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var invoicesDTO = JsonConvert.DeserializeObject<InvoiceImportDTO[]>(jsonString);
            List<Invoice> invoices = new List<Invoice>();

            var validClientsID = context.Clients.Select(c => c.Id).ToList();

            foreach (var invoiceDTO in invoicesDTO)
            {
                if (!IsValid(invoiceDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (invoiceDTO.IssueDate > invoiceDTO.DueDate
                    || invoiceDTO.IssueDate == DateTime.MinValue
                    || invoiceDTO.DueDate == DateTime.MaxValue)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!validClientsID.Contains(invoiceDTO.ClientId))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Invoice invoice = new Invoice()
                {
                    Number = invoiceDTO.Number,
                    IssueDate = invoiceDTO.IssueDate,
                    DueDate = invoiceDTO.DueDate,
                    Amount = invoiceDTO.Amount,
                    CurrencyType = invoiceDTO.CurrencyType,
                    ClientId = invoiceDTO.ClientId
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

            var productsDTO = JsonConvert.DeserializeObject<ProductDTO[]>(jsonString);

            List<Product> products = new List<Product>();


            var validClientsID = context.Clients.Select(c => c.Id).ToList();

            foreach (var pDTO in productsDTO)
            {
                if (!IsValid(pDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Product p = new Product()
                {
                    Name=pDTO.Name,
                    Price=pDTO.Price,
                    CategoryType=pDTO.CategoryType,
                };

                foreach (var cID in pDTO.Clients.Distinct())
                {
                    if (!validClientsID.Contains(cID))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ProductClient pClient = new ProductClient()
                    {
                        Product=p,
                        ClientId=cID
                    };
                    p.ProductsClients.Add(pClient);
                }
                products.Add(p);

                sb.AppendLine(string.Format(SuccessfullyImportedProducts, p.Name, p.ProductsClients.Count));

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
    }
}
