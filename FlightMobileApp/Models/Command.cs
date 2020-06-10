using Microsoft.AspNetCore.Routing.Constraints;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightMobileApp.Models
{
    public class Command
    {

        [JsonProperty("aileron")]
        [JsonPropertyName("aileron")]
        public double Aileron { get; set; }

        [JsonProperty("rudder")] 
        [JsonPropertyName("rudder")]
        public double Rudder { get; set; }

        [JsonProperty("elevator")]
        [JsonPropertyName("elevator")]
        public double Elevator { get; set; }

        [JsonProperty("throttle")]
        [JsonPropertyName("throttle")]
        public double Throttle { get; set; }
    }
}
