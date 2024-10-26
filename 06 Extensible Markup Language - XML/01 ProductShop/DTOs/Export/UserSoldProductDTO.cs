using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    [XmlType("User")]
    public class UserSoldProductDTO
    {
        public UserSoldProductDTO()
        {

        }
        [XmlElement("firstName")]
        public string FirstName { get; set; } = null!;
        [XmlElement("lastName")]
        public string LastName { get; set; } = null!;
        [XmlArray("soldProducts")]
        public List<ProductsInRangeDTO> SoldProducts { get; set; } = new List<ProductsInRangeDTO>();

    }
}
