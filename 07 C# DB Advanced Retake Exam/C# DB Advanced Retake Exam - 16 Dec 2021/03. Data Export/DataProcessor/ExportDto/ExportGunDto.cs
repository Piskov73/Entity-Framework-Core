using Artillery.DataProcessor.ImportDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto
{
    [XmlType("Gun")]
    public class ExportGunDto
    {
        //<Gun Manufacturer="Krupp"
        [XmlAttribute("Manufacturer")]
        public string Manufacturer { get; set; } = null!;

        //GunType="Mortar"
        [XmlAttribute("GunType")]
        public string GunType { get; set; } = null!;

        //GunWeight="1291272"
        [XmlAttribute("GunWeight")]
        public int GunWeight { get; set; }

        //BarrelLength="8.31"
        [XmlAttribute("BarrelLength")]
        public double BarrelLength { get; set; }

        //Range="14258"> 
        [XmlAttribute("Range")]
        public int Range { get; set; }

        [XmlArray("Countries")]
        [XmlArrayItem("Country")]
        public List<ExportCountryDto> Countries { get; set; } = new List<ExportCountryDto>();
    }
}
