namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ExportDtos;
    using Newtonsoft.Json;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            DateTime dateTime = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var patients = context.Patients
                 .Where(p => p.PatientsMedicines.Any(pm => pm.Medicine.ProductionDate > dateTime))
                 .ToList()
                 .Select(p => new
                 {
                     Gender = p.Gender.ToString(),
                     Name = p.FullName,
                     AgeGroup = p.AgeGroup.ToString(),
                     Medicines = p.PatientsMedicines.Where(pm => pm.Medicine.ProductionDate > dateTime)
                     .ToList()
                     .Select(pm => new
                     {
                         Category = pm.Medicine.Category.ToString(),
                         Name = pm.Medicine.Name,
                         Price = pm.Medicine.Price,
                         Producer = pm.Medicine.Producer,
                         BestBefore = pm.Medicine.ExpiryDate
                     })
                     .OrderByDescending(pm => pm.BestBefore)
                     .ThenBy(pm => pm.Price)
                     .ToList()
                 })
                 .OrderByDescending(p => p.Medicines.Count)
                 .ThenBy(p => p.Name)
                 .ToList();

            var patientsDto = patients.Select(p => new ExportPatient_Dto
            {
                Gender = p.Gender.ToString().ToLower(),
                Name = p.Name,
                AgeGroup = p.AgeGroup,
                Medicines = p.Medicines.Select(m => new ExportMedicineDto
                {
                    Category = m.Category.ToString().ToLower(),
                    Name = m.Name,
                    Price = $"{m.Price:f2}",
                    Producer = m.Producer,
                    BestBefore = m.BestBefore.ToString("yyyy-MM-dd")
                }).ToList()

            }).ToList();

            return XmlSerializeText(patientsDto, "Patients");
        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {
            var medicines = context.Medicines
                .Where(m => m.Category == (Category)medicineCategory && m.Pharmacy.IsNonStop == true)
                .OrderBy(m => m.Price)
                .ThenBy(m => m.Name)
                .Select(m => new
                {
                    Name = m.Name,
                    Price = $"{m.Price:f2}",
                    Pharmacy = new
                    {
                        Name = m.Pharmacy.Name,
                        PhoneNumber = m.Pharmacy.PhoneNumber
                    }

                }).ToArray();

            return JsonSerializeText(medicines);
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
