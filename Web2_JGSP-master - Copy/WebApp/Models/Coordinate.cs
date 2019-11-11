using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Coordinate
    {
        [Required]
        public double X { get; set; }

        [Required]
        public double Y { get; set; }
    }
}