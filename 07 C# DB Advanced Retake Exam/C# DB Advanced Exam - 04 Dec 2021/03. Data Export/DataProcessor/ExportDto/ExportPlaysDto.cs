using System.Xml.Serialization;
using Theatre.Data.Models;

namespace Theatre.DataProcessor.ExportDto
{
    [XmlType("Play")]
    public class ExportPlaysDto
    {

        //<Play

        //Title = "A Raisin in the Sun"
        [XmlAttribute("Title")]
        public string Title { get; set; } = null!;

        //Duration="01:40:00"
        [XmlAttribute("Duration")]
        public string Duration { get; set; } = null!;

        //Rating="5.4"
        [XmlAttribute("Rating")]
        public string Rating { get; set; } = null!;

        //Genre="Drama">
        [XmlAttribute("Genre")]
        public string Genre { get; set; } = null!;

       
        [XmlArray("Actors")]
        [XmlArrayItem("Actor")]

        public ExportActorDto[] Actors { get; set; } = new ExportActorDto[] { };

    }
}
