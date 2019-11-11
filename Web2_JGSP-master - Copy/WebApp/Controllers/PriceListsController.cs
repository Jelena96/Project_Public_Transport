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
    [RoutePrefix("Api/PriceList")]
    public class PriceListsController : ApiController
    {
        private IUnitOfWork db;

        public PriceListsController(IUnitOfWork db)
        {
            this.db = db;
        }
        // GET: api/PriceLists
        [ResponseType(typeof(PriceList))]
        [AllowAnonymous]
        [Route("GetAllPL")]
        public IEnumerable<PriceList> GetPriceLists()
        {
            return db.PriceLists.GetAll();
        }
        
        [AllowAnonymous]
        [Route("GetPriceList")]
        public IEnumerable<PriceList> GetPriceList(string card, string pass)
        {
            TicketType tp = db.TicketTypes.Find(e=>e.Name == card).FirstOrDefault();
            PassengerType pt = db.PassengerTypes.Find(e => e.Name == pass).FirstOrDefault();

            return db.PriceLists.GetAll().Where(e=>e.PassengerTypeId == pt.Id &&
            e.TicketTypeId == tp.Id);
        }
        // GET: api/PriceLists/5
        [ResponseType(typeof(PriceList))]
        public IHttpActionResult GetPriceList(int id)
        {
            PriceList priceList = db.PriceLists.Get(id);
            if (priceList == null)
            {
                return NotFound();
            }

            return Ok(priceList);
        }

        // PUT: api/PriceLists/5
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(void))]
        [Route("UpdatePriceList")]
        public IHttpActionResult PutPriceList([FromBody]PriceList priceList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

          
            db.PriceLists.Update(priceList);

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

        // POST: api/PriceLists
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(PriceList))]
        [Route("InsertPriceList")]
        public IHttpActionResult PostPriceList(PriceList priceList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Find all oldest priceLists
            //DateTime date = Convert.ToDateTime(priceList.To);
            PriceList forDelete = new PriceList();
            bool obrisi = false;
            //Remove all oldest 
            foreach (var old in db.PriceLists.GetAll())
            {
                //For To time
                DateTime date1 = DateTime.ParseExact(priceList.To, "dd/MM/yyyy", null);
                DateTime date2 = DateTime.ParseExact(old.To, "dd/MM/yyyy", null);
                int result = DateTime.Compare(date1, date2);

                //For From time
                DateTime date_f_1 = DateTime.ParseExact(priceList.From, "dd/MM/yyyy", null);
                DateTime date_f_2 = DateTime.ParseExact(old.From, "dd/MM/yyyy", null);
                int result_f = DateTime.Compare(date_f_1, date_f_2);

                if ((priceList.TicketTypeId.Equals(old.TicketTypeId)) && (result > 0) && 
                    (result_f > 0) && (priceList.PassengerTypeId.Equals(old.PassengerTypeId)))
                    {
                        obrisi = true;
                        forDelete = old;
                    }
            }

            if (obrisi)
            {
                //If you added pricelist with later datetime for From and To, set false to currentvalid for old pricelist
                forDelete.CurrentValid = false;
                db.PriceLists.Update(forDelete);
            }

            //Add new price list
            db.PriceLists.Add(priceList);
            db.Complete();

            return CreatedAtRoute("DefaultApi", new { controller = "priceList", id = priceList.Id }, priceList);
        }

        // DELETE: api/PriceLists/5
        [ResponseType(typeof(PriceList))]
        [Route("DeletePriceList")]
        public IHttpActionResult DeletePriceList(int id)
        {
            PriceList priceList = db.PriceLists.Get(id);
            if (priceList == null)
            {
                return NotFound();
            }

            db.PriceLists.Remove(priceList);
            db.Complete();

            return Ok(priceList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PriceListExists(int id)
        {
            return db.PriceLists.GetAll().Count(e => e.Id == id) > 0;
        }
    }
}