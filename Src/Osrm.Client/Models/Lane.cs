using System;
using System.Text.Json.Serialization;

namespace Osrm.Client.Models
{
    public class Lane
    {
        [JsonPropertyName("indications")]
        public string[] Indications { get; set; } = Array.Empty<string>();

        [JsonPropertyName("valid")]
        public bool Valid { get; set; }
    }
}
