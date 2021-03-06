﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Osrm.Client.Models.Responses
{

    public class MatchResponse : BaseResponse
    {
        /// <summary>
        /// Array of Ẁaypoint objects representing all points of the trace in order.
        /// If the trace point was ommited by map matching because it is an outlier, the entry will be null.
        /// </summary>
        [JsonPropertyName("tracepoints")]
        public Waypoint[] Tracepoints { get; set; }

        [JsonPropertyName("matchings")]
        public Route[] Matchings { get; set; }
    }
}