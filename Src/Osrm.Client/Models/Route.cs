using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Osrm.Client.Models
{
    public class Route
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

        [JsonPropertyName("legs")]
        public RouteLeg[] Legs { get; set; } = Array.Empty<RouteLeg>();

        /// <summary>
        /// Match. Confidence of the matching. float value between 0 and 1. 1 is very confident that the matching is correct.
        /// </summary>
        [JsonPropertyName("confidence")]
        public float? Confidence { get; set; }
    }
}