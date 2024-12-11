using System.Xml.Serialization;

namespace TravelAgency.DataProcessor.ExportDtos
{
    [XmlType("Guide")]
    public class GuideExportDTO
    {
        [XmlElement("FullName")]
        public string FullName { get; set; } = null!;
        //<TourPackages>
        
        [XmlArray("TourPackages")]
        [XmlArrayItem("TourPackage")]
        public List<TourPackageExportDTO> TourPackages { get; set; } =new List<TourPackageExportDTO> ();

    }
}
