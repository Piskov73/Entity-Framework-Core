using System.ComponentModel.DataAnnotations;
using Trucks.Data.Models;

namespace Trucks.DataProcessor.ImportDto
{
    public class ImportClientDto
    {
        //•	Name – text with length[3, 40] (required)
        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        //•	Nationality – text with length[2, 40] (required)
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Nationality { get; set; } = null!;

        //•	Type – text(required)
        [Required]
        public string Type { get; set; } = null!;

        //•	ClientsTrucks – collection of type ClientTruck
        public int[] Trucks { get; set; } = null!;
    }
}