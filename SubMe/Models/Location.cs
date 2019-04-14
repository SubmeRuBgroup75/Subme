using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SubMe.Models.DAL;

namespace SubMe.Models
{
    public class Location
    {
        public int SubletID { get; set; }
        public string City { get; set; }
        public string Rout { get; set; }
        public string StreetAddress { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string PlaceID { get; set; }

        public Location(int _subletid, string _city, string _rout, string _streetaddress, string _lat, string _lng, string _placeid)
        {
            SubletID = _subletid;
            City = _city;
            Rout = _rout;
            StreetAddress = _streetaddress;
            Lat = _lat;
            Lng = _lng;
            PlaceID = _placeid;
        }

        public Location() { }

        public List<Location> GetLocations()
        {
            DBservices dbs = new DBservices();
            return dbs.GetLocations();
        }

        public List<Location> GetSearchedLocations(string IdString)
        {
            DBservices dbs = new DBservices();
            return dbs.GetSearchedLocations(IdString);
        }
    }
}