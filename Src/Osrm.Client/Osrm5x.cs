﻿using System.Text.Json;
using Osrm.Client.Models;
using Osrm.Client.Models.Requests;
using Osrm.Client.Models.Responses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Osrm.Client
{
    public class Osrm5x : IOsrmClient
    {
        /// <summary>
        /// Modern HTTP client implementation
        /// </summary>
        private readonly HttpClient Client;

        /// <summary>
        /// Url of OSRM server
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Version of the protocol implemented by the service.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Mode of transportation, is determined by the profile that is used to prepare the data
        /// </summary>
        public string Profile { get; set; }

        /// <summary>
        /// Timeout for web request. If not specified default value will be used.
        /// </summary>
        public int? Timeout { get; set; }

        protected readonly string RouteServiceName = "route";
        protected readonly string NearestServiceName = "nearest";
        protected readonly string TableServiceName = "table";
        protected readonly string MatchServiceName = "match";
        protected readonly string TripServiceName = "trip";
        protected readonly string TileServiceName = "tile";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <param name="version"></param>
        /// <param name="profile"></param>
        public Osrm5x(HttpClient client, string url = "", string version = "v1", string profile = "driving")
        {
            Client = client;
            Url = url;
            Version = version;
            Profile = profile;
        }

        /// <summary>
        /// Given an array of geopoints, find the shortest path between each in given order
        /// This service provides shortest path queries with multiple via locations.
        /// It supports the computation of alternative paths as well as giving turn instructions.
        /// https://github.com/Project-OSRM/osrm-backend/blob/master/docs/http.md#route-service
        /// </summary>
        /// <param name="locs"></param>
        /// <returns></returns>
        public async Task<RouteResponse> Route(Location[] locs)
        {
            return await Route(new RouteRequest()
            {
                Coordinates = locs
            });
        }

        /// <summary>
        /// Given an array of geopoints, find the shortest path between each in given order
        /// This service provides shortest path queries with multiple via locations.
        /// It supports the computation of alternative paths as well as giving turn instructions.
        /// https://github.com/Project-OSRM/osrm-backend/blob/master/docs/http.md#route-service
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        public async Task<RouteResponse> Route(RouteRequest requestParams)
        {
            return await Send<RouteResponse>(RouteServiceName, requestParams);
        }

        /// <summary>
        /// Snaps a coordinate to the street network and returns the nearest n matches.
        /// https://github.com/Project-OSRM/osrm-backend/blob/master/docs/http.md#nearest-service
        /// </summary>
        /// <param name="locs"></param>
        /// <returns></returns>
        public async Task<NearestResponse> Nearest(params Location[] locs)
        {
            return await Nearest(new NearestRequest()
            {
                Coordinates = locs
            });
        }

        /// <summary>
        /// Snaps a coordinate to the street network and returns the nearest n matches.
        /// https://github.com/Project-OSRM/osrm-backend/blob/master/docs/http.md#nearest-service
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        public async Task<NearestResponse> Nearest(NearestRequest requestParams)
        {
            return await Send<NearestResponse>(NearestServiceName, requestParams);
        }

        /// <summary>
        /// Given an array of geopoints, find the fastest path through all points.
        /// Computes the duration of the fastest route between all pairs of supplied coordinates. 
        /// Returns the durations or distances or both between the coordinate pairs. Note that the 
        /// distances are not the shortest distance between two coordinates, but rather the distances 
        /// of the fastest routes. Duration is in seconds and distances is in meters.
        /// https://github.com/Project-OSRM/osrm-backend/blob/master/docs/http.md#table-service
        /// </summary>
        /// <param name="locs"></param>
        /// <returns></returns>
        public async Task<TableResponse> Table(params Location[] locs)
        {
            return await Table(new TableRequest()
            {
                Coordinates = locs
            });
        }

        /// <summary>
        /// Given an array of geopoints, find the fastest path through all points.
        /// Computes the duration of the fastest route between all pairs of supplied coordinates. 
        /// Returns the durations or distances or both between the coordinate pairs. Note that the 
        /// distances are not the shortest distance between two coordinates, but rather the distances 
        /// of the fastest routes. Duration is in seconds and distances is in meters.
        /// https://github.com/Project-OSRM/osrm-backend/blob/master/docs/http.md#table-service
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        public async Task<TableResponse> Table(TableRequest requestParams)
        {
            return await Send<TableResponse>(TableServiceName, requestParams);
        }

        /// <summary>
        /// Map matching matches/snaps given GPS points to the road network in the most plausible 
        /// way. Please note the request might result multiple sub-traces. Large jumps in the 
        /// timestamps (> 60s) or improbable transitions lead to trace splits if a complete matching 
        /// could not be found. The algorithm might not be able to match all points. Outliers are 
        /// removed if they can not be matched successfully.
        /// https://github.com/Project-OSRM/osrm-backend/blob/master/docs/http.md#match-service
        /// </summary>
        /// <param name="locs"></param>
        /// <returns></returns>
        public async Task<MatchResponse> Match(params Location[] locs)
        {
            return await Match(new MatchRequest()
            {
                Coordinates = locs
            });
        }

        /// <summary>
        /// Map matching matches/snaps given GPS points to the road network in the most plausible 
        /// way. Please note the request might result multiple sub-traces. Large jumps in the 
        /// timestamps (> 60s) or improbable transitions lead to trace splits if a complete matching 
        /// could not be found. The algorithm might not be able to match all points. Outliers are 
        /// removed if they can not be matched successfully.
        /// https://github.com/Project-OSRM/osrm-backend/blob/master/docs/http.md#match-service
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        public async Task<MatchResponse> Match(MatchRequest requestParams)
        {
            return await Send<MatchResponse>(MatchServiceName, requestParams);
        }

        /// <summary>
        /// The trip plugin solves the Traveling Salesman Problem  (shortest round-trip 
        /// between all points) using a greedy heuristic (farthest-insertion algorithm) 
        /// for 10 or more waypoints and uses brute force for less than 10 waypoints. 
        /// The returned path does not have to be the fastest path. As TSP is NP-hard it 
        /// only returns an approximation. Note that all input coordinates have to be 
        /// connected for the trip service to work.
        /// https://github.com/Project-OSRM/osrm-backend/blob/master/docs/http.md#trip-service
        /// </summary>
        /// <param name="locs"></param>
        /// <returns></returns>
        public async Task<TripResponse> Trip(params Location[] locs)
        {
            return await Trip(new TripRequest()
            {
                Coordinates = locs
            });
        }

        /// <summary>
        /// The trip plugin solves the Traveling Salesman Problem  (shortest round-trip 
        /// between all points) using a greedy heuristic (farthest-insertion algorithm) 
        /// for 10 or more waypoints and uses brute force for less than 10 waypoints. 
        /// The returned path does not have to be the fastest path. As TSP is NP-hard it 
        /// only returns an approximation. Note that all input coordinates have to be 
        /// connected for the trip service to work.
        /// https://github.com/Project-OSRM/osrm-backend/blob/master/docs/http.md#trip-service
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        public async Task<TripResponse> Trip(TripRequest requestParams)
        {
            return await Send<TripResponse>(TripServiceName, requestParams);
        }

        protected async Task<T> Send<T>(string service, BaseRequest request) //string coordinatesStr, List<Tuple<string, string>> urlParams)
        {
            var coordinatesStr = request.CoordinatesUrlPart;
            List<Tuple<string, string>> urlParams = request.UrlParams;
            var fullUrl = OsrmRequestBuilder.GetUrl(Url, service, Version, Profile, coordinatesStr, urlParams);

            try
            {
                string responseBody = await Client.GetStringAsync(fullUrl);

                return await Task.FromResult(JsonSerializer.Deserialize<T>(responseBody));
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                throw;
            }

        }

    }
}