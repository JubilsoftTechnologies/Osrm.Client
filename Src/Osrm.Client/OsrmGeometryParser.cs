using Osrm.Client.Models;
using System;
using System.Linq;
using System.Text.Json;

namespace Osrm.Client
{
    internal static class OsrmGeometryParser
    {
        public static string? GetEncodedGeometry(JsonElement geometryValue)
        {
            return geometryValue.ValueKind == JsonValueKind.String
                ? geometryValue.GetString()
                : null;
        }

        public static Location[] GetLocations(JsonElement geometryValue, string geometryFormat)
        {
            if (geometryValue.ValueKind == JsonValueKind.String)
            {
                var encoded = geometryValue.GetString();
                if (string.IsNullOrEmpty(encoded))
                {
                    return Array.Empty<Location>();
                }

                var factor = string.Equals(geometryFormat, "polyline6", StringComparison.OrdinalIgnoreCase)
                    ? 1E6
                    : 1E5;

                return OsrmPolylineConverter.Decode(encoded!, factor)
                    .ToArray();
            }

            if (geometryValue.ValueKind == JsonValueKind.Object
                && geometryValue.TryGetProperty("coordinates", out var coordinates)
                && coordinates.ValueKind == JsonValueKind.Array)
            {
                return coordinates
                    .EnumerateArray()
                    .Where(c => c.ValueKind == JsonValueKind.Array && c.GetArrayLength() >= 2)
                    .Select(c => new Location(c[1].GetDouble(), c[0].GetDouble()))
                    .ToArray();
            }

            return Array.Empty<Location>();
        }
    }
}
