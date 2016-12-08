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
    public class CountriesController : ApiController
    {
        private OlympicsEntities db ;

        public CountriesController()
            : base()
        {
            this.db = new OlympicsEntities();
        }

        public CountriesController(OlympicsEntities db)
            :base()
        {
            
            this.db = db;
        }

        // GET: api/Countries
        public IQueryable<country> GetCountries()
        {
            return db.countries;
        }

        // GET: api/Countries/5
        [ResponseType(typeof(country))]
        public IHttpActionResult GetCountry(int id)
        {
            country country = db.countries.Single(x => x.id == id);
            if (country == null)
            {
                return NotFound();
            }

            return Ok(country);
        }

        public IQueryable<country> GetCountriesWithResult([FromUri] bool flag, int? gameId, string countriesFilter)
        {
            var result = db.results.AsQueryable();

            if (gameId != null && gameId != 0)
            {
                result = result.Where(r => r.game == gameId);
            }
            var distCountriesResult = result.Select(s => s.country).Distinct();

            var countries = db.countries.Where(x => distCountriesResult.Contains(x.id));
            if (!string.IsNullOrEmpty(countriesFilter))
            {
                countries = countries.Where(c => c.name.Contains(countriesFilter));
            }
            return countries;
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