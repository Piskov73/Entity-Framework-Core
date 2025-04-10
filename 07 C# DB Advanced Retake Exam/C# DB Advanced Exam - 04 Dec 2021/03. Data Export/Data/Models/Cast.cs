﻿using static System.Net.Mime.MediaTypeNames;
using System.Numerics;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theatre.Data.Models
{
    public class Cast
    {
        //        •	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	FullName – text with length[4, 30] (required)
        [Required]
        [MaxLength(30)]
        public string FullName { get; set; } = null!;

        //•	IsMainCharacter – Boolean represents if the actor plays the main character in a play(required)
        [Required]
        public bool IsMainCharacter { get; set; }

        //•	PhoneNumber – text in the following format: "+44-{2 numbers}-{3 numbers}-{4 numbers}".
        //Valid phone numbers are: +44-53-468-3479, +44-91-842-6054, +44-59-742-3119 (required)
        [Required]
        public string PhoneNumber { get; set; } = null!;

        //•	PlayId – integer, foreign key(required)
        [Required]
        public int PlayId { get; set; }
        [ForeignKey(nameof(PlayId))]
        public Play Play { get; set; } = null!;

    }
}