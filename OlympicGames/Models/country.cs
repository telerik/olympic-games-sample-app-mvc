using System.ComponentModel.DataAnnotations;

namespace OlympicGames.Models
{
    public partial class country
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string abbr { get; set; }
    }
}
