using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Persistence.Repository
{
    public class ScheduleTypeRepository : Repository<ScheduleType, int>, IScheduleTypesRepository
    {
        public ScheduleTypeRepository(DbContext context) : base(context)
        {
        }
    }

}