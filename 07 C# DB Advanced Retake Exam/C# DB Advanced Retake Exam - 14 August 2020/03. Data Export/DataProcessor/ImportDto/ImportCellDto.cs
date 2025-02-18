using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportCellDto
    {
        //•	CellNumber – integer in the range[1, 1000] (required)
        [Required]
        [Range(1,1_000)]
        public int CellNumber { get; set; }

        //•	HasWindow – bool (required)
        [Required]
        public string HasWindow { get; set; } = null!;
    }
}