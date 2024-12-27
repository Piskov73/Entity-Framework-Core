using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace Artillery.Data.Models
{
    public class Shell
    {
        public Shell()
        {
            this.Guns = new HashSet<Gun>();
        }
        //•	Id – integer, Primary Key
        [Required]
        public int Id { get; set; }

        //•	ShellWeight – double in range[2…1_680] (required)
        [Required]
        public double ShellWeight { get; set; }

        //•	Caliber – text with length[4…30] (required)
        [Required]
        [StringLength(30)]
        public string Caliber { get; set; } = null!;

        //•	Guns – a collection of Gun
        public ICollection<Gun> Guns { get; set; }

    }
}
