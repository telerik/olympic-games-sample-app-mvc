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
    public class ResultsController : ApiController
    {
        private OlympicsEntities db;

        public ResultsController()
        {
            this.db = new OlympicsEntities();
        }

        public ResultsController(OlympicsEntities db)
        {
            this.db = db;
        }

        // GET: api/Results
        public IQueryable<result> GetResults()
        {
            return db.results;
        }

        //GET: api/Results/5
        [ResponseType(typeof(result))]
        public IHttpActionResult GetResult(int id)
        {
            result sport = db.results.Single(x => x.id == id);
            if (sport == null)
            {
                return NotFound();
            }

            return Ok(sport);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Route("api/results/GetMedalsByCountry")]
        [ResponseType(typeof(IQueryable<MedalsByCountry>))]
        public IHttpActionResult GetMedalsByCountry([FromUri] int[] countryIds, [FromUri] int startYear, [FromUri] int endYear)
        {
            var years = db.games
                            .Where(game => game.year >= startYear && game.year <= endYear)
                            .ToDictionary(key => key.id, value => value.year);

            var gamesIdArr = years.Select(item => item.Key);

            var coutries = db.countries
                            .Where(country => countryIds.Contains(country.id))
                            .ToDictionary(key => key.id, value => value.name);

            var results = db.results.Where(result => countryIds.Contains(result.country) && gamesIdArr.Contains(result.game));

            Dictionary<int, MedalsByCountry> data = new Dictionary<int, MedalsByCountry>();

            foreach (var item in results)
            {
                var hashCode = (coutries[item.country] + years[item.game]).GetHashCode();
                if (data.ContainsKey(hashCode))
                {
                    var oldMedal = data[hashCode].Medals;
                    data[hashCode].Medals += Convert.ToInt32(item.medal.HasValue);
                }
                else
                {
                    data.Add(hashCode, new MedalsByCountry
                    {
                        Country = coutries[item.country],
                        Year = years[item.game],
                        Medals = Convert.ToInt32(item.medal.HasValue)
                    });
                }
            }

            return Ok(data.Select(x => x.Value).AsQueryable());
        }

        [Route("api/results/GetPivotGridResultItems")]
        [ResponseType(typeof(IQueryable<PivotGridResultItem>))]
        public IHttpActionResult GetPivotGridResultItems([FromUri] int[] countryIds, [FromUri] int[] sportIds, [FromUri] int startYear, [FromUri] int endYear)
        {
            var mainSportsById = db.sports.ToDictionary(key => key.id, value => value.name);

            var games = db.games.ToDictionary(key => key.id);

            var sports = db.sports.ToDictionary(key => key.id);

            var athletes = db.athletes.ToDictionary(key => key.id);

            var contries = db.countries.ToDictionary(key => key.id);

            if (countryIds.Length == 0)
            {
                countryIds = contries.Select(x => x.Value.id).ToArray();
            }

            var athletesBySportArr = new List<int>();

            if (sportIds.Length == 0)
            {
                athletesBySportArr = athletes.Select(x => x.Value.id).ToList();
            }
            else
            {
                athletesBySportArr = athletes.Where(x => sportIds.Contains(x.Value.sport)).Select(x => x.Value.id).ToList();
            }

            var results = db.results.Where(item => item.medal.HasValue &&
                                                   countryIds.Contains(item.country) &&
                                                   athletesBySportArr.Contains(item.athlete) &&
                                                   db.games.FirstOrDefault(x => x.id == item.game).year >= startYear &&
                                                   db.games.FirstOrDefault(x => x.id == item.game).year <= endYear)
                                                   .OrderBy(x => x.medal);

            var data = new List<PivotGridResultItem>();

            foreach (var result in results)
            {
                var MedalType = ((MedalType) result.medal).ToString("F");
                var CountryName = contries[result.country].name;
                var OlympicsGame = String.Format("{0} {1} {2}",
                                      games[result.game].year, contries[games[result.game].country].name, games[result.game].city);
                var OlympicsYear = games[result.game].year;
                var SportName = sports[athletes[result.athlete].sport].name;


                data.Add(new PivotGridResultItem
                {
                    MedalType = MedalType,
                    CountryName = CountryName,
                    OlympicsGame = OlympicsGame,
                    OlympicsYear = OlympicsYear,
                    SportName = SportName
                });
            }

            return Ok(new { Data = data });
        }
    }
}