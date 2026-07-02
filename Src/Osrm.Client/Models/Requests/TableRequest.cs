using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Osrm.Client.Models.Requests
{
    public class TableRequest : BaseRequest
    {
        private const string DefaultAnnotations = "duration";
        private const string DefaultFallbackCoordinate = "input";

        public TableRequest()
        {
            Sources = Array.Empty<uint>();
            Destinations = Array.Empty<uint>();
            Annotations = DefaultAnnotations;
            FallbackCoordinate = DefaultFallbackCoordinate;
        }

        /// <summary>
        /// Use location with given index as source.
        /// {index};{index}[;{index} ...] or all (default)
        /// </summary>
        public uint[] Sources { get; set; }

        /// <summary>
        /// Use location with given index as destination.
        /// {index};{index}[;{index} ...] or all (default)
        /// </summary>
        public uint[] Destinations { get; set; }

        /// <summary>
        /// Return the requested table or tables in the response.
        /// duration (default), distance, duration,distance
        /// </summary>
        public string Annotations { get; set; }

        /// <summary>
        /// Estimates duration for unreachable cells using as-the-crow-flies distance.
        /// </summary>
        public double? FallbackSpeed { get; set; }

        /// <summary>
        /// Selects which coordinate to use for fallback-speed calculations.
        /// input (default), snapped
        /// </summary>
        public string FallbackCoordinate { get; set; }

        /// <summary>
        /// Scales returned duration values.
        /// </summary>
        public double? ScaleFactor { get; set; }

        public override List<Tuple<string, string>> UrlParams
        {
            get
            {
                var urlParams = new List<Tuple<string, string>>(BaseUrlParams);

                urlParams
                    .AddParams("sources", Sources.Select(x => x.ToString()).ToArray())
                    .AddParams("destinations", Destinations.Select(x => x.ToString()).ToArray())
                    .AddStringParameter("annotations", Annotations, () => Annotations != DefaultAnnotations)
                    .AddStringParameter("fallback_speed", FallbackSpeed?.ToString(CultureInfo.InvariantCulture))
                    .AddStringParameter("fallback_coordinate", FallbackCoordinate, () => FallbackCoordinate != DefaultFallbackCoordinate)
                    .AddStringParameter("scale_factor", ScaleFactor?.ToString(CultureInfo.InvariantCulture));

                return urlParams;
            }
        }

        internal override void Validate()
        {
            base.Validate();
            ValidateCoordinateIndexes(Sources, nameof(Sources));
            ValidateCoordinateIndexes(Destinations, nameof(Destinations));

            if (string.IsNullOrWhiteSpace(Annotations))
            {
                throw new ArgumentException("Annotations cannot be null or empty.", nameof(Annotations));
            }

            if (FallbackSpeed.HasValue && FallbackSpeed.Value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(FallbackSpeed), FallbackSpeed.Value, "FallbackSpeed must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(FallbackCoordinate))
            {
                throw new ArgumentException("FallbackCoordinate cannot be null or empty.", nameof(FallbackCoordinate));
            }

            if (!string.Equals(FallbackCoordinate, DefaultFallbackCoordinate, StringComparison.Ordinal)
                && !string.Equals(FallbackCoordinate, "snapped", StringComparison.Ordinal))
            {
                throw new ArgumentException("FallbackCoordinate must be either 'input' or 'snapped'.", nameof(FallbackCoordinate));
            }

            if (ScaleFactor.HasValue && ScaleFactor.Value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(ScaleFactor), ScaleFactor.Value, "ScaleFactor must be greater than zero.");
            }
        }
    }
}