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
    public class ScheduleTypesController : ApiController
    {
        public IUnitOfWork db { get; set; }

        public ScheduleTypesController(IUnitOfWork db)
        {
            this.db = db;
        }

        // GET: api/ScheduleTypes
        public IEnumerable<ScheduleType> GetScheduleTypes()
        {
            return db.ScheduleTypes.GetAll();
        }

        // GET: api/ScheduleTypes/5
        [ResponseType(typeof(ScheduleType))]
        public IHttpActionResult GetScheduleType(int id)
        {
            ScheduleType scheduleType = db.ScheduleTypes.Get(id);
            if (scheduleType == null)
            {
                return NotFound();
            }

            return Ok(scheduleType);
        }

        // PUT: api/ScheduleTypes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutScheduleType(int id, ScheduleType scheduleType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != scheduleType.Id)
            {
                return BadRequest();
            }

            db.ScheduleTypes.Update(scheduleType);

            try
            {
                db.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScheduleTypeExists(id))
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

        // POST: api/ScheduleTypes
        [ResponseType(typeof(ScheduleType))]
        public IHttpActionResult PostScheduleType(ScheduleType scheduleType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ScheduleTypes.Add(scheduleType);
            db.Complete();

            return CreatedAtRoute("DefaultApi", new { id = scheduleType.Id }, scheduleType);
        }

        // DELETE: api/ScheduleTypes/5
        [ResponseType(typeof(ScheduleType))]
        public IHttpActionResult DeleteScheduleType(int id)
        {
            ScheduleType scheduleType = db.ScheduleTypes.Get(id);
            if (scheduleType == null)
            {
                return NotFound();
            }

            db.ScheduleTypes.Remove(scheduleType);
            db.Complete();

            return Ok(scheduleType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ScheduleTypeExists(int id)
        {
            return db.ScheduleTypes.GetAll().Count(e => e.Id == id) > 0;
        }
    }
}