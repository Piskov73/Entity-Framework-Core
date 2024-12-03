namespace NetPay.Data.Models
{
    using NetPay.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static Shared.Constants;
    public class Expense
    {
        [Key]
        public int Id { get; set; }

        //•	ExpenseName – text with length[5, 50] (required)
        [Required]
        [StringLength(ExpenseNameMaxLength)]
        public string ExpenseName { get; set; } = null!;

        //•	Amount - a decimal value in the range[0.01, 100 000](required)
        [Required]
        public  decimal Amount { get; set; }

        //•	DueDate - DateTime(required)
        [Required]
        public DateTime DueDate { get; set; }

        //•	PaymentStatus – PaymentStatus enum (Paid = 1, Unpaid, Overdue, Expired) (required)
        [Required]
        public PaymentStatus PaymentStatus { get; set; }

        //•	HouseholdId - integer, foreign key(required)
        [Required]
        public int HouseholdId { get; set; }

        //•	Household – Household
        [ForeignKey(nameof(HouseholdId))]
        public Household Household { get; set; } = null!;

        //•	ServiceId - integer, foreign key(required)
        [Required]
        public int ServiceId { get; set; }

        //•	Service - Service
        [ForeignKey(nameof(ServiceId))]
        public Service Service { get; set; } = null!;

    }
}
