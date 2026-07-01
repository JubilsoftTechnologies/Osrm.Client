using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osrm.Client.Models.Requests
{
    public class RouteRequest : BaseRequest, Osrm.Client.IHasGeometryFormat
    {
        private const string DefaultGeometries = "polyline";
        private const string DefaultOverview = "simplified";
        private const string DefaultContinueStraight = "default";

        public RouteRequest()
        {
            Geometries = DefaultGeometries;
            Overview = DefaultOverview;
            ContinueStraight = DefaultContinueStraight;
            Waypoints = Array.Empty<uint>();
        }

        /// <summary>
        /// Search for alternative routes and return as well.*
        /// true, false (default)
        /// </summary>
        public bool Alternative { get; set; }

        /// <summary>
        /// Searches for up to the provided number of alternative routes.
        /// </summary>
        public int? AlternativeCount { get; set; }

        /// <summary>
        /// Return route steps for each route leg
        /// true, false (default)
        /// </summary>
        public bool Steps { get; set; }

        /// <summary>
        /// Returns additional metadata for each coordinate along the route geometry.
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
        /// Forces the route to keep going straight at waypoints and don't do a uturn even if it would be faster. Default value depends on the profile.
        /// default (default), true, false
        /// </summary>
        public string ContinueStraight { get; set; }

        /// <summary>
        /// Treats the provided input indices as waypoints in the returned route.
        /// </summary>
        public uint[] Waypoints { get; set; }

        public override List<Tuple<string, string>> UrlParams
        {
            get
            {
                var urlParams = new List<Tuple<string, string>>(BaseUrlParams);

                if (AlternativeCount.HasValue)
                {
                    urlParams.AddStringParameter("alternatives", AlternativeCount.Value.ToString());
                }
                else
                {
                    urlParams.AddBoolParameter("alternatives", Alternative, false);
                }

                urlParams
                    .AddBoolParameter("steps", Steps, false)
                    .AddStringParameter("annotations", Annotations)
                    .AddStringParameter("geometries", Geometries, () => Geometries != DefaultGeometries)
                    .AddStringParameter("overview", Overview, () => Overview != DefaultOverview)
                    .AddStringParameter("continue_straight", ContinueStraight, () => ContinueStraight != DefaultContinueStraight)
                    .AddParams("waypoints", Waypoints.Select(x => x.ToString()).ToArray());

                return urlParams;
            }
        }

        internal override void Validate()
        {
            ValidateCoordinateCount(2);
            ValidateOptionalParameterLengths();
            ValidateCoordinateIndexes(Waypoints, nameof(Waypoints));

            if (AlternativeCount.HasValue && AlternativeCount.Value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(AlternativeCount), AlternativeCount.Value, "AlternativeCount must be greater than zero.");
            }
        }
    }
}