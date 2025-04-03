using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cadastre.Data.Models
{
    public class PropertyCitizen
    {
        //•	PropertyId – integer, Primary Key, foreign key(required)
        [Required]
        public int PropertyId { get; set; }

        //•	Property – Property
        [ForeignKey(nameof(PropertyId))]
        public Property Property { get; set; } = null!;

        //•	CitizenId – integer, Primary Key, foreign key(required)
        [Required]
        public int CitizenId { get; set; }

        //•	Citizen – Citizen
        [ForeignKey(nameof(CitizenId))]
        public Citizen Citizen { get; set; } = null!;

    }
}