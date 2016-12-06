using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using OlympicGames.Models;

namespace OlympicGames.WebApiControllers
{
    public class SportMedalsByCountryController : ApiController
    {
        private OlympicsEntities db;

        public SportMedalsByCountryController()
        {
            this.db = new OlympicsEntities();
        }

        public SportMedalsByCountryController(OlympicsEntities db)
        {
            this.db = db;
        }

        private static int GetUniqueMedalKey(result result)
        {
            return int.Parse(result.game.ToString() + result.country.ToString() + result.@event.ToString() + result.medal.Value.ToString());
        }

        [ActionName("GetSportMedalsByCountry")]
        [ResponseType(typeof(List<MedalsByType>))]
        public IHttpActionResult GetSportMedalsByCountry([FromUri]int gameId, [FromUri]int countryId, [FromUri]string sportName)
        {
            int[] medals = new int[3];

            var sports = db.sports;
            var results = db.results;


            Dictionary<int, string> mainSportsById = new Dictionary<int, string>();
            foreach (sport sport in sports.Where(s => !s.category.HasValue))
            {
                mainSportsById.Add(sport.id, sport.name);
            }
            HashSet<int> uniqueMedals = new HashSet<int>();
            foreach (result result in results.Where(r => r.country == countryId && r.medal.HasValue))
            {
                if (gameId > 0 && result.game != gameId) continue;

                int key = GetUniqueMedalKey(result);
                if (uniqueMedals.Contains(key))
                {
                    continue;
                }
                uniqueMedals.Add(key);

                int sportId = result.@event;
                int sportCategory = sports.Where(s => s.id == sportId).Select(s => s.category).SingleOrDefault().Value;

                if (mainSportsById[sportCategory] == sportName)
                {
                    medals[result.medal.Value - 1]++;
                }
            }
            // the array medals contains the medals count by type in the following order - gold, silver, bronze
            List<MedalsByType> medalsByType = new List<MedalsByType>();
            string[] medalTypes = new string[] { "Gold", "Silver", "Bronze" };
            string[] medalColors = new string[] { "#f1d05f", "#b9b9b9", "#c78a38" };
            for (int i = 0; i < 3; i++)
            {
                int medalsCount = medals[i];
                if (medalsCount > 0)
                {
                    medalsByType.Add(new MedalsByType() { Medals = medals[i], Type = medalTypes[i], Color = medalColors[i] });
                }                
            }
            return Ok(medalsByType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}