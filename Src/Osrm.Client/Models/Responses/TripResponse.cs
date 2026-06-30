using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Osrm.Client.Models.Responses
{

    public class TripResponse : BaseResponse
    {
        /// <summary>
        /// Array of Waypoint objects representing all waypoints in input order. Each Waypoint object has the following additional properties:
        /// </summary>
        [JsonPropertyName("waypoints")]
        public Waypoint[] Waypoints { get; set; } = Array.Empty<Waypoint>();

        /// <summary>
        /// An array of Route objects that assemble the trace.
        /// </summary>
        [JsonPropertyName("trips")]
        public Route[] Trips { get; set; } = Array.Empty<Route>();
    }
}