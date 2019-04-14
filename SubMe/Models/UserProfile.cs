using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SubMe.Models.DAL;

namespace SubMe.Models
{
    public class UserProfile
    {
        public string UserId { get; set; }
        public int DurationKod { get; set; }
        public float DurationBelonging { get; set; }
        public float DurationDeviationValue { get; set; }
        public int PriceKod { get; set; }
        public float PriceBelonging { get; set; }
        public float PriceDeviationValue { get; set; }

        public UserProfile(string _id, int _Duration, float _DurationBelonging, float _DurationDeviationValue, int _Price, float _PriceBelonging, float _PriceDeviationValue)
        {
            UserId = _id;
            DurationKod = _Duration;
            DurationBelonging = _DurationBelonging;
            DurationDeviationValue = _DurationDeviationValue;
            PriceKod = _Price;
            PriceBelonging = _PriceBelonging;
            PriceDeviationValue = _PriceDeviationValue;
        }

        public UserProfile()
        {

        }


        //public UserProfile ReturnOneUserProfile(int id)
        //{
        //    DBservices dbs = new DBservices();
        //    UserProfile UP = dbs.ReturnOneUserProfile(id);
        //    return UP;
        //}


        public int InsertUserProfile(UserProfile profile)
        {
            DBservices dbs = new DBservices();
            int numAffected = dbs.InsertUserProfile(profile);
            return numAffected;
        }
    }
}