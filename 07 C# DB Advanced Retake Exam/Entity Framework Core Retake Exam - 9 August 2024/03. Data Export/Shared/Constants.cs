using static System.Net.Mime.MediaTypeNames;

namespace NetPay.Shared
{
    public static class Constants
    {
        //Household

        public const int ContactPersonMinLength = 5;
        public const int ContactPersonMaxLength = 50;

        public const int EmailMinLegngth = 6;
        public const int EmailMaxLegngth = 80;

        public const int PhoneNumberMinLength = 15;
        public const int PhoneNumberMaxLength = 15;
        public const string PhoneNumberValidation = @"^\+\d{3}/\d{3}-\d{6}$";

        //Expense

        public const int ExpenseNameMinLength = 5;
        public const int ExpenseNameMaxLength = 50;
        //•	Amount - a decimal value in the range[0.01, 100 000](required)
        //•	DueDate - DateTime(required)
        //•	PaymentStatus – PaymentStatus enum (Paid = 1, Unpaid, Overdue, Expired) (required)
        //•	HouseholdId - integer, foreign key(required)
        //•	Household – Household
        //•	ServiceId - integer, foreign key(required)
        //•	Service - Service

        //Service
        public const int ServiceNameMinLength = 5;
        public const int ServiceNameMaxLength = 30;

        //Supplier
        //•	SupplierName – text with length[3, 60] (required)
        public const int SupplierNameMinLehgth = 3;
        public const int SupplierNameMaxLehgth = 60;

    }
}
