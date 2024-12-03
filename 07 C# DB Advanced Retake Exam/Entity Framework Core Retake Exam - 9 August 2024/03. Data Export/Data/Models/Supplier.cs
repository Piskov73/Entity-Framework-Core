namespace NetPay.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Shared.Constants;
    public class Supplier
    {

        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }
        //•	SupplierName – text with length[3, 60] (required)
        [Required]
        [StringLength(SupplierNameMaxLehgth)]
        public string SupplierName { get; set; } = null!;

        //•	SuppliersServices - collection of type SupplierService

        public ICollection<SupplierService> SuppliersServices { get; set; }=new HashSet<SupplierService>();

    }
}
