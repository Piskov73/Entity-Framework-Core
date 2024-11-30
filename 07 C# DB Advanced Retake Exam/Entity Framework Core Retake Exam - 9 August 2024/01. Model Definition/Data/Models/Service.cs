namespace NetPay.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Shared.Constants;
    public class Service
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	ServiceName – text with length[5, 30] (required)
        [Required]
        [StringLength(ServiceNameMaxLength)]
        public string ServiceName { get; set; } = null!;

        //•	Expenses - a collection of type Expense
        public ICollection<Expense> Expenses { get; set; } = new HashSet<Expense>();

        //•	SuppliersServices - collection of type SupplierService
        public ICollection<SupplierService> SuppliersServices { get; set; } = new HashSet<SupplierService>();

    }
}
