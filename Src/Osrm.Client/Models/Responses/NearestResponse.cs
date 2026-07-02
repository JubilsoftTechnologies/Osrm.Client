using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Osrm.Client.Models.Responses
{

    public class NearestResponse : BaseResponse
    {
        /// <summary>
        /// Array of Waypoint objects representing all waypoints in order:
        /// </summary>
        [JsonPropertyName("waypoints")]
        public Waypoint[] Waypoints { get; set; } = Array.Empty<Waypoint>();
    }
}