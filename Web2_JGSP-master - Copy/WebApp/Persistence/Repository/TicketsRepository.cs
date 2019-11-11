using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Persistence.Repository
{
    public class TicketsRepository : Repository<BaseTicket, int>, ITicketsRepository
    {
        public TicketsRepository(DbContext context) : base(context)
        {
        }
    }

  
}