using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Osrm.Client.Models
{

    public class StepManeuver
    {
        /// <summary>
        /// The clockwise angle from true north to the direction of travel immediately after the maneuver.
        /// </summary>
        [JsonPropertyName("bearing_after")]
        public int BearingAfter { get; set; }

        /// <summary>
        /// The clockwise angle from true north to the direction of travel immediately before the maneuver.
        /// </summary>
        [JsonPropertyName("bearing_before")]
        public int BearingBefore { get; set; }

        /// <summary>
        /// An optional integer indicating number of the exit to take. The field exists for the following type field:
        /// </summary>
        [JsonPropertyName("exit")]
        public int Exit { get; set; }

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

        /// <summary>
        /// A string indicating the type of maneuver. new identifiers might be introduced without API change Types unknown to the client should be handled like the turn type, the existance of correct modifier values is guranteed.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// An optional string indicating the direction change of the maneuver.
        /// </summary>
        [JsonPropertyName("modifier")]
        public string? Modifier { get; set; }
    }
}