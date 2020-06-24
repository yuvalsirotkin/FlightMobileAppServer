using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightControlApp.Models
{
    public class Command
    {

        [JsonProperty("aileron")]
        [JsonPropertyName("aileron")]
        public Double Aileron { get; set; } = -2;

        [JsonProperty("rudder")]
        [JsonPropertyName("rudder")]
        public Double Rudder { get; set; } = -2;

        [JsonProperty("elevator")]
        [JsonPropertyName("elevator")]
        public Double Elevator { get; set; } = -2;

        [JsonProperty("throttle")]
        [JsonPropertyName("throttle")]
        public Double Throttle { get; set; } = -2;
    }
}
