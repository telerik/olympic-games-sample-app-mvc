using System;
using System.ComponentModel.DataAnnotations;

namespace OlympicGames.Models
{
    public partial class athlete
    {
        [Key]
        public int id { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public int country { get; set; }
        public int sport { get; set; }
        public Nullable<System.DateTime> birthday { get; set; }
        public string url { get; set; }
    }
}
