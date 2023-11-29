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

            var results = db.results.Where(item => item.medal.HasValue).Take(5).OrderBy(x => x.medal);

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
                                                        Result = isScoreResult(sportId) ? item.result1 + " points":
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
                resultInSeconds += int.Parse(secondsSection[i]) * (int) Math.Pow(60, secondsSection.Count() - i - 1);
            }

            if (resultSections.Length > 1)
            {
                string miliseconds = resultSections[1].Split(' ')[0];
                resultInSeconds += double.Parse(miliseconds) / (double) Math.Pow(10, miliseconds.Length);
            }

            return (decimal) resultInSeconds;
        }
    }
}