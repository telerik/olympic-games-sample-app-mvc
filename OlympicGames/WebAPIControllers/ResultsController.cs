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
using System.Text.RegularExpressions;

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

        //GET: api/Result/5
        [ResponseType(typeof(result))]
        [Route("api/result/GetMedalsByCountry")]
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

            var countries = db.countries
                            .Where(country => countryIds.Contains(country.id))
                            .ToDictionary(key => key.id, value => value.name);

            var results = db.results.Where(result => countryIds.Contains(result.country) && gamesIdArr.Contains(result.game));

            Dictionary<int, MedalsByCountry> data = new Dictionary<int, MedalsByCountry>();

            foreach (var item in results)
            {
                var hashCode = (countries[item.country] + years[item.game]).GetHashCode();
                if (data.ContainsKey(hashCode))
                {
                    var oldMedal = data[hashCode].Medals;
                    data[hashCode].Medals += Convert.ToInt32(item.medal.HasValue);
                }
                else
                {
                    data.Add(hashCode, new MedalsByCountry
                    {
                        Country = countries[item.country],
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
            var results = db.results
                .Where(item => item.medal.HasValue &&
                                (countryIds.Count() > 0 ? countryIds.Contains(item.country) : db.countries.Any(x => x.id == item.country)) &&
                                (sportIds.Count() > 0 ? db.athletes.Where(x => x.id == item.athlete).Any(x => sportIds.Contains(x.sport)) : db.athletes.Any(x => x.id == item.athlete)) &&
                                db.games.FirstOrDefault(g => g.id == item.game).year >= startYear &&
                                db.games.FirstOrDefault(g => g.id == item.game).year <= endYear)
                .OrderBy(x => x.medal)
                .Select(result => new PivotGridResultItem
                {
                    MedalType = db.medals.FirstOrDefault(g => g.id == result.medal).name,
                    CountryName = db.countries.FirstOrDefault(g => g.id == result.country).name,
                    OlympicsGame = db.games.FirstOrDefault(g => g.id == result.game).year + " " + db.countries.FirstOrDefault(s => s.id == db.games.FirstOrDefault(g => g.id == result.game).country).name + " " + db.games.FirstOrDefault(g => g.id == result.game).city,
                    OlympicsYear = db.games.FirstOrDefault(g => g.id == result.game).year,
                    SportName = db.sports.FirstOrDefault(s => s.id == db.athletes.FirstOrDefault(g => g.id == result.athlete).sport).name
                })
                .ToList();

            return Ok(new { Data = results });
        }

        [Route("api/results/GetTopResults")]
        [ResponseType(typeof(IList<AthleteResult>))]
        public IHttpActionResult GetTopResults([FromUri] int sportId)
        {

            var athletes = db.athletes.ToDictionary(key => key.id, value => String.Format("{0} {1}", value.firstName, value.lastName));

            var results = db.results.Where(r => r.@event == sportId &&
                                                r.result1 != string.Empty &&
                                                r.result1 != "n/a")
                                    .ToList()
                                    .Select(item => new AthleteResult
                                    {
                                        Name = athletes[item.athlete],
                                        Result = isScoreResult(sportId) ? item.result1 + " points" :
                                                                 ((item.result1.Split('.')[0].Length == 1) ? "0:0" : "0:") + item.result1,
                                        NumResult = isScoreResult(sportId) ?
                                                            Decimal.Parse(item.result1) :
                                                            getTimeResult(item.result1)
                                    });


            if (isScoreResult(sportId))
            {
                results = results.OrderByDescending(ar => ar.NumResult).Take(10);
            }
            else
            {
                results = results.OrderBy(ar => ar.NumResult).Take(10);
            }

            return Ok(results);
        }

        private bool isScoreResult(int sportId)
        {
            int[] sportsWithScoreResult = new int[] { 154, 155, 896, 405, 437 };
            return sportsWithScoreResult.Contains(sportId);
        }

        private decimal getTimeResult(string result)
        {
            string[] resultSections = result.Split('.');
            string[] secondsSection = resultSections[0].Split(':');
            double resultInSeconds = 0;
            for (int i = 0; i < secondsSection.Count(); i++)
            {
                resultInSeconds += int.Parse(secondsSection[i]) * (int)Math.Pow(60, secondsSection.Count() - i - 1);
            }

            if (resultSections.Length > 1)
            {
                string miliseconds = resultSections[1].Split(' ')[0];
                resultInSeconds += double.Parse(miliseconds) / (double)Math.Pow(10, miliseconds.Length);
            }

            return (decimal)resultInSeconds;
        }
    }
}