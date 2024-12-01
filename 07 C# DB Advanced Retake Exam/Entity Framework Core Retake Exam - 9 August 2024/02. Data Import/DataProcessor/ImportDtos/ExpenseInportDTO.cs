namespace NetPay.DataProcessor.ImportDtos
{

    using System.ComponentModel.DataAnnotations;
    using NetPay.Data.Models.Enums;

    using static Shared.Constants;
    public class ExpenseInportDTO
    {
        //•	ExpenseName – text with length[5, 50] (required)
        [Required]
        [MinLength(ExpenseNameMinLength)]
        [MaxLength(ExpenseNameMaxLength)]
        public string ExpenseName { get; set; } = null!;

        //•	Amount - a decimal value in the range[0.01, 100 000](required)
        [Required]
        [Range(0.01,100000.00)]
        public decimal Amount { get; set; }

        //•	DueDate - DateTime(required)
        [Required]
        public string DueDate { get; set; }=null!;

        //•	PaymentStatus – PaymentStatus enum (Paid = 1, Unpaid, Overdue, Expired) (required)
        [Required]
        [EnumDataType(typeof(PaymentStatus))]
        public string PaymentStatus { get; set; } = null!;

        //•	HouseholdId - integer, foreign key(required)
        [Required]
        public int HouseholdId { get; set; }



        //•	ServiceId - integer, foreign key(required)
        [Required]
        public int ServiceId { get; set; }


    }
}
