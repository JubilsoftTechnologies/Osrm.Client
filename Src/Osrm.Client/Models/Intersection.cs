using System;
using System.Text.Json.Serialization;

namespace Osrm.Client.Models
{
    public class Intersection
    {
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

        [JsonPropertyName("bearings")]
        public int[] Bearings { get; set; } = Array.Empty<int>();

        [JsonPropertyName("classes")]
        public string[] Classes { get; set; } = Array.Empty<string>();

        [JsonPropertyName("entry")]
        public bool[] Entry { get; set; } = Array.Empty<bool>();

        [JsonPropertyName("in")]
        public int? In { get; set; }

        [JsonPropertyName("out")]
        public int? Out { get; set; }

        [JsonPropertyName("lanes")]
        public Lane[] Lanes { get; set; } = Array.Empty<Lane>();
    }
}
