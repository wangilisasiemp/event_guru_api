using System;
namespace event_guru_api.Utils
{
    public static class EventTypes
    {
        public const string Seminar = "Seminar"; //transport:no, food :yes, mc:no,drinks:yes
        public const string Wedding = "Wedding"; //transport:yes, food :yes, mc:yes,drinks:yes
        public const string Meeting = "Meeting"; //transport:no, food:yes,mc:no,drinks:yes
        public const string Congregation = "Congregation"; //transport:no,food:yes,mc:yes,drinks:yes,
        public const string Show = "Show"; //transport:no, food:no,mc:yes,drinks:no,
        public const string Party = "Party";//transport:no,food:yes,mc:yes,drinks:yes
    }
    public static class BudgetTypes
    {
        public const string Bronze = "Bronze";
        public const string Silver = "Silver";
        public const string Gold = "Gold";
    }
    public static class VendorTypes
    {
        public const string Caterer = "Caterer";
        public const string Drinks = "Drinks";
        public const string MC = "MC";
        public const string ConferenceHall = "Conference Hall";
        public const string RoyalTransport = "Royal Transport";
        public const string Decoration = "Decoration";
        public const string OrdinaryTransport = "Ordinary Transport";
        public const string Security = "Security";
        public const string Entertainment = "Entertainment";
    }
}

