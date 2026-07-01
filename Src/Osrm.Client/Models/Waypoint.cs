using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Osrm.Client.Models
{
    public class Waypoint
    {
        [JsonPropertyName("distance")]
        public double Distance { get; set; }

        /// <summary>
        /// Unique internal identifier of the segment (ephemeral, not constant over data updates) This can be used on subsequent request to significantly speed up the query and to connect multiple services. E.g. you can use the hint value obtained by the nearest query as hint values for route inputs.
        /// </summary>
        [JsonPropertyName("hint")]
        public string? Hint { get; set; }

        [JsonPropertyName("location")]
        public double[]? LocationArr { get; set; }

        public Location? Location
        {
            get
            {
                if (LocationArr == null)
                {
                    return null;
                }

                return new Location(LocationArr[1], LocationArr[0]);
            }
        }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("nodes")]
        public ulong[] Nodes { get; set; } = Array.Empty<ulong>();

        /// <summary>
        /// Match. Index to the Route object in matchings the sub-trace was matched to.
        /// </summary>
        [JsonPropertyName("matchings_index")]
        public int? MatchingsIndex { get; set; }

        [JsonPropertyName("trips_index")]
        public int? TripsIndex { get; set; }

        /// <summary>
        /// Match. Index of the waypoint inside the matched route.
        /// </summary>
        [JsonPropertyName("waypoint_index")]
        public int? WaypointIndex { get; set; }

        [JsonPropertyName("alternatives_count")]
        public int? AlternativesCount { get; set; }
    }
}