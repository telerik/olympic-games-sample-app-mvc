using System.ComponentModel.DataAnnotations;

namespace OlympicGames.Models
{
    public partial class game
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public int country { get; set; }
        public short year { get; set; }
    }
}
