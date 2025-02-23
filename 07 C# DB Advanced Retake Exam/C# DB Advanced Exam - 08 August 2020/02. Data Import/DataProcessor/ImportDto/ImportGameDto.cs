using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VaporStore.Data.Models;

namespace VaporStore.DataProcessor.ImportDto
{
    public class ImportGameDto
    {
        //•	Name – text(required)
        [Required]
        public string Name { get; set; } = null!;

        //•	Price – decimal (non-negative, minimum value: 0) (required)
        [Required]
        [Range(0,double.MaxValue)]
        public decimal Price { get; set; }

        //•	ReleaseDate – Date(required)
        [Required]
        public string ReleaseDate { get; set; } = null!;

        [Required]
        public string Developer { get; set; } = null!;

        [Required]
        public string Genre { get; set; } = null!;

        public string[] Tags { get; set; } = new string[] { };
    }
}
