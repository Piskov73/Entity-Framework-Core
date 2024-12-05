﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TravelAgency.DataProcessor.ImportDtos
{
    public class BookingImportDTO
    {
        [Required]
        public string BookingDate { get; set; } = null!;

        [Required]
        public string CustomerName { get; set; } = null!;


        [Required]
        public string TourPackageName { get; set; } = null!;

       

    }
}
