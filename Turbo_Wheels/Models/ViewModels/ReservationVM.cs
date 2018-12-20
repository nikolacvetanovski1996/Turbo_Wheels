using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Turbo_Wheels.Models
{
    public class ReservationVM
    {
        public string pickupPlace { get; set; }
        public DateTime pickupDateTime { get; set; }
        public string returnPlace { get; set; }
        public DateTime returnDateTime { get; set; }
    }
}