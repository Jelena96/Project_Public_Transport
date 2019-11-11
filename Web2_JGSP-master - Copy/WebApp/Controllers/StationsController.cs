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
using WebApp.Models;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers
{
    [RoutePrefix("Api/Stations")]

    public class StationsController : ApiController
    {
        public IUnitOfWork db { get; set; }

        public StationsController(IUnitOfWork db)
        {
            this.db = db;
        }
        // GET: api/Stations
        
        [Route("AllStations")]
        public IEnumerable<Station> GetStations()
        {
            return db.Stations.GetAll();
        }

        // GET: api/Stations/5
        [ResponseType(typeof(Station))]
        public IHttpActionResult GetStation(int id)
        {
            Station station = db.Stations.Get(id);
            if (station == null)
            {
                return NotFound();
            }

            return Ok(station);
        }

        // PUT: api/Stations/5
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Admin")]
        [Route("UpdateStation")]
        public IHttpActionResult PutStation([FromBody]Station station)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

          
            db.Stations.Update(station);

            

            int result = 0;
            result = db.Complete();
            if (result == 0)
            {
                return Conflict();

            }
            else if (result == -1)
            {
                return BadRequest("Podatak je menjan u medjuvremenu, molimo pokusajte ponovo.");
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Stations
        [ResponseType(typeof(Station))]
        [Authorize(Roles = "Admin")]
        [Route("InsertStation")]
        public IHttpActionResult PostStation([FromBody]Station station)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Stations.Add(station);
            db.Complete();


            return CreatedAtRoute("DefaultApi", new { controller = "station", id = station.Id }, station);
        }

        // DELETE: api/Stations/5
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(Station))]
        [Route("DeleteStation")]
        public IHttpActionResult DeleteStation(int id)
        {
            Station station = db.Stations.Get(id);
            if (station == null)
            {
                return NotFound();
            }

            db.Stations.Remove(station);
            db.Complete();

            return Ok(station);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StationExists(int id)
        {
            return db.Stations.GetAll().Count(e => e.Id == id) > 0;
        }
    }
}