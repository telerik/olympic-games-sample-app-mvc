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
    public class TopSportsMedalsController : ApiController
    {
        private OlympicsEntities db;

        public TopSportsMedalsController()
        {
            this.db = new OlympicsEntities();
        }

        public TopSportsMedalsController(OlympicsEntities db)
        {
            this.db = db;
        }

        private static int GetUniqueMedalKey(result result)
        {
            return int.Parse(result.game.ToString() + result.country.ToString() + result.@event.ToString() + result.medal.Value.ToString());
        }

        [ActionName("GetTopSportsMedals")]
        [ResponseType(typeof(IQueryable<MedalsBySport>))]
        public IHttpActionResult GetTopSportsMedals([FromUri]int? gameId = 0, [FromUri]int? countryId = 0, [FromUri]int limit = 5)
        {
            var mainSportsById = db.sports
                            .Where(s => !s.category.HasValue)
                            .ToDictionary(key => key.id, value => value.name);

            var results = db.results
                            .Where(r => r.country == countryId && r.medal.HasValue);

            var sports = db.sports;

            HashSet<int> uniqueMedals = new HashSet<int>();

            Dictionary<string, MedalsBySport> topSportsByMedal = new Dictionary<string, MedalsBySport>();

            foreach (var result in results)
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
                string sportCategoryName = mainSportsById[sportCategory];

                if (!topSportsByMedal.ContainsKey(sportCategoryName))
                {
                    topSportsByMedal.Add(sportCategoryName, new MedalsBySport
                    {
                                Sport= sportCategoryName,
                                Medals = 0
                    });
                }
                topSportsByMedal[sportCategoryName].Medals++;
            }
            return Ok(topSportsByMedal.Select(i => i.Value).OrderByDescending(i=>i.Medals).Take(limit).AsQueryable());
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