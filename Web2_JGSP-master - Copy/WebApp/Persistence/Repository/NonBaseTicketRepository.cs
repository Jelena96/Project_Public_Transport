using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Persistence.Repository
{
    public class NonBaseTicketRepository : Repository<Ticket, int>, INonBaseTicketRepository
    {
        public NonBaseTicketRepository(DbContext context) : base(context)
        {
        }
    
    }
}