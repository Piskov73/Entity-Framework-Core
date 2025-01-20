using System.Security.AccessControl;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
    [XmlType("Patient")]
    public class ExportPatient_Dto
    {


        //    <Patient Gender = "male" >
        [XmlAttribute("Gender")]
        public string Gender { get; set; } = null!;
        //< Name > Stanimir Pavlov</Name>
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        //<AgeGroup>Adult</AgeGroup>
        [XmlElement("AgeGroup")]
        public string AgeGroup { get; set; } = null!;

        //<Medicines>
        [XmlArray("Medicines")]
        [XmlArrayItem("Medicine")]
        public List<ExportMedicineDto> Medicines { get; set; } = new List<ExportMedicineDto>();

    }
}
