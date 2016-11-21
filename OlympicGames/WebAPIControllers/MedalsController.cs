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
    public class MedalsController : ApiController
    {
        private OlympicsEntities db;

        public MedalsController()
        {
            this.db = new OlympicsEntities();
        }

        public MedalsController(OlympicsEntities db)
        {
            this.db = db;
        }

        // GET: api/Medals
        public IQueryable<medal> GetMedals()
        {
            return db.medals;
        }

        // GET: api/Medals/5
        [ResponseType(typeof(medal))]
        public IHttpActionResult GetMedal(int id)
        {
            medal medal = db.medals.Single(x => x.id == id);
            if (medal == null)
            {
                return NotFound();
            }

            return Ok(medal);
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