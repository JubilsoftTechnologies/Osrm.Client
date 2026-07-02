using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osrm.Client.Models.Requests
{
    public class MatchRequest : BaseRequest, Osrm.Client.IHasGeometryFormat
    {
        private const string DefaultGeometries = "polyline";
        private const string DefaultOverview = "simplified";
        private const string DefaultGaps = "split";

        public MatchRequest()
        {
            Geometries = DefaultGeometries;
            Overview = DefaultOverview;
            Timestamps = Array.Empty<int>();
            Gaps = DefaultGaps;
            Waypoints = Array.Empty<uint>();
        }

        /// <summary>
        /// Return route steps for each route
        /// true, false (default)
        /// </summary>
        public bool Steps { get; set; }

        /// <summary>
        /// Returns additional metadata for each coordinate along the matched geometry.
        /// true, false, nodes, distance, duration, datasources, weight, speed
        /// </summary>
        public string? Annotations { get; set; }

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

        /// <summary>
        /// Allows the input track splitting based on huge timestamp gaps between points.
        /// split (default), ignore
        /// </summary>
        public string Gaps { get; set; }

        /// <summary>
        /// Allows input track modification to obtain better matching quality for noisy tracks.
        /// </summary>
        public bool Tidy { get; set; }

        /// <summary>
        /// Timestamp of the input location.
        /// simplified (default), full, false
        /// </summary>
        public int[] Timestamps { get; set; }

        /// <summary>
        /// Treats the provided input indices as waypoints in the returned match object.
        /// </summary>
        public uint[] Waypoints { get; set; }

        public override List<Tuple<string, string>> UrlParams
        {
            get
            {
                var urlParams = new List<Tuple<string, string>>(BaseUrlParams);

                urlParams
                    .AddBoolParameter("steps", Steps, false)
                    .AddStringParameter("annotations", Annotations)
                    .AddStringParameter("geometries", Geometries, () => Geometries != DefaultGeometries)
                    .AddStringParameter("overview", Overview, () => Overview != DefaultOverview)
                    .AddStringParameter("gaps", Gaps, () => Gaps != DefaultGaps)
                    .AddBoolParameter("tidy", Tidy, false)
                    .AddParams("timestamps", Timestamps.Select(x => x.ToString()).ToArray())
                    .AddParams("waypoints", Waypoints.Select(x => x.ToString()).ToArray());

                return urlParams;
            }
        }

        internal override void Validate()
        {
            base.Validate();
            ValidateOptionalParameterLength(Timestamps, nameof(Timestamps));
            ValidateCoordinateIndexes(Waypoints, nameof(Waypoints));

            if (!string.Equals(Gaps, DefaultGaps, StringComparison.Ordinal)
                && !string.Equals(Gaps, "ignore", StringComparison.Ordinal))
            {
                throw new ArgumentException("Gaps must be either 'split' or 'ignore'.", nameof(Gaps));
            }
        }
    }
}