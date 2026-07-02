using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Osrm.Client.Models.Responses
{

    public class TableResponse : BaseResponse
    {
        /// <summary>
        /// array of arrays that stores the matrix in row-major order. durations[i][j] gives the travel time from the i-th waypoint to the j-th waypoint. Values are given in seconds.
        /// </summary>
        [JsonPropertyName("durations")]
        public double?[][] Durations { get; set; } = Array.Empty<double?[]>();

        [JsonPropertyName("distances")]
        public double?[][] Distances { get; set; } = Array.Empty<double?[]>();

        /// <summary>
        /// array of Waypoint objects describing all sources in order
        /// </summary>
        [JsonPropertyName("sources")]
        public Waypoint[] Sources { get; set; } = Array.Empty<Waypoint>();

        /// <summary>
        /// array of Waypoint objects describing all destinations in order
        /// </summary>
        [JsonPropertyName("destinations")]
        public Waypoint[] Destinations { get; set; } = Array.Empty<Waypoint>();

        [JsonPropertyName("fallback_speed_cells")]
        public uint[][] FallbackSpeedCells { get; set; } = Array.Empty<uint[]>();
    }
}