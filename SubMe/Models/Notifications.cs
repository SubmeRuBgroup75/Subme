using SubMe.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubMe.Models
{
    public class Notifications
    {
        public List <Sublet> SubletList { get; set; }
        public string UserId { get; set; }   // including duration and price onley 
        public int MatchPercentages;
        public UserProfile Profile { get; set; }   // including duration and price onley 
        public CityBelonging CityB { get; set; }

        public Notifications(List <Sublet> _SubletList, string _UserId, int _MatchPercentages, UserProfile _Profile, CityBelonging _City)
        {
            SubletList = _SubletList;
            UserId = _UserId;
            MatchPercentages = _MatchPercentages;
            Profile = _Profile;
            CityB = _City;
        }

        public List <SubletToPush> NotificationsList() // פונקציה שמחזירה רשימה של מודעות להתריאה למשתמש
        {
            List <SubletToPush> STP = new List<SubletToPush>(); //צריך ליצור פה עוד משתנה שמכיל רק מודעה ואחוז התאמה 
            int MatchPercentages;
            foreach (var Sublet in SubletList)
            {
               SmartMatch SM = new SmartMatch(Sublet, Profile, CityB);
                MatchPercentages = SM.MatchPercentages(Sublet);

                if (MatchPercentages >= 0.65)
                {
                    SubletToPush NewPush = new SubletToPush(Sublet, MatchPercentages);
                    STP.Add(NewPush);
                }
            }

            return STP;
        }

        public List <SubletToPush> Notifications_Sorted_List() // פונקציה שמחזירה רשימה ממויינת של מודעות להתריאה למשתמש
        {
            List <SubletToPush> STP = NotificationsList();
            List <SubletToPush> SortedSTP = new List<SubletToPush>();
            float maxPercentage = SortedSTP[0].MatchPercentages;

            for (int i = 1; i < STP.Count; i++)
            {
                if (STP[i].MatchPercentages > maxPercentage)
                {
                    maxPercentage = STP[i].MatchPercentages;
                    SortedSTP.Add(STP[i]);
                    STP.Remove(STP[i]);
                }
        }
            return SortedSTP;
        }

        public void InsertNotifications()
        {
            DBservices dbs = new DBservices();
            List <SubletToPush> SortedNot = Notifications_Sorted_List();

            foreach (var SubletToPush in SortedNot)
            {
              dbs.InsertNotifications(UserId, SubletToPush.Ad.SubletID, SubletToPush.MatchPercentages);
            }
        }

    }
    }
