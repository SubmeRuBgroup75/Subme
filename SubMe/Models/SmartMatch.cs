using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubMe.Models
{
    public class SmartMatch
    {
        public static readonly int[] subletDurationCategory = { 3, 5, 10, 14, 21, 30, 90 }; //The duration of sublet ads that the user (searcher) marked as a like

        public static readonly int[] maxPriceCategory = { 1500, 3000, 4500, 6000, 7500, 9000, 10500, 12000 }; //The maximum price a user searched for a sublet ad

        public static readonly int[] commonCountryAreaCategory = { 0, 4, 2, 3, 7 }; //In order to find the user's most common area search at israel (haifa, jerusalem,tlv, eilat)
                                                                                                                    //         0               1        2        3 
        enum Weights { duration = 30, price = 40, city = 30};

        public Sublet AdPost { get; set; }
        public UserProfile Profile { get; set; }   // including duration and price onley 
        public CityBelonging City { get; set; }
  

        public SmartMatch(Sublet _ad, UserProfile _profile, CityBelonging _city)
        {
            AdPost = _ad;
            Profile = _profile;
            City = _city;
        }

        //  פונקציה שסורקת את מאגר המודעות הגולמי ומקטלגת כל מודעה לווקטור שיוך ומפעילה את שאר הפונקציות של האלגוריתם  
        public int MatchPercentages(Sublet AdPost)
        {
            DateTime parsedDateCheckOut, parsedDateCheckIn;
            TimeSpan diffResult;
            int Percentages = 0;
            // AdVector including the values of the add, exp - sublet for 4 night 4700 nis center tlv
          
                parsedDateCheckOut = AdPost.CheckOut;
                parsedDateCheckIn = AdPost.CheckIn;
                diffResult = parsedDateCheckOut.Subtract(parsedDateCheckIn);

            //    Percentages = CalculateMatchPercentages(diffResult, AdPost.Price, AdPost.CityKod, AdPost.SubletID); // להוסיף לפונקציה גם את הפרופיל האישי של המשתמש כדי לבצע השוואות 
           
                return Percentages;
        }

        // מקבלת ווקטור של מודעה מסויימת ומחשבת אחוזי התאמה עם פרופיל משתמש (מחפש) מסויים ומחזירה את אחוז ההתאמה  
        public float CalculateMatchPercentages(int durationAd,int priceAd, int cityAd,int adId)
        {
            float match = 0;
            int fit, indexAdVectorProp = -1;

            //לולאה שמתאימה מספר אינקדס למודעה במאפיין משך הסאבלט 
            for (int i = 1; i <= subletDurationCategory.Length; i++)
            {
                if (durationAd <= subletDurationCategory[i])
                {
                    indexAdVectorProp = i;
                    break;
                }

                fit = IsPriceOrDurationFit(Profile.DurationKod, Profile.DurationDeviationValue, indexAdVectorProp, subletDurationCategory, durationAd);
                match = match + fit * (Profile.DurationBelonging * (int)(Weights.duration) / 100);
            }

            //לולאה שמתאימה מספר אינקדס למודעה במאפיין מחיר הסאבלט 
            for (int i = 1; i <= maxPriceCategory.Length; i++)
            {
                if (priceAd <= maxPriceCategory[i])
                {
                    indexAdVectorProp = i;
                    break;
                }

                fit = IsPriceOrDurationFit(Profile.PriceKod, Profile.PriceDeviationValue, indexAdVectorProp, maxPriceCategory, priceAd);
                match = match + fit * (Profile.PriceBelonging * (int)(Weights.price) / 100);
            }

            fit = IsCityFit(City, cityAd);
            match = match + fit * ((int)Weights.city) / 100;

            return match;
        }

        // , מחזירה 0 או 1 פונקציה שמקבלת שני ערכים  מהווקטורים , מווקטור השיוך למחפש ומווקטור המודעה ומחזירה האם הם מתאימים  
        public int IsPriceOrDurationFit(int searcherIndexVector, float deviationValue, int indexAdVectorProp, int [] vectorCategory, int AdPropValue)
        {
            if (indexAdVectorProp == -1)
            {
                return 0;
            }
            else
            {
                if (searcherIndexVector == indexAdVectorProp)
                {
                    return 1;
                }

                if (AdPropValue <= vectorCategory[searcherIndexVector] * deviationValue)
                {
                    return 1;
                }
                return 0;
            }
        }

        public int IsCityFit(CityBelonging city, int adCityKod)
        {
            float Percent = 0;

            if (adCityKod == 0 && city.WithoutPreferencePercent >= 0.65)
            {
                return 1;
            }
            else
            {
                switch (adCityKod)
                {

                    case 0:
                        Percent = city.WithoutPreferencePercent;
                        break;

                    case 4:
                        Percent = city.WithoutPreferencePercent + city.HaifaPercent;
                        break;

                    case 2:
                        Percent = city.WithoutPreferencePercent + city.JerusalemPercent;
                        break;

                    case 3:
                        Percent = city.WithoutPreferencePercent + city.TlvPercent;
                        break;

                    case 7:
                        Percent = city.WithoutPreferencePercent + city.EilatPercent;
                        break;
                }
                if (Percent >= 0.65)
                {
                    return 1;
                }
                return 0;
            }
        }
    }
}