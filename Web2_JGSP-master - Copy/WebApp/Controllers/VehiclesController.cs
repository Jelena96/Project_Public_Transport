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
    public class VehiclesController : ApiController
    {
        public IUnitOfWork db { get; set; }

        public VehiclesController(IUnitOfWork db)
        {
            this.db = db;
        }

        // GET: api/Vehicles
        public IEnumerable<Vehicles> GetVehicles()
        {
            return db.Vehicles.GetAll();
        }

        // GET: api/Vehicles/5
        [ResponseType(typeof(Vehicles))]
        public IHttpActionResult GetVehicles(int id)
        {
            Vehicles vehicles = db.Vehicles.Get(id);
            if (vehicles == null)
            {
                return NotFound();
            }

            return Ok(vehicles);
        }

        // PUT: api/Vehicles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVehicles(int id, Vehicles vehicles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vehicles.Id)
            {
                return BadRequest();
            }

            db.Vehicles.Update(vehicles);

            try
            {
                db.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehiclesExists(id))
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

        // POST: api/Vehicles
        [ResponseType(typeof(Vehicles))]
        public IHttpActionResult PostVehicles(Vehicles vehicles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Vehicles.Add(vehicles);
            db.Complete();

            return CreatedAtRoute("DefaultApi", new { id = vehicles.Id }, vehicles);
        }

        // DELETE: api/Vehicles/5
        [ResponseType(typeof(Vehicles))]
        public IHttpActionResult DeleteVehicles(int id)
        {
            Vehicles vehicles = db.Vehicles.Get(id);
            if (vehicles == null)
            {
                return NotFound();
            }

            db.Vehicles.Remove(vehicles);
            db.Complete();

            return Ok(vehicles);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VehiclesExists(int id)
        {
            return db.Vehicles.GetAll().Count(e => e.Id == id) > 0;
        }
    }
}