using ProductShop.Models;

namespace ProductShop.DTOs.Export
{
    public class ExportSoldProductsDTO
    {
      

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<SoldProductDTO> SoldProducts { get; set; }

    }
}
