using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class TicketType
    {
        public int Id { get; set; }
        [Required]
        [StringLength(15)]
        public string Name { get; set; }
        [Required]
        [Range(0,15000)]
        public int Price { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }



    }
}