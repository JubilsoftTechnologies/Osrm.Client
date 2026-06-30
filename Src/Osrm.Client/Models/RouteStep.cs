using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Osrm.Client.Models
{

    public class RouteStep
    {
        [JsonPropertyName("distance")]
        public double Distance { get; set; }

        [JsonPropertyName("duration")]
        public double Duration { get; set; }

        [JsonPropertyName("geometry")]
        public string? GeometryStr { get; set; }

        public Location[] Geometry
        {
            get
            {
                var geometry = GeometryStr;
                if (string.IsNullOrEmpty(geometry))
                {
                    return Array.Empty<Location>();
                }

                return OsrmPolylineConverter.Decode(geometry!, 1E5)
                    .ToArray();
            }
        }

        [JsonPropertyName("maneuver")]
        public StepManeuver? Maneuver { get; set; }

        [JsonPropertyName("mode")]
        public string? Mode { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

    }
}