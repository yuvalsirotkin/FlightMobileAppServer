using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMobileApp.Models
{
    public class Command
    {
        [Key]
        public double Aileron { get; set; }
        public double Rudder { get; set; }
        public double Elevator { get; set; }
        public double Throttle { get; set; }
    }
}
