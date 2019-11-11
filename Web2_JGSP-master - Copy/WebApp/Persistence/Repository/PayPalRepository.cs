using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Persistence.Repository
{
    public class PayPalRepository : Repository<PayPal, int>, IPayPalRepository
    {
        public PayPalRepository(DbContext context) : base(context)
        {
        }
    }
}