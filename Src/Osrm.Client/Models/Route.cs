using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Osrm.Client.Models
{
    public class Route : Osrm.Client.IGeometryFormatAware
    {
        private string geometryFormat = "polyline";

        [JsonPropertyName("distance")]
        public double Distance { get; set; }

        [JsonPropertyName("duration")]
        public double Duration { get; set; }

        [JsonPropertyName("geometry")]
        public JsonElement RawGeometry { get; set; }

        [JsonIgnore]
        public string? GeometryStr => Osrm.Client.OsrmGeometryParser.GetEncodedGeometry(RawGeometry);

        [JsonIgnore]
        public Location[] Geometry
        {
            get
            {
                return Osrm.Client.OsrmGeometryParser.GetLocations(RawGeometry, geometryFormat);
            }
        }

        [JsonPropertyName("legs")]
        public RouteLeg[] Legs { get; set; } = Array.Empty<RouteLeg>();

        [JsonPropertyName("weight")]
        public double Weight { get; set; }

        [JsonPropertyName("weight_name")]
        public string? WeightName { get; set; }

        /// <summary>
        /// Match. Confidence of the matching. float value between 0 and 1. 1 is very confident that the matching is correct.
        /// </summary>
        [JsonPropertyName("confidence")]
        public float? Confidence { get; set; }

        void Osrm.Client.IGeometryFormatAware.SetGeometryFormat(string geometryFormat)
        {
            this.geometryFormat = geometryFormat;

            foreach (var leg in Legs)
            {
                ((Osrm.Client.IGeometryFormatAware)leg).SetGeometryFormat(geometryFormat);
            }
        }
    }
}