using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using WebApp.Models;

namespace WebApp.Persistence
{
    
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //public DbSet<ApplicationUser> AppUser { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<BaseTicket> BaseTickets { get; set; }
        public DbSet<PassengerType> PassengerTypes { get; set; }
        public DbSet<PriceList> PriceLists { get; set; }
        public DbSet<ScheduleType> ScheduleTypes { get; set; }
        public DbSet<TicketType> TicketType { get; set; }
        public DbSet<Vehicles> Vehicless { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<PayPal> PayPals { get; set; }


        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Types<Ticket>().Configure(ctc => ctc.Property(cust => cust.TicketType.Id).HasColumnName("Ticket_type_id"));
        //}
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}