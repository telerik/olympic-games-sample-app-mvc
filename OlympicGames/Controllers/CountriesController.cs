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

namespace OlympicGames.Controllers
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
        public IQueryable<country> Getcountries()
        {
            return db.countries;
        }

        // GET: api/Countries/5
        [ResponseType(typeof(country))]
        public IHttpActionResult Getcountry(int id)
        {
            country country = db.countries.Find(id);
            if (country == null)
            {
                return NotFound();
            }

            return Ok(country);
        }

        // PUT: api/Countries/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putcountry(int id, country country)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != country.id)
            {
                return BadRequest();
            }

            db.Entry(country).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!countryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Countries
        [ResponseType(typeof(country))]
        public IHttpActionResult Postcountry(country country)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.countries.Add(country);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (countryExists(country.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = country.id }, country);
        }

        // DELETE: api/Countries/5
        [ResponseType(typeof(country))]
        public IHttpActionResult Deletecountry(int id)
        {
            country country = db.countries.Find(id);
            if (country == null)
            {
                return NotFound();
            }

            db.countries.Remove(country);
            db.SaveChanges();

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

        private bool countryExists(int id)
        {
            return db.countries.Count(e => e.id == id) > 0;
        }
    }
}