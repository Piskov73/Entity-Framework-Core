namespace Invoices.Constants
{
    public static class Constants
    {
        //Product
        public const int MinLengthProductName = 9;
        public const int MaxLengthProductName = 30;

        //Address
        public const int MinLengthStreetName = 10;
        public const int MaxLengthStreetName = 20;
        public const int MinLenghtCityName = 5;
        public const int MaxLenghtCityName = 15;
        public const int MinLenghtCountryName = 5;
        public const int MaxLenghtCountryName = 15;

        //Invoice
        public const int NumberMinRange = 1_000_000_000;
        public const int NumberMaxRange = 1_500_000_000;

        //Client

        public const int ClientNameMinLength = 10;
        public const int ClientNameMaxLength = 25;
        public const int NumberVatMinLength = 10;
        public const int NumberVatMaxLength = 15;


    }
}
