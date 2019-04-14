using SubMe.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SubMe.Models;

namespace SubMe.Models
{
    public class CityBelonging
    {
        public string UserId { get; set; }
        public float WithoutPreferencePercent { get; set; }
        public float HaifaPercent { get; set; }
        public float JerusalemPercent { get; set; }
        public float TlvPercent { get; set; }
        public float EilatPercent { get; set; }

        public CityBelonging(string _id, float _WithoutPreferencePercent, float _HaifaPercent,
                     float _JerusalemPercent, float _TlvPercent, int _EilatPercent)
        {
            UserId = _id;
            WithoutPreferencePercent = _WithoutPreferencePercent;
            HaifaPercent = _HaifaPercent;
            JerusalemPercent = _JerusalemPercent;
            TlvPercent = _TlvPercent;
            EilatPercent = _EilatPercent;
        }

        public CityBelonging()
        {

        }

        public CityBelonging ReturnOneUserCityBelonging(string id)
        {
            DBservices dbs = new DBservices();
            CityBelonging CB = dbs.ReturnOneUserCityBelonging(id);
            return CB;
        }

        public int InsertCityBelonging(CityBelonging CB)
        {
            DBservices dbs = new DBservices();
            int numAffected = dbs.InsertOneUserCityBelonging(CB); 
            return numAffected;
        }

    }
}