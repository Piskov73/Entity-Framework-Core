using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    [XmlType("Users")]
    public class UsersWithProductsDTO
    {
        [XmlElement("count")]
        public int Count { get; set; }
        [XmlArray("users")]
        public List<UserDTO> Users { get; set; } = new List<UserDTO>();

    }
    [XmlType("User")]
    public class UserDTO
    {
       

        [XmlElement("firstName")]
        public string FirstName { get; set; } = null!;

        [XmlElement("lastName")]
        public string LastName { get; set; } = null!;

        [XmlElement("age")]
        public int? Age { get; set; }
        [XmlElement("SoldProducts")]
        public SoldProductDTO? SoldProduct { get; set; }
       
    }
    [XmlType("SoldProducts")]
    public class SoldProductDTO
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public List <ProductDto> Products { get; set; }=new List<ProductDto> ();
    }

    [XmlType("Product")]
    public class ProductDto
    {
        [XmlElement("name")]
        public string Name { get; set; }=null!;
        [XmlElement("price")]
        public decimal Price { get; set; }
    }

}
