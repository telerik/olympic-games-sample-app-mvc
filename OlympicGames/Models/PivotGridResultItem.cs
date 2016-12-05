using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OlympicGames.Models
{
    public class PivotGridResultItem
    {
        public string MedalType { get; set; }
        public string SportName { get; set; }
        public string CountryName { get; set; }
        public int OlympicsYear { get; set; }
        public string OlympicsGame { get; set; }
    }
}