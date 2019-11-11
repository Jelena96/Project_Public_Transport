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
    [RoutePrefix("Api/PassengerType")]
    public class PassengerTypesController : ApiController
    {
        private IUnitOfWork db;

        public PassengerTypesController(IUnitOfWork db)
        {
            this.db = db;
        }

        // GET: api/PassengerTypes
        [ResponseType(typeof(PassengerType))]
        [Route("GetAllPT")]
        [AllowAnonymous]
        public IEnumerable<PassengerType> GetPassengerTypes()
        {
            return db.PassengerTypes.GetAll();
        }

        // GET: api/PassengerTypes/5
        [AllowAnonymous]
        [ResponseType(typeof(PassengerType))]
        [Route("GetById/{id}")]
        public IHttpActionResult GetPassengerType(int id)
        {
            PassengerType passengerType = db.PassengerTypes.Get(id);
            if (passengerType == null)
            {
                return NotFound();
            }

            return Ok(passengerType);
        }

        // PUT: api/PassengerTypes/5
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(void))]
        [Route("PutPassengerType/{id}")]
        public IHttpActionResult PutPassengerType([FromBody]PassengerType passengerType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PassengerTypes.Update(passengerType);

            //Logic for transaction administrators action
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

        // POST: api/PassengerTypes
        [ResponseType(typeof(PassengerType))]
        public IHttpActionResult PostPassengerType(PassengerType passengerType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PassengerTypes.Add(passengerType);
            db.Complete();

            return CreatedAtRoute("DefaultApi", new { id = passengerType.Id }, passengerType);
        }

        // DELETE: api/PassengerTypes/5
        [ResponseType(typeof(PassengerType))]
        public IHttpActionResult DeletePassengerType(int id)
        {
            PassengerType passengerType = db.PassengerTypes.Get(id);
            if (passengerType == null)
            {
                return NotFound();
            }

            db.PassengerTypes.Remove(passengerType);
            db.Complete();

            return Ok(passengerType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PassengerTypeExists(int id)
        {
            return db.PassengerTypes.GetAll().Count(e => e.Id == id) > 0;
        }
    }
}