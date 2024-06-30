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
    public class SportsController : ApiController
    {
        private OlympicsEntities db;

        public SportsController()
        {
            this.db = new OlympicsEntities();
        }

        public SportsController(OlympicsEntities db)
        {
            this.db = db;
        }

        // GET: api/Sports
        public IQueryable<sport> GetSports()
        {
            return db.sports;
        }

        // GET: api/Sports
        [Route("api/Sports/NoCat")]
        public IQueryable<sport> GetNoCategorySports()
        {
            var sports = db.sports.ToList();
            return db.sports.Where(x => !x.category.HasValue);
        }

        // GET: api/Sports/5
        [ResponseType(typeof(sport))]
        public IHttpActionResult GetSport(int id)
        {
            sport sport = db.sports.Single(x => x.id == id);
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
    }
}