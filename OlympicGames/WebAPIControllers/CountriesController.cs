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