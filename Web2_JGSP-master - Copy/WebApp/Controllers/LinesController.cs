using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using WebApp.Models;
using WebApp.Persistence.Repository;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers
{
    [RoutePrefix("api/Line")]
    public class LinesController : ApiController
    {
        private IUnitOfWork db;

        public LinesController(IUnitOfWork db)
        {
            this.db = db;
        }


        [Route("GetScheduleLines")]
        [AllowAnonymous]
        public IEnumerable<Line> GetScheduleLines(string typeOfLine)
        {
            List<Line> lines = new List<Line>();
            var linijice = db.Lines.GetAll().ToList();

            if (typeOfLine == null)
            {
                var type = db.Schedules.GetAll().FirstOrDefault(u => u.ScheduleTypeId == 1);
                return db.Lines.GetAll().Where(u => u.Id == type.LineId);

            }
            else if (typeOfLine == "Suburban")
            {

                var type = db.Schedules.GetAll().FirstOrDefault(u => u.ScheduleTypeId == 2);
                return db.Lines.GetAll().Where(u => u.Id == type.LineId);

            }
            else
            {
                var type = db.Schedules.GetAll().FirstOrDefault(u => u.ScheduleTypeId == 1);
                return db.Lines.GetAll().Where(u => u.TypeOfLine == type.ScheduleTypeId);
                          
            }
        }

        [Route("GetSchedule")]
        [AllowAnonymous]
        public string GetSchedule(string typeOfLine, string typeOfDay, string Number)
        {
            var line = db.Lines.GetAll().FirstOrDefault(u => u.LineNumber.ToString() == Number);

            var schedules = db.Schedules.GetAll().ToList();
            var lines = db.Lines.GetAll().ToList();
            var scheduletipes = db.ScheduleTypes.GetAll().Where(e=>e.Name==typeOfLine);

            string dep = "";
            foreach (Schedule l in schedules)
            {
                foreach( var item in scheduletipes)
                { 
                    if ((l.Line.Id == line.Id) && (l.DayInWeek == typeOfDay) && (l.ScheduleTypeId == item.Id))
                    {
                        dep += l.Departure+",";

                    }
                }

            }

            if (dep.Length > 0)
                dep = dep.Substring(0, dep.Length - 1);
            return dep;    
        }

        // GET: api/Lines
        [Route("AllLines")]
        [AllowAnonymous]
        public IEnumerable<Line> GetLines()
        {
            return db.Lines.GetAll();
        }

        // GET: api/Lines/5
        [ResponseType(typeof(Line))]
        public IHttpActionResult GetLine(int id)
        {
            Line line = db.Lines.Get(id);
            if (line == null)
            {
                return NotFound();
            }

            return Ok(line);
        }

        // PUT: api/Lines/5
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(void))]
        [Route("UpdateLine/{id}")]
        public IHttpActionResult PutLine(int id,[FromBody] Line line)
        {
            int result = 0;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Lines.Update(line);

            //Logic for transaction administrators action
            result = db.Complete();
            if(result == 0)
            {
                return Conflict();

            }
            else if(result == -1)
            {
                return BadRequest("Podatak je menjan u medjuvremenu, molimo pokusajte ponovo.");
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Lines
        [ResponseType(typeof(Line))]
        [Route("InsertLine")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult PostLine([FromBody]Line line)
        {
            List<Station> stations = new List<Station>();
            foreach (Station s in line.Stations)
            {
                stations.Add(db.Stations.Find(x => x.Id == s.Id).FirstOrDefault());
            }
            line.Stations = stations;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Lines.Add(line);
            db.Complete();
            
            return CreatedAtRoute("DefaultApi", new { controller = "line", id = line.Id }, line);
            
        }

        // DELETE: api/Lines/5
        [ResponseType(typeof(Line))]
        [Authorize(Roles = "Admin")]
        [Route("DeleteLine")]
        public IHttpActionResult DeleteLine(int id)
        {
            Line line = db.Lines.Get(id);
            if (line == null)
            {
                return NotFound();
            }

            db.Lines.Remove(line);
            db.Complete();
         
            return Ok(line);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LineExists(int id)
        {
            return db.Lines.GetAll().Count(e => e.Id == id) > 0;
        }
    }
}