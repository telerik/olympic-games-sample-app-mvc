using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OlympicGames.Models
{
    public class MedalsByCountry
    {
        public string Country { get; set; }
        public int Year { get; set; }
        public int Medals { get; set; }
    }
}