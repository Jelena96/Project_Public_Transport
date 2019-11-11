using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Passenger : ApplicationUser
    {
        public string TypeOfPassenger { get; set; }
        public PassengerType PassengerType { get; set; }

        [Required]
        public string Password { get; set; }


    }
}