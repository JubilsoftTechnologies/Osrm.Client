using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osrm.Client.Models.Requests
{
    public abstract class BaseRequest
    {
        public BaseRequest()
        {
            Coordinates = Array.Empty<Location>();
            Bearings = Array.Empty<Bearing>();
            Radiuses = Array.Empty<int>();
            Hints = Array.Empty<string>();
        }

        /// <summary>
        /// Use locs encoded plyline param instead locs
        /// </summary>
        public bool SendCoordinatesAsPolyline { get; set; }

        public Location[] Coordinates { get; set; }

        /// <summary>
        /// Limits the search to segments with given bearing in degrees towards true north in clockwise direction.
        /// integer 0 .. 360,integer 0 .. 180
        /// </summary>
        public Bearing[] Bearings { get; set; }

        /// <summary>
        /// Limits the search to given radius in meters.
        /// double >= 0 or unlimited (default)
        /// </summary>
        public int[] Radiuses { get; set; }

        /// <summary>
        /// Hint to derive position in street network.
        /// Base64 string
        /// </summary>
        public string[] Hints { get; set; }

        public string CoordinatesUrlPart
        {
            get
            {
                if (Coordinates == null)
                {
                    return string.Empty;
                }

                if (SendCoordinatesAsPolyline)
                {
                    var encodedLocs = OsrmPolylineConverter.Encode(Coordinates, 1E5);
                    return "polyline(" + encodedLocs + ")";
                }
                return string.Join(";", Coordinates.Select(x => x.Longitude.ToString("F6", CultureInfo.InvariantCulture)
                        + "," + x.Latitude.ToString("F6", CultureInfo.InvariantCulture)));
            }
        }

        public abstract List<Tuple<string, string>> UrlParams { get; }

        protected List<Tuple<string, string>> BaseUrlParams
        {
            get
            {
                var urlParams = new List<Tuple<string, string>>();

                urlParams
                    .AddParams("bearings", Bearings.Select(x => x.Item1 + "," + x.Item2).ToArray())
                    .AddParams("radiuses", Radiuses.Select(x => x.ToString()).ToArray())
                    .AddParams("hints", Hints);

                return urlParams;
            }
        }
    }
}