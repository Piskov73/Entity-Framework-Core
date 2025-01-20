namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            return "";
        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {
            return "";
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
