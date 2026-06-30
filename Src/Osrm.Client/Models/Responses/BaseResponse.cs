using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Osrm.Client.Models.Responses
{

    public abstract class BaseResponse
    {
        /// <summary>
        /// The status code. 200 means successful, 207 means no route was found.
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// (optional) can either be Found route between points or Cannot find route between points
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}