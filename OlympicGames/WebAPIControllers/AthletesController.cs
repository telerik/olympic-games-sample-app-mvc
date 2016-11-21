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
    public class AthletesController : ApiController
    {
        private OlympicsEntities db;

        public AthletesController()
        {
            this.db = new OlympicsEntities();
        }

        public AthletesController(OlympicsEntities db)
        {
            this.db = db;
        }

        // GET: api/Athletes
        public IQueryable<athlete> GetAthletes()
        {
            return db.athletes;
        }

        // GET: api/Athletes/5
        [ResponseType(typeof(athlete))]
        public IHttpActionResult GetAthlete(int id)
        {
            athlete athlete = db.athletes.Single(x => x.id == id);
            if (athlete == null)
            {
                return NotFound();
            }

            return Ok(athlete);
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