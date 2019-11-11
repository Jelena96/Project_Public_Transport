using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class PayPal
    {
        public int Id { get; set; }
        public string Cart { get; set; }
        public string CreateTime { get; set; }
        public string PayPalId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PayerId { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public string State { get; set; }
        public string Currency { get; set; }
        public string Total { get; set; }

    }
}