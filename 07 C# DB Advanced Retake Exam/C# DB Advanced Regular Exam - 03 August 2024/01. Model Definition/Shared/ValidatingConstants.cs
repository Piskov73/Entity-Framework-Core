namespace TravelAgency.Shared
{
    public static class ValidatingConstants
    {

        //Customer
        public const int CustomerNameMinLehgth = 4;
        public const int CustomerNameMaxLehgth = 60;

        public const int EmailMinLength = 6;
        public const int EmailMaxLength = 50;

      
        public const int  PhoneNumberLegnth= 13;
        public const string ValidPhoneNumberPattern = @"^\+\d{12}$";

        public const int PackageNameMinLeghth = 2;
        public const int PackageNameMaxLeghth = 40;

        public const int DescriptionLeghth = 200;

        //Guide

        //•	FullName – text with length[4, 60] (required)
        public const int GuideNameMinLehgth = 4;
        public const int GuideNameMaxLehgth = 60;

    }
}
