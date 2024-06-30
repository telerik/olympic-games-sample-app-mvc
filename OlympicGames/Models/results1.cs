using System;
using System.ComponentModel.DataAnnotations;

namespace OlympicGames.Models
{
    public partial class results1
    {
        [Key]
        public int id { get; set; }
        public int athlete { get; set; }
        public int game { get; set; }
        public int @event { get; set; }
        public int country { get; set; }
        public Nullable<int> medal { get; set; }
        public string result { get; set; }
    }
}
