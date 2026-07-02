using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Osrm.Client.Models
{
    public class RouteStep : Osrm.Client.IGeometryFormatAware
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

        [JsonPropertyName("weight")]
        public double Weight { get; set; }

        [JsonPropertyName("maneuver")]
        public StepManeuver? Maneuver { get; set; }

        [JsonPropertyName("ref")]
        public string? Ref { get; set; }

        [JsonPropertyName("pronunciation")]
        public string? Pronunciation { get; set; }

        [JsonPropertyName("destinations")]
        public string? Destinations { get; set; }

        [JsonPropertyName("exits")]
        public string? Exits { get; set; }

        [JsonPropertyName("mode")]
        public string? Mode { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("intersections")]
        public Intersection[] Intersections { get; set; } = Array.Empty<Intersection>();

        [JsonPropertyName("rotary_name")]
        public string? RotaryName { get; set; }

        [JsonPropertyName("rotary_pronunciation")]
        public string? RotaryPronunciation { get; set; }

        [JsonPropertyName("driving_side")]
        public string? DrivingSide { get; set; }

        void Osrm.Client.IGeometryFormatAware.SetGeometryFormat(string geometryFormat)
        {
            this.geometryFormat = geometryFormat;
        }

    }
}