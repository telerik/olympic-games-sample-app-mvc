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
    public class GamesController : ApiController
    {
        private OlympicsEntities db;

        public GamesController()
        {
            this.db = new OlympicsEntities();
        }

        public GamesController(OlympicsEntities db)
        {
            this.db = db;
        }

        // GET: api/games
        public IQueryable<game> GetGames()
        {
            return db.games;
        }

        // GET: api/games/5
        [ResponseType(typeof(game))]
        public IHttpActionResult GetGame(int id)
        {
            game game = db.games.Single(x => x.id == id);
            if (game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        [ResponseType(typeof(short[]))]
        //[Route(Name = "Games")]
        public short[] GetMinMaxYears([FromUri] bool getMinMaxYears)
        {
            short minYear = db.games.Min(x => x.year);
            short maxYear = db.games.Max(x => x.year);
            var result = new short[2] { minYear, maxYear };

            return result;
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