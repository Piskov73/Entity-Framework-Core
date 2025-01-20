namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ImportDtos;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var patientsDto = ImportDtoJson<ImportPatientDto[]>(jsonString);

            List<Patient> patients = new List<Patient>();
            List<int> validMedicineId = context.Medicines.Select(m => m.Id).ToList();
            foreach (var p in patientsDto)
            {
                if (!IsValid(p))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Patient patient = new Patient()
                {
                    FullName = p.FullName,
                    AgeGroup = (AgeGroup)p.AgeGroup,
                    Gender = (Gender)p.Gender
                };

                foreach (var m in p.Medicines)
                {
                    if (!IsValid(m))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (!validMedicineId.Contains(m))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if ( patient.PatientsMedicines.Any(x => x.MedicineId== m))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }



                    PatientMedicine pm = new PatientMedicine()
                    {
                        Patient = patient,
                        MedicineId = m
                    };

                    patient.PatientsMedicines.Add(pm);
                }

                patients.Add(patient);
                sb.AppendLine(string.Format(SuccessfullyImportedPatient, patient.FullName, patient.PatientsMedicines.Count));

            }
            context.Patients.AddRange(patients);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            var pharmaciersDto = ImportDtoXml<ImportPharmacyDto[]>(xmlString, "Pharmacies");

            List<Pharmacy> pharmacies = new List<Pharmacy>();

            foreach (var p in pharmaciersDto)
            {
                if (!IsValid(p))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (!bool.TryParse(p.IsNonStop, out var isNonStop))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Pharmacy pharmacy = new Pharmacy()
                {
                    Name = p.Name,
                    PhoneNumber = p.PhoneNumber,
                    IsNonStop = isNonStop
                };

                foreach (var m in p.Medicines)

                {
                    if (!IsValid(m))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!DateTime.TryParseExact(m.ProductionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out var productionDate))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!DateTime.TryParseExact(m.ExpiryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out var expiryDate))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (productionDate >= expiryDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (pharmacy.Medicines.Any(x => x.Name == m.Name && x.Producer == m.Producer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Medicine medicine = new Medicine()
                    {
                        Name = m.Name,
                        Price = m.Price,
                        Category = (Category)m.Category,
                        ProductionDate = productionDate,
                        ExpiryDate = expiryDate,
                        Producer = m.Producer,
                        Pharmacy = pharmacy
                    };

                    pharmacy.Medicines.Add(medicine);
                }
                pharmacies.Add(pharmacy);
                sb.AppendLine(string.Format(SuccessfullyImportedPharmacy, pharmacy.Name, pharmacy.Medicines.Count));
            }

            context.Pharmacies.AddRange(pharmacies);
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
