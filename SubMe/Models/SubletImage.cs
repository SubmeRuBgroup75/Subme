using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SubMe.Models.DAL;

namespace SubMe.Models
{
    public class SubletImage
    {
        public int SubletID { get; set; }
        public string ImagePath1 { get; set; }
        public string ImagePath2 { get; set; }
        public string ImagePath3 { get; set; }
        public string ImagePath4 { get; set; }
        public string ImagePath5 { get; set; }

        public SubletImage(int subletID, string imagePath1, string imagePath2, string imagePath3, string imagePath4, string imagePath5)
        {
            SubletID = subletID;
            ImagePath1 = imagePath1;
            ImagePath2 = imagePath2;
            ImagePath3 = imagePath3;
            ImagePath4 = imagePath4;
            ImagePath5 = imagePath5;
        }

        public SubletImage()
        {

        }

        public List<SubletImage> GetSubletImages(string SubId)
        {
            DBservices dbs = new DBservices();
            return dbs.GetSubletImages(SubId);
        }

    }
}