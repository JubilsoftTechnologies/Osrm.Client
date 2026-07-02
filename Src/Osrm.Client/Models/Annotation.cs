using System;
using System.Text.Json.Serialization;

namespace Osrm.Client.Models
{
    public class Annotation
    {
        [JsonPropertyName("distance")]
        public double[] Distance { get; set; } = Array.Empty<double>();

        [JsonPropertyName("duration")]
        public double[] Duration { get; set; } = Array.Empty<double>();

        [JsonPropertyName("datasources")]
        public int[] Datasources { get; set; } = Array.Empty<int>();

        [JsonPropertyName("nodes")]
        public ulong[] Nodes { get; set; } = Array.Empty<ulong>();

        [JsonPropertyName("weight")]
        public double[] Weight { get; set; } = Array.Empty<double>();

        [JsonPropertyName("speed")]
        public double[] Speed { get; set; } = Array.Empty<double>();

        [JsonPropertyName("metadata")]
        public AnnotationMetadata? Metadata { get; set; }
    }
}
