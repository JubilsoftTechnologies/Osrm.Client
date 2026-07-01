using System;
using System.Text.Json.Serialization;

namespace Osrm.Client.Models
{
    public class RouteLeg : Osrm.Client.IGeometryFormatAware
    {
        [JsonPropertyName("distance")]
        public double Distance { get; set; }

        [JsonPropertyName("duration")]
        public double Duration { get; set; }

        [JsonPropertyName("steps")]
        public RouteStep[] Steps { get; set; } = Array.Empty<RouteStep>();

        [JsonPropertyName("summary")]
        public string Summary { get; set; } = string.Empty;
    
        [JsonPropertyName("weight")]
        public double Weight { get; set; }

        [JsonPropertyName("annotation")]
        public Annotation? Annotation { get; set; }

        void Osrm.Client.IGeometryFormatAware.SetGeometryFormat(string geometryFormat)
        {
            foreach (var step in Steps)
            {
                ((Osrm.Client.IGeometryFormatAware)step).SetGeometryFormat(geometryFormat);
            }
        }
    }
}