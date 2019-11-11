using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.UI;
using WebApp.Models;
using WebApp.Persistence;
using WebApp.Persistence.Repository;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers
{
    [RoutePrefix("Api/Schedules")]
    public class SchedulesController : ApiController
    {

        public IUnitOfWork _unitOfWork;

        public SchedulesController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(Schedule))]
        [Route("AdminSchedules")]
        // GET: api/Schedules
        public IEnumerable<Schedule> GetSchedules()
        {
            
            return _unitOfWork.Schedules.GetAll();

        }

        // GET: api/Schedules/5
        [ResponseType(typeof(Schedule))]
        public IHttpActionResult GetSchedule(int id)
        {
            Schedule schedule = _unitOfWork.Schedules.Get(id);
            if (schedule == null)
            {
                return NotFound();
            }

            return Ok(schedule);
        }

        // PUT: api/Schedules/5
        [ResponseType(typeof(void))]
        [Route("PutSchedule/{id}")]
        //[Authorize(Roles = "Admin")]
        public IHttpActionResult PutSchedule(int id, [FromBody] Schedule schedule)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            int result = 0;
            schedule.Line = new Line();
            schedule.Line.Id = schedule.LineId;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _unitOfWork.Schedules.Update(schedule);

            //Logic for transaction administrators action
            result = _unitOfWork.Complete();
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

        // POST: api/Schedules
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(Schedule))]
        [Route("InsertSchedule")]
        public IHttpActionResult PostSchedule([FromBody] Schedule schedule)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
                _unitOfWork.Schedules.Add(schedule);
            int vr = _unitOfWork.Complete();
             
            
            return CreatedAtRoute("DefaultApi", new { controller = "schedules", id = schedule.Id }, schedule);
        }

        // DELETE: api/Schedules/5
        [ResponseType(typeof(Schedule))]
        
        [Route("DeleteSchedule")]
        public IHttpActionResult DeleteSchedule(int id)
        {
            Schedule schedule = _unitOfWork.Schedules.Get(id);
            if (schedule == null)
            {
                return NotFound();
            }

            _unitOfWork.Schedules.Remove(schedule);
            _unitOfWork.Complete();
            


            return Ok(schedule);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ScheduleExists(int id)
        {
            return _unitOfWork.Schedules.GetAll().Count(e => e.Id == id) > 0;
        }
    }
}