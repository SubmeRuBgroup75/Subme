using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SubMe.Models.DAL;

namespace SubMe.Models
{
    public class UserPreferences
    {
        public string UserId { get; set; }
        public int DurationKod { get; set; }
        public int PriceKod { get; set; }
        public int CityKod { get; set; }
        public string CityArea { get; set; }

        public UserPreferences(string _id, int _durationkod, int _pricekod, int _citykod, string _cityarea)
        {
            UserId = _id;
            DurationKod = _durationkod;
            PriceKod = _pricekod;
            CityKod = _citykod;
            CityArea = _cityarea;
        }

        public UserPreferences()
        {
        }

        public int InsertUserPreferences(UserPreferences newP)
        {
            DBservices dbs = new DBservices();
            return dbs.InsertUserPreferences(newP);
        }

        public List <UserPreferences> GetListOfSearchAttribute(string uid)
        {
            DBservices dbs = new DBservices();
            return dbs.GetListOfSearchAttribute(uid);
        }
    }
}