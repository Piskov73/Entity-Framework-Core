using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.DataProcessor.ImportDtos;

namespace TravelAgency.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedCustomer = "Successfully imported customer - {0}";
        private const string SuccessfullyImportedBooking = "Successfully imported booking. TourPackage: {0}, Date: {1}";

        public static string ImportCustomers(TravelAgencyContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(CustomerImportDTO[]), new XmlRootAttribute("Customers"));

            using StringReader reader = new StringReader(xmlString);

            CustomerImportDTO[]? customersDto = serializer.Deserialize(reader) as CustomerImportDTO[];

            List<Customer> customers = new List<Customer>();
            if (customersDto != null)
            {
                foreach (var cus in customersDto)
                {
                    if (!IsValid(cus))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool duplicatedCustomer = context.Customers
                        .Any(c => c.FullName == cus.FullName)
                        || context.Customers.Any(c => c.Email == cus.Email)
                        || context.Customers.Any(c => c.PhoneNumber == cus.PhoneNumber)
                        || customers.Any(c => c.FullName == cus.FullName)
                        || customers.Any(c => c.Email == cus.Email)
                        || customers.Any(c => c.PhoneNumber == cus.PhoneNumber);
                    if (duplicatedCustomer)
                    {
                        sb.AppendLine(DuplicationDataMessage);
                        continue;
                    }

                    Customer customer = new Customer()
                    {
                        FullName = cus.FullName,
                        Email = cus.Email,
                        PhoneNumber = cus.PhoneNumber
                    };
                    customers.Add(customer);
                    sb.AppendLine(string.Format(SuccessfullyImportedCustomer, customer.FullName));

                }
            }
            context.AddRange(customers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportBookings(TravelAgencyContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<Booking> bookings = new List<Booking>();
            var boourngDTOs = JsonConvert.DeserializeObject<BookingImportDTO[]>(jsonString) ;

            foreach (var b in boourngDTOs)
            {
                if (!IsValid(b))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!DateTime.TryParseExact(b.BookingDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None,out  DateTime dateTimr))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Customer? customer = context.Customers.FirstOrDefault(c => c.FullName == b.CustomerName);
                TourPackage? tourPackage = context.TourPackages.FirstOrDefault(t => t.PackageName == b.TourPackageName);

                if (customer==null!||tourPackage==null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;   
                }

                Booking booking = new Booking()
                {
                    BookingDate = dateTimr,
                    Customer=customer,
                    TourPackage=tourPackage
                };

                bookings.Add(booking);
                sb.AppendLine(string.Format(SuccessfullyImportedBooking
                    ,booking.TourPackage.PackageName,booking.BookingDate.ToString("yyyy-MM-dd")));
            }

            context.Bookings.AddRange(bookings);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validateContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validateContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                string currValidationMessage = validationResult.ErrorMessage;
            }

            return isValid;
        }
    }
}
