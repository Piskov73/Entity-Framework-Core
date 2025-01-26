using AutoMapper;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Security.AccessControl;
using System;
using System.ComponentModel.DataAnnotations;
using Trucks.Data.Models.Enums;

namespace Trucks.Data.Models
{
    public class Truck
    {

        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	RegistrationNumber – text with length 8
        //. First two characters are upper letters[A - Z],
        //followed by four digits and the last two characters are upper letters[A - Z] again.
        [Required]
        [MaxLength(8)]
        public string RegistrationNumber { get; set; } = null!;

        //•	VinNumber – text with length 17 (required)
        [Required]
        [MaxLength(17)]
        public string VinNumber { get; set; } = null!;

        //•	TankCapacity – integer in range[950…1420]
        [Required]
        public int TankCapacity { get; set; }

        //•	CargoCapacity – integer in range[5000…29000]
        [Required]
        public int CargoCapacity { get; set; }

        //•	CategoryType – enumeration of type CategoryType, with possible values
        //(Flatbed, Jumbo, Refrigerated, Semi) (required)
        [Required]
        public CategoryType CategoryType { get; set; }

        //•	MakeType – enumeration of type MakeType, with possible values
        //(Daf, Man, Mercedes, Scania, Volvo) (required)
        [Required]
        public MakeType MakeType { get; set; }

        //•	DespatcherId – integer, foreign key (required)
        [Required]
        public int DespatcherId { get; set; }

        //•	Despatcher – Despatcher
        public Despatcher Despatcher { get; set; } = null!;

        //•	ClientsTrucks – collection of type ClientTruck
        public ICollection<ClientTruck> ClientsTrucks { get; set; } = new HashSet<ClientTruck>();

    }
}
