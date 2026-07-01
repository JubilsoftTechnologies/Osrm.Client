using Microsoft.VisualStudio.TestTools.UnitTesting;
using Osrm.Client.Models;
using Osrm.Client.Models.Requests;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Osrm.Client.Tests
{
    [TestClass]
    public class ResponseTests
    {
        private static readonly Location[] RouteLocations =
        {
            new(52.503033, 13.420526),
            new(52.516582, 13.429290),
        };

        [TestMethod]
        public async Task Route_Response()
        {
            var osrm = CreateOsrmClient(
                """
                {
                  "code": "Ok",
                  "waypoints": [
                    {
                      "distance": 1.2,
                      "hint": "hint-a",
                      "location": [13.420526, 52.503033],
                      "name": "Start"
                    },
                    {
                      "distance": 1.8,
                      "hint": "hint-b",
                      "location": [13.429290, 52.516582],
                      "name": "Finish"
                    }
                  ],
                  "routes": [
                    {
                      "distance": 123.4,
                      "duration": 56.7,
                      "geometry": "_p~iF~ps|U_ulLnnqC_mqNvxq`@",
                      "legs": [
                        {
                          "distance": 123.4,
                          "duration": 56.7,
                          "summary": "Sample leg",
                          "weight": 56.7,
                          "steps": [
                            {
                              "distance": 123.4,
                              "duration": 56.7,
                              "geometry": "_p~iF~ps|U_ulLnnqC_mqNvxq`@",
                              "maneuver": {
                                "bearing_after": 90,
                                "bearing_before": 0,
                                "location": [13.420526, 52.503033],
                                "type": "turn",
                                "modifier": "right"
                              },
                              "mode": "driving",
                              "name": "Sample street"
                            }
                          ]
                        }
                      ]
                    }
                  ]
                }
                """);

            var result = await osrm.Route(RouteLocations);

            Assert.AreEqual("Ok", result.Code);
            Assert.IsTrue(result.Routes.Length > 0);
            Assert.IsTrue(result.Waypoints.Length > 0);
            Assert.IsTrue(result.Routes[0].Legs.Length > 0);
            Assert.AreEqual(52.503033, result.Waypoints[0].Location!.Latitude, 0.000001);
            Assert.AreEqual(13.420526, result.Waypoints[0].Location!.Longitude, 0.000001);
            Assert.AreEqual(52.503033, result.Routes[0].Legs[0].Steps[0].Maneuver!.Location!.Latitude, 0.000001);
            Assert.AreEqual(13.420526, result.Routes[0].Legs[0].Steps[0].Maneuver!.Location!.Longitude, 0.000001);
        }

        [TestMethod]
        public async Task Table_Response()
        {
            var osrm = CreateOsrmClient(
                """
                {
                  "code": "Ok",
                  "durations": [
                    [0.0, 12.3],
                    [12.3, 0.0]
                  ],
                  "sources": [
                    { "distance": 0.0, "location": [13.160621, 52.554070], "name": "A" },
                    { "distance": 0.0, "location": [13.720654, 52.431272], "name": "B" }
                  ],
                  "destinations": [
                    { "distance": 0.0, "location": [13.160621, 52.554070], "name": "A" },
                    { "distance": 0.0, "location": [13.720654, 52.431272], "name": "B" }
                  ]
                }
                """);

            var result = await osrm.Table(
                new TableRequest
                {
                    Coordinates =
                    [
                        new Location(52.554070, 13.160621),
                        new Location(52.431272, 13.720654),
                    ],
                });

            Assert.AreEqual("Ok", result.Code);
            Assert.AreEqual(2, result.Durations.Length);
            Assert.AreEqual(2, result.Durations[0].Length);
            Assert.AreEqual(52.554070, result.Sources[0].Location!.Latitude, 0.000001);
        }

        [TestMethod]
        public async Task Match_Response()
        {
            var osrm = CreateOsrmClient(
                """
                {
                  "code": "Ok",
                  "tracepoints": [
                    { "distance": 1.0, "location": [13.393252, 52.542648], "name": "Trace A", "matchings_index": 0, "waypoint_index": 0 },
                    null
                  ],
                  "matchings": [
                    {
                      "distance": 10.0,
                      "duration": 20.0,
                      "geometry": "_p~iF~ps|U_ulLnnqC_mqNvxq`@",
                      "confidence": 0.9,
                      "legs": [
                        {
                          "distance": 10.0,
                          "duration": 20.0,
                          "summary": "Matched leg",
                          "weight": 20.0,
                          "steps": []
                        }
                      ]
                    }
                  ]
                }
                """);

            var result = await osrm.Match(
                new MatchRequest
                {
                    Coordinates =
                    [
                        new Location(52.542648, 13.393252),
                        new Location(52.543079, 13.394780),
                        new Location(52.542107, 13.397389),
                    ],
                    Timestamps = [1424684612, 1424684616, 1424684620],
                });

            Assert.AreEqual("Ok", result.Code);
            Assert.IsTrue(result.Matchings.Length > 0);
            Assert.IsTrue(result.Matchings[0].Legs.Length > 0);
            Assert.IsNotNull(result.Matchings[0].Confidence);
            Assert.IsNull(result.Tracepoints[1]);
        }

        [TestMethod]
        public async Task Nearest_Response()
        {
            var osrm = CreateOsrmClient(
                """
                {
                  "code": "Ok",
                  "waypoints": [
                    {
                      "distance": 0.1,
                      "hint": "nearest-hint",
                      "location": [13.333086, 52.4224],
                      "name": "Nearest point"
                    }
                  ]
                }
                """);

            var result = await osrm.Nearest(new Location(52.4224, 13.333086));

            Assert.AreEqual("Ok", result.Code);
            Assert.IsNotNull(result.Waypoints);
            Assert.AreEqual(52.4224, result.Waypoints[0].Location!.Latitude, 0.000001);
        }

        [TestMethod]
        public async Task Trip_Response()
        {
            var osrm = CreateOsrmClient(
                """
                {
                  "code": "Ok",
                  "waypoints": [
                    { "distance": 0.1, "location": [13.420526, 52.503033], "name": "A", "trips_index": 0, "waypoint_index": 0 },
                    { "distance": 0.1, "location": [13.429290, 52.516582], "name": "B", "trips_index": 0, "waypoint_index": 1 }
                  ],
                  "trips": [
                    {
                      "distance": 12.0,
                      "duration": 34.0,
                      "geometry": "_p~iF~ps|U_ulLnnqC_mqNvxq`@",
                      "legs": [
                        {
                          "distance": 12.0,
                          "duration": 34.0,
                          "summary": "Trip leg",
                          "weight": 34.0,
                          "steps": []
                        }
                      ]
                    }
                  ]
                }
                """);

            var result = await osrm.Trip(RouteLocations);

            Assert.AreEqual("Ok", result.Code);
            Assert.AreEqual(1, result.Trips.Length);
            Assert.IsTrue(result.Trips[0].Legs.Length > 0);
        }

        [TestMethod]
        public async Task Route_Response_Throws_For_NonSuccess_Status()
        {
            var osrm = CreateOsrmClient(
                """
                {
                  "code": "InvalidOptions",
                  "message": "Invalid request"
                }
                """,
                HttpStatusCode.BadRequest);

            var ex = await CaptureExceptionAsync<HttpRequestException>(() => osrm.Route(RouteLocations));

            StringAssert.Contains(ex.Message, "400");
            StringAssert.Contains(ex.Message, "route");
        }

        [TestMethod]
        public async Task Route_Response_Throws_For_Too_Few_Coordinates()
        {
            var osrm = CreateOsrmClient("""{"code":"Ok","waypoints":[],"routes":[]}""");
            var ex = await CaptureExceptionAsync<ArgumentException>(() => osrm.Route(new RouteRequest()));

            Assert.AreEqual("Coordinates", ex.ParamName);
            StringAssert.Contains(ex.Message, "At least 2 coordinates");
        }

        [TestMethod]
        public async Task Nearest_Response_Throws_For_Multiple_Coordinates()
        {
            var osrm = CreateOsrmClient("""{"code":"Ok","waypoints":[]}""");
            var ex = await CaptureExceptionAsync<ArgumentException>(() => osrm.Nearest(RouteLocations));

            Assert.AreEqual("Coordinates", ex.ParamName);
            StringAssert.Contains(ex.Message, "No more than 1 coordinates");
        }

        [TestMethod]
        public async Task Match_Response_Throws_For_Mismatched_Timestamps()
        {
            var osrm = CreateOsrmClient("""{"code":"Ok","tracepoints":[],"matchings":[]}""");
            var ex = await CaptureExceptionAsync<ArgumentException>(() => osrm.Match(
                new MatchRequest
                {
                    Coordinates = RouteLocations,
                    Timestamps = [1],
                }));

            Assert.AreEqual("Timestamps", ex.ParamName);
            StringAssert.Contains(ex.Message, "exactly one value per coordinate");
        }

        [TestMethod]
        public async Task Table_Response_Throws_For_Invalid_Source_Index()
        {
            var osrm = CreateOsrmClient("""{"code":"Ok","durations":[],"sources":[],"destinations":[]}""");
            var ex = await CaptureExceptionAsync<ArgumentOutOfRangeException>(() => osrm.Table(
                new TableRequest
                {
                    Coordinates = RouteLocations,
                    Sources = [2],
                }));

            Assert.AreEqual("Sources", ex.ParamName);
            StringAssert.Contains(ex.Message, "there are only 2 coordinates");
        }

        [TestMethod]
        public async Task Route_Response_Throws_When_Timeout_Is_Exceeded()
        {
            var osrm = CreateOsrmClient(
                """{"code":"Ok","waypoints":[],"routes":[]}""",
                responseDelay: TimeSpan.FromMilliseconds(200));
            osrm.Timeout = 10;

            var ex = await CaptureExceptionAsync<TimeoutException>(() => osrm.Route(RouteLocations));

            StringAssert.Contains(ex.Message, "10 ms");
        }

        private static async Task<TException> CaptureExceptionAsync<TException>(Func<Task> action)
            where TException : Exception
        {
            try
            {
                await action();
            }
            catch (TException ex)
            {
                return ex;
            }

            Assert.Fail($"Expected a {typeof(TException).Name}.");
            return null!;
        }

        private static Osrm5x CreateOsrmClient(
            string responseBody,
            HttpStatusCode statusCode = HttpStatusCode.OK,
            TimeSpan? responseDelay = null)
        {
            var httpClient = new HttpClient(new StubHttpMessageHandler(responseBody, statusCode, responseDelay));
            return new Osrm5x(httpClient, "https://router.project-osrm.org/");
        }

        private sealed class StubHttpMessageHandler : HttpMessageHandler
        {
            private readonly string responseBody;
            private readonly HttpStatusCode statusCode;
            private readonly TimeSpan responseDelay;

            public StubHttpMessageHandler(string responseBody, HttpStatusCode statusCode, TimeSpan? responseDelay)
            {
                this.responseBody = responseBody;
                this.statusCode = statusCode;
                this.responseDelay = responseDelay ?? TimeSpan.Zero;
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (responseDelay > TimeSpan.Zero)
                {
                    await Task.Delay(responseDelay, cancellationToken);
                }

                var response = new HttpResponseMessage(statusCode)
                {
                    RequestMessage = request,
                    Content = new StringContent(responseBody, Encoding.UTF8, "application/json"),
                };

                return response;
            }
        }
    }
}