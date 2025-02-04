using System.Xml.Serialization;

namespace Theatre.DataProcessor.ExportDto
{
    [XmlType("Actor")]
    public class ExportActorDto
    {
        //<Actor FullName = "Sylvia Felipe"
        [XmlAttribute("FullName")]
        public string FullName { get; set; } = null!;

        //MainCharacter="Plays main character in 'A Raisin in the Sun'."
        [XmlAttribute("MainCharacter")]
        public string MainCharacter { get; set; } = null!;
    }
}