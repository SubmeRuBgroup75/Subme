using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubMe.Models
{
    public class SubletToPush
    {
        public Sublet Ad { get; set; }
        public int MatchPercentages;

        public SubletToPush(Sublet _Ad, int _MatchPercentages)
        {
            Ad = _Ad;
            MatchPercentages = _MatchPercentages;
        }
    }
}