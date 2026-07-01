using Osrm.Client.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osrm.Client
{
    internal static class OsrmRequestBuilder
    {
        public static string GetUrl(string server, string service, string version, string profile, string coordinatesString, List<Tuple<string, string>> urlParams)
        {
            if (string.IsNullOrWhiteSpace(server))
            {
                throw new ArgumentException("An OSRM server URL is required.", nameof(server));
            }

            var uriBuilder = new UriBuilder(server);
            uriBuilder.Path += service + "/" + version + "/" + profile + "/" + coordinatesString;
            var url = uriBuilder.Uri.ToString();

            string result = url;
            if (urlParams != null
                && urlParams.Count > 0)
            {
                var encodedParams = urlParams
                    .Select(x => string.Format("{0}={1}", Uri.EscapeDataString(x.Item1), Uri.EscapeDataString(x.Item2)))
                    .ToList();

                result += "?" + string.Join("&", encodedParams);
            }

            return result;
        }

        public static List<Tuple<string, string>> AddBoolParameter(this List<Tuple<string, string>> urlParams, string urlKey, bool param, bool defaultValue)
        {
            if (param != defaultValue)
            {
                urlParams.Add(new Tuple<string, string>(urlKey, param ? "true" : "false"));
            }

            return urlParams;
        }

        public static List<Tuple<string, string>> AddStringParameter(this List<Tuple<string, string>> urlParams, string urlKey, string? value, Func<bool>? condition = null)
        {
            if (!string.IsNullOrEmpty(value) && (condition == null || condition()))
            {
                urlParams.Add(new Tuple<string, string>(urlKey, value!));
            }

            return urlParams;
        }

        public static List<Tuple<string, string>> AddParams(this List<Tuple<string, string>> urlParams, string urlKey, string[]? values, string? defaultIfEmpty = null)
        {
            if (values != null && values.Length > 0)
            {
                urlParams.Add(new Tuple<string, string>(urlKey, string.Join(";", values)));
            }
            else if (!string.IsNullOrEmpty(defaultIfEmpty))
            {
                urlParams.Add(new Tuple<string, string>(urlKey, defaultIfEmpty!));
            }

            return urlParams;
        }

        public static List<Tuple<string, string>> AddCsvParameter(this List<Tuple<string, string>> urlParams, string urlKey, string[]? values)
        {
            if (values != null && values.Length > 0)
            {
                urlParams.Add(new Tuple<string, string>(urlKey, string.Join(",", values)));
            }

            return urlParams;
        }
    }
}