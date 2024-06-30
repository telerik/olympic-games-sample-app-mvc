using System;
using System.ComponentModel.DataAnnotations;

namespace OlympicGames.Models
{
    public partial class sport
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string abbr { get; set; }
        public Nullable<int> category { get; set; }
    }
}
