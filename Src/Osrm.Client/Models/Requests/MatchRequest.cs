using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osrm.Client.Models.Requests
{
    public class MatchRequest : BaseRequest
    {
        private const string DefaultGeometries = "polyline";
        private const string DefaultOverview = "simplified";

        public MatchRequest()
        {
            Geometries = DefaultGeometries;
            Overview = DefaultOverview;
            Timestamps = Array.Empty<int>();
        }

        /// <summary>
        /// Return route steps for each route
        /// true, false (default)
        /// </summary>
        public bool Steps { get; set; }

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
        /// Timestamp of the input location.
        /// simplified (default), full, false
        /// </summary>
        public int[] Timestamps { get; set; }

        public override List<Tuple<string, string>> UrlParams
        {
            get
            {
                var urlParams = new List<Tuple<string, string>>(BaseUrlParams);

                urlParams
                    .AddBoolParameter("steps", Steps, false)
                    .AddStringParameter("geometries", Geometries, () => Geometries != DefaultGeometries)
                    .AddStringParameter("overview", Overview, () => Overview != DefaultOverview)
                    .AddParams("timestamps", Timestamps.Select(x => x.ToString()).ToArray());

                return urlParams;
            }
        }

        internal override void Validate()
        {
            base.Validate();
            ValidateOptionalParameterLength(Timestamps, nameof(Timestamps));
        }
    }
}