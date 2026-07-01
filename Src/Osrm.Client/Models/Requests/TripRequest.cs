using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osrm.Client.Models.Requests
{
    public class TripRequest : BaseRequest, Osrm.Client.IHasGeometryFormat
    {
        private const string DefaultGeometries = "polyline";
        private const string DefaultOverview = "simplified";
        private const string DefaultSource = "any";
        private const string DefaultDestination = "any";

        public TripRequest()
        {
            Geometries = DefaultGeometries;
            Overview = DefaultOverview;
            Roundtrip = true;
            Source = DefaultSource;
            Destination = DefaultDestination;
        }

        /// <summary>
        /// Return route instructions for each trip
        /// </summary>
        public bool Steps { get; set; }

        /// <summary>
        /// Returns additional metadata for each coordinate along the route geometry.
        /// </summary>
        public bool Annotate { get; set; }

        /// <summary>
        /// Returns additional metadata for each coordinate along the route geometry.
        /// true, false, nodes, distance, duration, datasources, weight, speed
        /// </summary>
        public string? Annotations { get; set; }

        /// <summary>
        /// Returned route is a roundtrip.
        /// true (default), false
        /// </summary>
        public bool Roundtrip { get; set; }

        /// <summary>
        /// Controls whether the route starts at any coordinate or the first coordinate.
        /// any (default), first
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Controls whether the route ends at any coordinate or the last coordinate.
        /// any (default), last
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Returned route geometry format (influences overview and per step)
        /// polyline (default), geojson
        /// </summary>
        public string Geometries { get; set; }

        /// <summary>
        /// Add overview geometry either full, simplified according to highest zoom level it could be display on, or not at all.
        /// simplified (default), full, false
        /// </summary>
        public string Overview { get; set; }

        public override List<Tuple<string, string>> UrlParams
        {
            get
            {
                var urlParams = new List<Tuple<string, string>>(BaseUrlParams);

                urlParams
                    .AddBoolParameter("roundtrip", Roundtrip, true)
                    .AddStringParameter("source", Source, () => Source != DefaultSource)
                    .AddStringParameter("destination", Destination, () => Destination != DefaultDestination)
                    .AddBoolParameter("steps", Steps, false)
                    .AddStringParameter("annotations", Annotations ?? (Annotate ? "true" : null))
                    .AddStringParameter("geometries", Geometries, () => Geometries != DefaultGeometries)
                    .AddStringParameter("overview", Overview, () => Overview != DefaultOverview);

                return urlParams;
            }
        }

        internal override void Validate()
        {
            ValidateCoordinateCount(2);
            ValidateOptionalParameterLengths();

            if (!string.Equals(Source, DefaultSource, StringComparison.Ordinal)
                && !string.Equals(Source, "first", StringComparison.Ordinal))
            {
                throw new ArgumentException("Source must be either 'any' or 'first'.", nameof(Source));
            }

            if (!string.Equals(Destination, DefaultDestination, StringComparison.Ordinal)
                && !string.Equals(Destination, "last", StringComparison.Ordinal))
            {
                throw new ArgumentException("Destination must be either 'any' or 'last'.", nameof(Destination));
            }
        }
    }
}