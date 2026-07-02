using Microsoft.VisualStudio.TestTools.UnitTesting;
using Osrm.Client.Models;
using Osrm.Client.Models.Requests;

namespace Osrm.Client.Tests
{
    [TestClass]
    public class RequestModelsTests
    {
        [TestMethod]
        public void RouteRequest_Defaults()
        {
            var r = new RouteRequest();
            Assert.AreEqual<bool>(false, r.Alternative);
            Assert.IsNull(r.AlternativeCount);
            Assert.AreEqual<bool>(false, r.Steps);
            Assert.IsNull(r.Annotations);
            Assert.AreEqual<string>("polyline", r.Geometries);
            Assert.AreEqual<string>("simplified", r.Overview);
            Assert.AreEqual<string>("default", r.ContinueStraight);
            Assert.IsNotNull(r.Waypoints);
        }

        [TestMethod]
        public void TableRequest_Defaults()
        {
            var r = new TableRequest();
            Assert.IsNotNull(r.Sources);
            Assert.IsNotNull(r.Destinations);
            Assert.AreEqual<string>("duration", r.Annotations);
            Assert.AreEqual<string>("input", r.FallbackCoordinate);
            Assert.IsNull(r.FallbackSpeed);
            Assert.IsNull(r.ScaleFactor);
        }

        [TestMethod]
        public void MatchRequest_Defaults()
        {
            var r = new MatchRequest();
            Assert.AreEqual<bool>(false, r.Steps);
            Assert.IsNull(r.Annotations);
            Assert.AreEqual<string>("polyline", r.Geometries);
            Assert.AreEqual<string>("simplified", r.Overview);
            Assert.AreEqual<string>("split", r.Gaps);
            Assert.AreEqual<bool>(false, r.Tidy);
            Assert.IsNotNull(r.Timestamps);
            Assert.IsNotNull(r.Waypoints);
        }

        [TestMethod]
        public void NearestRequest_Defaults()
        {
            var r = new NearestRequest();
            Assert.AreEqual<uint>(1, r.Number);
        }

        [TestMethod]
        public void Location_Equals()
        {
            var a1 = new Location(52.542648, 13.393252);
            var a2 = new Location(52.542648, 13.393252);
            Assert.IsTrue(a1 == a2);
            Assert.IsTrue(a1.Equals(a2));

            var b1 = new Location(12.542648, 13.393252);
            var b2 = new Location(12.542648, 13.393252);
            Assert.IsTrue(b1 == b2);
            Assert.IsTrue(b1.Equals(b2));
        }

        [TestMethod]
        public void TripRequest_Defaults()
        {
            var r = new TripRequest();
            Assert.AreEqual<bool>(false, r.Annotate);
            Assert.IsNull(r.Annotations);
            Assert.AreEqual<bool>(false, r.Steps);
            Assert.AreEqual<bool>(true, r.Roundtrip);
            Assert.AreEqual<string>("any", r.Source);
            Assert.AreEqual<string>("any", r.Destination);
            Assert.AreEqual<string>("polyline", r.Geometries);
            Assert.AreEqual<string>("simplified", r.Overview);
        }

        [TestMethod]
        public void BaseRequest_Defaults()
        {
            var r = new TripRequest();

            // test common base request items
            Assert.AreEqual<bool>(false, r.SendCoordinatesAsPolyline);
            Assert.IsNotNull(r.Coordinates);
            Assert.IsNotNull(r.Bearings);
            Assert.IsNotNull(r.Radiuses);
            Assert.IsNotNull(r.Hints);
            Assert.AreEqual<bool>(true, r.GenerateHints);
            Assert.IsNotNull(r.Approaches);
            Assert.IsNotNull(r.Exclude);
            Assert.AreEqual<string>("default", r.Snapping);
            Assert.AreEqual<bool>(false, r.SkipWaypoints);
        }
    }
}