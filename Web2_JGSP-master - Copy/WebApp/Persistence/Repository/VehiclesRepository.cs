using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Persistence.Repository
{

    public class VehiclesRepository : Repository<Vehicles, int>, IVehicleRepository
    {
        public VehiclesRepository(DbContext context) : base(context)
        {
        }
    }
  
}