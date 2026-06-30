using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Osrm.Client.Models
{

    public class RouteLeg
    {
        [JsonPropertyName("distance")]
        public double Distance { get; set; }

        [JsonPropertyName("duration")]
        public double Duration { get; set; }

        [JsonPropertyName("steps")]
        public RouteStep[] Steps { get; set; } = Array.Empty<RouteStep>();

        [JsonPropertyName("summary")]
        public string Summary { get; set; } = string.Empty;
    
        [JsonPropertyName("weight")]
        public double Weight { get; set; }
    }
}