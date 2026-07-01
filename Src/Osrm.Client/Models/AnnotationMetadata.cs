using System;
using System.Text.Json.Serialization;

namespace Osrm.Client.Models
{
    public class AnnotationMetadata
    {
        [JsonPropertyName("datasource_names")]
        public string[] DatasourceNames { get; set; } = Array.Empty<string>();
    }
}
