using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trucks.Data.Models
{
    public class ClientTruck
    {
        //•	ClientId – integer, Primary Key, foreign key(required)
        [Required]
        public int ClientId { get; set; }

        //•	Client – Client
        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; } = null!;

        //•	TruckId – integer, Primary Key, foreign key(required)
        [Required]
        public int TruckId { get; set; }

        //•	Truck – Truck
        public Truck Truck { get; set; } = null!;

    }
}