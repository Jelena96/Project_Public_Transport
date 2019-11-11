using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Persistence.Repository
{
    public class TicketTypesRepository : Repository<TicketType, int>, ITicketTypeRepository
    {
        public TicketTypesRepository(DbContext context) : base(context)
        {
        }
    }

}