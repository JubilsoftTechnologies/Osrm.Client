using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osrm.Client.Models.Requests
{
    public class TripRequest : BaseRequest
    {
        private const string DefaultGeometries = "polyline";
        private const string DefaultOverview = "simplified";

        public TripRequest()
        {
            Geometries = DefaultGeometries;
            Overview = DefaultOverview;
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
                    .AddBoolParameter("steps", Steps, false)
                    .AddBoolParameter("annotate", Annotate, false)
                    .AddStringParameter("geometries", Geometries, () => Geometries != DefaultGeometries)
                    .AddStringParameter("overview", Overview, () => Overview != DefaultOverview);

                return urlParams;
            }
        }

        internal override void Validate()
        {
            ValidateCoordinateCount(2);
            ValidateOptionalParameterLengths();
        }
    }
}