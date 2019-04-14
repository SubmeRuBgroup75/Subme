using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SubMe.Models.DAL;

namespace SubMe.Models
{
    public class Search
    {
        public int SearchID { get; set; }
        public string City { get; set; }
        public string EnterDate { get; set; }
        public string ExitDate { get; set; }
        public string Type { get; set; }
        public string Rooms { get; set; }
        public int MinBudjet { get; set; }
        public int MaxBudjet { get; set; }

        public Search() { }

        public Search(string city, string checkIn, string checkOut, string type, string nomOfRooms, int minBudjet, int maxBudjet)
        {
            City = city;
            EnterDate = checkIn;
            ExitDate = checkOut;
            Type = type;
            Rooms = nomOfRooms;
            MinBudjet = minBudjet;
            MaxBudjet = maxBudjet;
        }
    }
}