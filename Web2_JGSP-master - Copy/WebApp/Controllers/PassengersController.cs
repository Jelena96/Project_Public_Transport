using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using WebApp.Models;
using WebApp.Persistence;
using WebApp.Persistence.Repository;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers
{
    [RoutePrefix("Api/Passengers")]
    public class PassengersController : ApiController
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public IUnitOfWork db { get; set; }

        public PassengersController(IUnitOfWork db)
        {
            this.db = db;
        }

        // GET: api/Passengers
        public IEnumerable<Passenger> GetUsers()
        {
            return db.Passengers.GetAll();
        }

        // GET: api/Passengers/5
        [ResponseType(typeof(Passenger))]
        [Route("GetById/{id}")]
        public IHttpActionResult GetPassenger(int id)
        {
            Passenger passenger = db.Passengers.Get(id);
            if (passenger == null)
            {
                return NotFound();
            }

            return Ok(passenger);
        }

        // PUT: api/Passengers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPassenger(string id, Passenger passenger)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != passenger.Id)
            {
                return BadRequest();
            }

            db.Passengers.Update(passenger);

            try
            {
                db.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PassengerExists(id))
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

        // POST: api/Passengers
        [AllowAnonymous]
        [ResponseType(typeof(Passenger))]
        [Route("InsertPassenger")]

        public IHttpActionResult PostPassenger([FromBody]Passenger passenger)
        {
         string imgUrl = "";

            ApplicationDbContext context = new ApplicationDbContext();
           

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var user = new Passenger() { UserName = passenger.Email, Email = passenger.Email, PasswordHash = ApplicationUser.HashPassword(passenger.Password), Password = ApplicationUser.HashPassword(passenger.Password), FirstName = passenger.FirstName, LastName = passenger.LastName, BirthDate = passenger.BirthDate, Address = passenger.Address, Approved = passenger.Approved, Role = "AppUser"};
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            Passenger user = new Passenger();


            if (passenger.Role == "AppUser")
            {
                user.UserName = passenger.Email;
                user.Email = passenger.Email;
                user.PasswordHash = ApplicationUser.HashPassword(passenger.Password);
                user.Password = ApplicationUser.HashPassword(passenger.Password);
                user.FirstName = passenger.FirstName;
                user.LastName = passenger.LastName;

                user.BirthDate = passenger.BirthDate;
                user.Address = passenger.Address;
                user.TypeOfPassenger = passenger.PassengerType.Name;
                user.Role = "AppUser";
                user.ImageUrl = passenger.ImageUrl;
               
                // Set verification status
                if (passenger.PassengerType.Id == 3)
                {
                   user.VerificationStatus = "";
                }
                else
                {

                    if (user.ImageUrl == "" || user.ImageUrl == null)
                        user.VerificationStatus = "Invalid";
                    else
                        user.VerificationStatus = "In Progress";

                    imgUrl = MakePath(user);
                    user.ImageUrl = imgUrl;
                   

                }
                user.PassengerType = context.PassengerTypes.SingleOrDefault(o => o.Name == passenger.PassengerType.Name);
                userManager.Create(user);
                userManager.AddToRole(user.Id, "AppUser");
                SendEmail(user.Email, $"Your varification status of profile is : " + user.VerificationStatus + ".");
            }


            try
            {
                db.Complete();
            }
            catch (DbUpdateException)
            {
                if (PassengerExists(passenger.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            //passenger.PasswordHash = passenger.Password.GetHashCode();

            return CreatedAtRoute("DefaultApi", new { controller = "passengers", id = user.Id }, user);
        }

        //Send mail for passenger verification
        private void SendEmail(string recipient, string body)
        {
            {
                if (String.IsNullOrEmpty(recipient))
                    return;
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add(recipient);
                    mail.From = new MailAddress("bajicbjelena@gmail.com");
                    mail.Subject = "Status verifikacije";

                    mail.Body = body;

                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
                    smtp.Credentials = new System.Net.NetworkCredential
                         ("email", "pass"); // ***use valid credentials***
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

        //Save user photo or document, convert to byte array
        public string MakePath(Passenger user)
        {
            string imgUrl = "";
            if (user.ImageUrl != "" && user.ImageUrl != null)
            {
                byte[] imageBytes = Convert.FromBase64String(user.ImageUrl);
                // Convert byte[] to Image
                using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    Image image = Image.FromStream(ms, true);
                    // image = resizeImage(image, new Size(500, 500));
                    try
                    {
                        image.Save(@"C:\Users\Jelena\Desktop\slike 18.07.2019" + user.Email + ".jpg");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    imgUrl = @"C:\Users\Jelena\Desktop\slike 18.07.2019" + user.Email + ".jpg";
                }
            }

            return imgUrl;

        }
        // DELETE: api/Passengers/5
        [ResponseType(typeof(Passenger))]
        public IHttpActionResult DeletePassenger(int id)
        {
            Passenger passenger = db.Passengers.Get(id);
            if (passenger == null)
            {
                return NotFound();
            }

            db.Passengers.Remove(passenger);
            db.Complete();

            return Ok(passenger);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PassengerExists(string id)
        {
            return db.Passengers.GetAll().Count(e => e.Id == id) > 0;
        }
    }
}