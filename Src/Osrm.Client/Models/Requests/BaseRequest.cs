using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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

        internal virtual void Validate()
        {
            ValidateCoordinateCount(1);
            ValidateOptionalParameterLengths();
        }

        protected void ValidateCoordinateCount(int minimumCount, int? maximumCount = null)
        {
            if (Coordinates is null)
            {
                throw new ArgumentNullException(nameof(Coordinates), "Coordinates are required.");
            }

            if (Coordinates.Any(c => c is null))
            {
                throw new ArgumentException("Coordinates cannot contain null values.", nameof(Coordinates));
            }

            if (Coordinates.Length < minimumCount)
            {
                var message = minimumCount == 1
                    ? "At least one coordinate is required."
                    : $"At least {minimumCount} coordinates are required.";
                throw new ArgumentException(message, nameof(Coordinates));
            }

            if (maximumCount.HasValue && Coordinates.Length > maximumCount.Value)
            {
                throw new ArgumentException($"No more than {maximumCount.Value} coordinates are allowed.", nameof(Coordinates));
            }
        }

        protected void ValidateOptionalParameterLengths()
        {
            ValidateOptionalParameterLength(Bearings, nameof(Bearings));
            ValidateOptionalParameterLength(Radiuses, nameof(Radiuses));
            ValidateOptionalParameterLength(Hints, nameof(Hints));
        }

        protected void ValidateOptionalParameterLength<T>(T[] values, string parameterName)
        {
            if (values is null)
            {
                throw new ArgumentNullException(parameterName, $"{parameterName} cannot be null.");
            }

            if (values.Length != 0 && values.Length != Coordinates.Length)
            {
                throw new ArgumentException(
                    $"{parameterName} must either be empty or contain exactly one value per coordinate.",
                    parameterName);
            }
        }

        protected void ValidateCoordinateIndexes(uint[] indexes, string parameterName)
        {
            if (indexes is null)
            {
                throw new ArgumentNullException(parameterName, $"{parameterName} cannot be null.");
            }

            foreach (var index in indexes)
            {
                if (index >= Coordinates.Length)
                {
                    throw new ArgumentOutOfRangeException(
                        parameterName,
                        index,
                        $"{parameterName} contains index {index}, but there are only {Coordinates.Length} coordinates.");
                }
            }
        }
    }
}