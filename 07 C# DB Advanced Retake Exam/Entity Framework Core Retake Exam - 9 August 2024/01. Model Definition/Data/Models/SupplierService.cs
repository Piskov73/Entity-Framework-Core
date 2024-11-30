using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetPay.Data.Models
{
    public class SupplierService
    {
        //•	SupplierId – integer, Primary Key, foreign key(required)
        [Required]
        public int SupplierId {  get; set; }
        //•	Supplier – Supplier
        [Required]
        [ForeignKey(nameof(SupplierId))]
        public Supplier Supplier { get; set; } = null!;

        //•	ServiceId – integer, Primary Key, foreign key(required)
        [Required]
        public int ServiceId { get; set; }

        //•	Service – Service
        [Required]
        [ForeignKey(nameof(ServiceId))]
        public Service Service { get; set; } = null!;

    }
}
