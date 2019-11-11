using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Persistence.Repository
{

    public class PassengerRepository : Repository<Passenger, int>, IPassengerRepository
    {
        public PassengerRepository(DbContext context) : base(context)
        {
        }

    }

   
}