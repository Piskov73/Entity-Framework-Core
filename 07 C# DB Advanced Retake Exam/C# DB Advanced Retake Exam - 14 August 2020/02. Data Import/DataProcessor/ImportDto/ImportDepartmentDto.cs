using SoftJail.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportDepartmentDto
    {
        //•	Name – text with min length 3 and max length 25 (required)
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; } = null!;

        //•	Cells - collection of type Cell
        public ImportCellDto[] Cells { get; set; } = new ImportCellDto[] { };
    }
}
