using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Ticket : BaseTicket
    {
        public TicketType TicketType { get; set; }
        public Passenger Passenger { get; set; }
        public int IdTypeOfUser { get; set; }

        public Ticket()
        {
            //this.TicketType = new TicketType();
            //this.Passenger = new Passenger();
        }
        public Ticket(TicketType tt,Passenger p, bool iv, string ti)
        {
            TicketType = tt;
            Passenger = p;
            IsValid = iv;
            TimeIssued = ti;
        }
           
    }
}