using Microsoft.VisualBasic;
using NetPay.Data;
using NetPay.Data.Models;
using NetPay.Data.Models.Enums;
using NetPay.DataProcessor.ImportDtos;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace NetPay.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedHousehold = "Successfully imported household. Contact person: {0}";
        private const string SuccessfullyImportedExpense = "Successfully imported expense. {0}, Amount: {1}";

        public static string ImportHouseholds(NetPayContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(HouseholdImportDTO[]), new XmlRootAttribute("Households"));

            using StringReader reader = new StringReader(xmlString);

            HouseholdImportDTO[]? householdsDTO = serializer.Deserialize(reader) as HouseholdImportDTO[];

            List<Household> households = new List<Household>();

            foreach (var hDTO in householdsDTO)
            {
                if (!IsValid(hDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var household = new Household()
                {
                    ContactPerson = hDTO.ContactPerson,
                    Email = hDTO.Email,
                    PhoneNumber = hDTO.PhoneNumber,
                };
                bool contextDuplicationCheck = context.Households.Any(h => h.ContactPerson == household.ContactPerson)
                    || context.Households.Any(h => h.Email == household.Email)
                    || context.Households.Any(h => h.PhoneNumber == household.PhoneNumber);

                bool householdsDuplicationCheck = households.Any(h => h.ContactPerson == household.ContactPerson)
                    || households.Any(h => h.Email == household.Email)
                    || households.Any(h => h.PhoneNumber == household.PhoneNumber);

                if (contextDuplicationCheck || householdsDuplicationCheck)
                {
                    sb.AppendLine(DuplicationDataMessage);
                    continue;
                }

                households.Add(household);
                sb.AppendLine(string.Format(SuccessfullyImportedHousehold, household.ContactPerson));
            }

            context.Households.AddRange(households);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportExpenses(NetPayContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var expensesDto = JsonConvert.DeserializeObject<ExpenseInportDTO[]>(jsonString);
            List<Expense> exceptions = new List<Expense>();

            foreach (var eDto in expensesDto)
            {
                if (!IsValid(eDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var validHouseholdId = context.Households.Select(h => h.Id).ToArray();
                var validServiceId = context.Services.Select(s => s.Id).ToArray();
                if (!validHouseholdId.Contains(eDto.HouseholdId) || !validServiceId.Contains(eDto.ServiceId))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                DateTime dueDate;
                if (!DateTime.TryParseExact(eDto.DueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dueDate))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Expense exception = new Expense()
                {
                    ExpenseName = eDto.ExpenseName,
                    Amount = eDto.Amount,
                    DueDate = dueDate,
                    PaymentStatus = Enum.Parse<PaymentStatus>(eDto.PaymentStatus),
                    HouseholdId = eDto.HouseholdId,
                    ServiceId = eDto.ServiceId,
                };
                exceptions.Add(exception);
                sb.AppendLine(string.Format(SuccessfullyImportedExpense, exception.ExpenseName, $"{exception.Amount:f2}"));
            }
            context.Expenses.AddRange(exceptions);
            context.SaveChanges();
            return sb.ToString().TrimEnd();

        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            foreach (var result in validationResults)
            {
                string currvValidationMessage = result.ErrorMessage;
            }

            return isValid;
        }
    }
}
