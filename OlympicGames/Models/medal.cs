using System.ComponentModel.DataAnnotations;

namespace OlympicGames.Models
{
    public partial class medal
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
    }
}
