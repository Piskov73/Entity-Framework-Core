using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Artillery.Data.Models
{
    public class Manufacturer
    {
        public Manufacturer()
        {
            this.Guns = new HashSet<Gun>();
        }
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	ManufacturerName – unique text with length[4…40] (required)
        [Required]
        [StringLength(40)]
        public string ManufacturerName { get; set; } = null!;

        //•	Founded – text with length[10…100] (required)
        [Required]
        [StringLength(100)]
        public string Founded { get; set; } = null!;

        //•	Guns – a collection of Gun
        public ICollection<Gun> Guns { get; set; }

    }
}
