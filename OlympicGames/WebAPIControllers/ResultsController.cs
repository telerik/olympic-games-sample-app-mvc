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

        // GET: api/Results/5
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
    }
}