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

        [ActionName("GetMedalsByCountry")]
        [ResponseType(typeof(IQueryable<MedalsByCountry>))]
        public IHttpActionResult GetMedalsByCountry([FromUri] int[] ids, [FromUri] int startYear, [FromUri] int endYear)
        {
            var years = db.games
                            .Where(game => game.year >= startYear && game.year <= endYear)
                            .ToDictionary(key => key.id, value => value.year);

            var gamesIdArr = years.Select(item => item.Key);

            var coutries = db.countries
                            .Where(country => ids.Contains(country.id))
                            .ToDictionary(key => key.id, value => value.name);

            var results = db.results.Where(result => ids.Contains(result.country) && gamesIdArr.Contains(result.game));

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
    }
}