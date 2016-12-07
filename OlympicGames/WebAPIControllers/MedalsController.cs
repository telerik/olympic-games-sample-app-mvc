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

        [ResponseType(typeof(List<AthleteMedals>))]
        [Route("api/Medals/GetTopSportsAthletes")]
        public IHttpActionResult GetTopSportsAthletes([FromUri]int sportId, [FromUri]int[] athleteIds)
        {
            var athletesWithSportId = db.athletes.ToList().Join(db.results, a => a.id, r => r.athlete, (a, r) => new { AthleteID = a.id, AthleteName = a.firstName + ' ' + a.lastName, SportID = r.@event });
            var athletesWithSportCategoryId = athletesWithSportId
                .Join(db.sports, a => a.SportID, s => s.id, (a, s) => new { AthleteID = a.AthleteID, AthleteName = a.AthleteName, SportCategoryId = s.category })
                .Where(a => a.SportCategoryId == sportId);
            var athletes = athletesWithSportCategoryId.GroupBy(a => new { a.AthleteID, a.AthleteName },
             (key, group) => new AthleteMedals
             {
                 AthleteId = key.AthleteID,
                 Name = key.AthleteName,
                 MedalsCount = group.Count()
             }).OrderByDescending(a => a.MedalsCount).Take(10)
             .Where(x => athleteIds.Length == 0 || athleteIds.Contains(x.AthleteId)).ToList();

            return Ok(athletes);
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