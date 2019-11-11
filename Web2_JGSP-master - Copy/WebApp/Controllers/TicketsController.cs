using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Models;
using WebApp.Persistence;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers
{
    [RoutePrefix("Api/ticket")]
    public class TicketsController : ApiController
    {
      
            private IUnitOfWork db;

            public TicketsController(IUnitOfWork db)
            {
                this.db = db;
            }

        // GET: api/Tickets
        [HttpGet]
        [Route("AllTicket")]
        [AllowAnonymous]
        public IEnumerable<BaseTicket> GetTickets()
        {
             return db.Tickets.GetAll();
        }

        // GET: api/Tickets/5
        [HttpGet]
        [AllowAnonymous]
        [Route("GetById")]
        [ResponseType(typeof(BaseTicket))]
        public IHttpActionResult GetTicket(int id)
        {
             BaseTicket ticket = db.Tickets.Get(id);
             if (ticket == null)
             {
                 return NotFound();
             }

             return Ok(ticket);
        }

        // PUT: api/Tickets/5
        [HttpPut]
        [Route("UpdateTicket")]
        [ResponseType(typeof(void))]
            public IHttpActionResult PutTicket(int id, BaseTicket ticket)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != ticket.Id)
                {
                    return BadRequest();
                }

                db.Tickets.Update(ticket);

                try
                {
                    db.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(id))
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

        // POST: api/Tickets
        [HttpPost]
        [Authorize(Roles = "AppUser")]
        [Route("InsertTicket")]
        [ResponseType(typeof(bool))]
        //buy ticket, day, month, year
        public IHttpActionResult PostTicket([FromBody]Ticket ticket)
        {
            TicketType timeTicket = db.TicketTypes.Find(x => x.Id == ticket.TicketType.Id).FirstOrDefault();
            
            int id = Convert.ToInt32(ticket.Passenger.Id);
            Passenger passenger = db.Passengers.Find(x => x.PassengerType.Id == id).FirstOrDefault();
            Ticket ticket2 = new Ticket(timeTicket, passenger, true, (DateTime.Now).ToString());
            ticket2.IdTypeOfUser = ticket.TicketType.Id;
            ticket2.IdTicketType = timeTicket.Id;
            ticket2.PayPalId = ticket.PayPalId;
            db.Tickets.Add(ticket2);
            db.Complete();

            return Ok(true);
        }


        // POST: api/Tickets
        [Route("InsertTimeTicket")]
        [ResponseType(typeof(bool))]
        [AllowAnonymous]
        public IHttpActionResult PostTimeTicket(Email email)
        {

            TicketType timeTicket = db.TicketTypes.Find(x => x.Name == "Vremenska").FirstOrDefault();
            Ticket ticket = new Ticket(timeTicket, null, true, (DateTime.Now).ToString());
            ticket.IdTypeOfUser = 1;
            ticket.PayPalId = email.PayPalId;
            ticket.IdTicketType = timeTicket.Id;
            db.Tickets.Add(ticket);
            db.Complete();

            DateTime validTo = DateTime.Now.AddHours(1);

            try
            {
                SendEmail(email.Value, "GSP Service, kupovina karte.", $"Postovani, Vasa karta traje sat vremena i istice {validTo}.\n  Hvala na koriscenju usluga");
                return Ok(true);
            }
            catch (Exception)
            {
                return Ok(false);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("SavePayPalInfo")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult SavePayPalInfo([FromBody]PayPal ticket)
        {
            try
            {
                //try to save data in db
                db.PayPals.Add(ticket);
                db.Complete();
                return Ok(true);
            }
            catch (Exception) {

                return BadRequest("Could not add the paypal info!");
            }
        }

        private void SendEmail(string recipient, string subject, string body)
        {
            {
                if (String.IsNullOrEmpty(recipient))
                    return;
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add(recipient);
                    mail.From = new MailAddress("bajicbjelena@gmail.com");
                    mail.Subject = "Kupovi karata";

                    mail.Body = body;

                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
                    smtp.Credentials = new System.Net.NetworkCredential
                         ("emali", "pass"); // ***use valid credentials***
                    smtp.Port = 587;

                    //Or your Smtp Email ID and Password
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in sendEmail:" + ex.Message);
                }
            }
        }

        [Authorize(Roles = "Controller")]
        [Route("CheckValidation")]
        [ResponseType(typeof(Ticket))]
        public string GetTicketForValidation(int Id)
        {
            DateTime dateTime = new DateTime();
            ApplicationDbContext context = new ApplicationDbContext();
            //Time ticket
            BaseTicket baseTicket = db.Tickets.Get(Id);
            var all_ticket = context.BaseTickets.OfType<Ticket>();
            Ticket ticket = all_ticket.Where(o => o.Id == Id).SingleOrDefault();
           
            DateTime timeIssuedDt = new DateTime(); 
            DateTime checkInDt = new DateTime();
            bool timeCard = false;
            //If is id of one-hour ticket
        
                timeIssuedDt = Convert.ToDateTime(ticket.TimeIssued);
                // CheckIn date have only one-hour card
                checkInDt = Convert.ToDateTime(ticket.CheckIn);
            

            //Set this message if ticket is not founf
            string result = "Ticket not found!";
            if (ticket == null)
            {
                return result;
            }

                
            
            //Day
            if (ticket != null)
            {
                if (ticket.IdTypeOfUser == 2)
                {
                    DateTime ti = Convert.ToDateTime(ticket.TimeIssued);
                    int value = DateTime.Compare(ti, DateTime.Now.AddDays(1));
                    if (value < 0 || value == 0)
                    {
                        result = "Valid ticket!";
                    }
                    else
                    {
                        result = "Day has expired. Invalid";
                    }
                    //Month
                }
                else if (ticket.IdTypeOfUser == 3)
                {
                    dateTime = DateTime.Now;

                    if (Convert.ToDateTime(ticket.TimeIssued).Month == dateTime.Month && Convert.ToDateTime(ticket.TimeIssued).Year == dateTime.Year)
                    {
                        result = "Valid ticket";
                    }
                    else
                    {
                        result = "Month has expired. Invalid";
                    }

                    //Check year ticket 
                }
                else if (ticket.IdTypeOfUser == 4)
                {
                    dateTime = DateTime.Now;

                    if (Convert.ToDateTime(ticket.TimeIssued).Year == dateTime.Year)
                    {
                        result = "Valid ticket";
                    }
                    else
                    {
                        result = "Year has expired. Invalid";
                    }
                }
                else
                {
                    //Check validation for one-hour ticket - baseticket class
                    if (ticket.CheckIn == null)
                    {
                        result = "Not checked in yet. Invalid.";

                    }
                    else if (checkInDt > timeIssuedDt)
                    {
                        dateTime = checkInDt.AddHours(1);

                        if (DateTime.Now > dateTime)
                        {
                            result = "1 hour has expired. Invalid.";
                        }
                        else
                        {
                            result = "Valid ticket!";
                        }
                    }

                }
            }
            return result;
        }

        [Authorize(Roles = "AppUser")]
        [Route("CheckInTicket")]
        [HttpPut]
        public IHttpActionResult CheckInTicket([FromBody]Ticket t)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            BaseTicket ticket = context.BaseTickets.SingleOrDefault(o => o.Id == t.Id);
            if (ticket == null)
            {
                return NotFound();
            }
            ticket.CheckIn = DateTime.Now.ToString();
            
            db.Tickets.Update(ticket);
            try
            {
                db.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(t.Id))
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

        [HttpDelete]
        [Route("DeleteTicket")]
        // DELETE: api/Tickets/5
        [ResponseType(typeof(BaseTicket))]
            public IHttpActionResult DeleteTicket(int id)
            {
                BaseTicket ticket = db.Tickets.Get(id);
                if (ticket == null)
                {
                    return NotFound();
                }

                db.Tickets.Remove(ticket);
                db.Complete();

                return Ok(ticket);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }

            private bool TicketExists(int id)
            {
                return db.Tickets.GetAll().Count(e => e.Id == id) > 0;
            }
        }
}
