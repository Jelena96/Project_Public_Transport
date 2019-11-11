using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class PriceList
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        [Required]
        [Range(0, 15000)]
        public float TicketPrice { get; set; }
        public int PassengerTypeId { get; set; }
        public int TicketTypeId { get; set; }
        public bool CurrentValid { get; set; }
        //for transaction
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}