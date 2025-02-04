using System.ComponentModel.DataAnnotations;

namespace Theatre.DataProcessor.ImportDto
{
    public class ImportTicketDto
    {
        //•	Price – decimal in the range[1.00….100.00] (required)
        [Required]
        [Range(1.00, 100.00)]
        public decimal Price { get; set; }

        //•	RowNumber – sbyte in range[1….10] (required)
        [Required]
        [Range(1, 10)]
        public sbyte RowNumber { get; set; }

        //•	PlayId – integer, foreign key(required)
        [Required]
        public int PlayId { get; set; }
       
      
    }
}