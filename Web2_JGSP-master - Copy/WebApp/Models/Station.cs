using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;

namespace WebApp.Models
{
    public class Station
    {
        public int Id { get; set; }
        public Coordinate Coordinate { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        [StringLength(40)]
        public string Address { get; set; }
        [JsonIgnore]
        public List<Line> Lines { get; set; }

        //for transaction
        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}