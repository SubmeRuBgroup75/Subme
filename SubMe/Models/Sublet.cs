using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SubMe.Models.DAL;

namespace SubMe.Models
{
    public class Sublet
    {
        public int SubletID { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int Price { get; set; }
        public int NomOfRooms { get; set; }
        public int SqMtr { get; set; }
        public int FloorNo { get; set; }
        public string Description { get; set; }
        public bool isFacebook { get; set; }
        public int Roommates { get; set; }

        public Sublet(int subletID, DateTime checkIn, DateTime checkOut, int price, int nomOfRooms, int sqMtr, int floorNo, string description, bool isFacebook, int roommates)
        {
            SubletID = subletID;
            CheckIn = checkIn;
            CheckOut = checkOut;
            Price = price;
            NomOfRooms = nomOfRooms;
            SqMtr = sqMtr;
            FloorNo = floorNo;
            Description = description;
            this.isFacebook = isFacebook;
            Roommates = roommates;
        }

        public Sublet() { }

        public List<Sublet> SearchSublets(string City, string EnterDate, string ExitDate, string Type, int Rooms, int MinBudjet, int MaxBudjet)
        {
            DBservices dbs = new DBservices();
            return dbs.SearchSublets(City, EnterDate, ExitDate, Type, Rooms, MinBudjet, MaxBudjet);
        }

        public List<Sublet> GetLikedSublets(string UserFBID)
        {
            DBservices dbs = new DBservices();
            return dbs.GetLikedSublets(UserFBID);
        }

        public List<Sublet> GetSubletSearchWithProp(string SubletQuery, string PropQuery)
        {
            DBservices dbs = new DBservices();
            return dbs.GetSubletSearchWithProp(SubletQuery, PropQuery);
        }

    }
}