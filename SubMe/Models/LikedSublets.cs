using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SubMe.Models.DAL;

namespace SubMe.Models
{
    public class LikedSublets
    {
        public string UserFBID { get; set; }
        public int SubletID { get; set; }

        public LikedSublets(string userFBID, int subletID)
        {
            UserFBID = userFBID;
            SubletID = subletID;
        }

        public LikedSublets()
        {
        }

        public int UpdateLikedSublets(LikedSublets[] ls)
        {
            DBservices dbs = new DBservices();
            return dbs.UpdateLikedSublets(ls);
        }

        public int DeleteLikedSublet(LikedSublets ls)
        {
            DBservices dbs = new DBservices();
            return dbs.DeleteLikedSublet(ls);
        }
    }
}