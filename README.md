Osrm.Client
==========
A modern .NET client for the OSRM 5.x HTTP API.

## Target frameworks

- `net9.0`
- `netstandard2.0` for low-maintenance compatibility with older consumers

## Build and test

From the repository root:

```bash
dotnet restore Src/Osrm.Client.sln
dotnet build Src/Osrm.Client.sln
dotnet test Src/Osrm.Client.Tests/Osrm.Client.Tests.csproj
dotnet pack Src/Osrm.Client/Osrm.Client.csproj -c Release
```

The test project is designed to run offline. Response tests use stubbed `HttpClient` responses instead of the public demo server.

Forked from https://github.com/narfunikita/Osrm.Client - version 3.0.0.0

## Changes from forked version:

#### v4.0.0
  - Upgraded the package to modern SDK-style .NET targets with `net9.0` as the primary target.
  - Added a `netstandard2.0` compatibility target without retaining legacy runtime-specific project structure.
  - Replaced network-dependent response tests with deterministic HTTP stubs.
  - Improved package/release metadata and repository automation for build, pack, and publish flows.
  - Corrected OSRM waypoint and maneuver response coordinate mapping to preserve `Location(latitude, longitude)`.

#### v3.5.0.1
  - Minor code clean-ups.

#### v3.5.0.0
  - Unify namespaces (further clean-up of 4x related code).
  - Added IOsrmClient interface for Osrm5x class.
  - Bumped version to get away from ones used by forked project's Nuget release.

#### v3.0.0.1
  - Migrated implementation and tests to .NetCore 3.1.
  - Replaced WebClient with newer HttpClient implementation.
  - OSRM server requests are now handled asynchronously.
  - Replaced dependency on NewtonSoft.json with System.Text.Json.
  - Removed 4x API implementation.
  - Fixed various bugs and omissions.


## Usage Summary: 
  (see also OSRM documentation - http://project-osrm.org/docs/v5.22.0/api)
#### Route
```csharp
using HttpClient client = new HttpClient();
var osrm5x = new Osrm5x(client, "http://router.project-osrm.org/");
var locations = new Location[] {
    new Location(52.503033, 13.420526),
    new Location(52.516582, 13.429290),
};

var result = await osrm.Route(locations);

var result2 = await osrm.Route(new RouteRequest()
{
    Coordinates = locations,
    Steps = true
});
var result3 = await osrm.Route(new RouteRequest()
{
    Coordinates = locations,
    SendCoordinatesAsPolyline = true
});
```

#### Table
```csharp
using HttpClient client = new HttpClient();
var osrm5x = new Osrm5x(client, "http://router.project-osrm.org/");
var locations = new Location[] {
    new Location(52.517037, 13.388860),
    new Location(52.529407, 13.397634),
    new Location(52.523219, 13.428555)
};

//Returns a 3x3 matrix:
var result = await osrm.Table(locations);

//Returns a 1x3 matrix:
var result2 = await osrm.Table(new Osrm.Client.Models.Requests.TableRequest()
{
    Coordinates = locations,
    Sources = new uint[] { 0 }
});

//Returns a asymmetric 3x2 matrix with from the polyline encoded locations qikdcB}~dpXkkHz:
var result3 = await osrm.Table(new Osrm.Client.Models.Requests.TableRequest()
{
    Coordinates = locations,
    SendCoordinatesAsPolyline = true,
    Sources = new uint[] { 0, 1, 3 },
    Destinations = new uint[] { 2, 4 }
});
```

#### Match
```csharp
using HttpClient client = new HttpClient();
var osrm5x = new Osrm5x(client, "http://router.project-osrm.org/");
var locations = new Location[] {
    new Location(52.517037, 13.388860),
    new Location(52.529407, 13.397634),
    new Location(52.523219, 13.428555)
};

var request = new Osrm.Client.Models.Requests.MatchRequest()
{
    Coordinates = locations
};

var result = await osrm.Match(request);
```

#### Nearest
```csharp
using HttpClient client = new HttpClient();
var osrm5x = new Osrm5x(client, "http://router.project-osrm.org/");
var result = await osrm.Nearest(new Location(52.4224, 13.333086));
```

#### Trip
```csharp
using HttpClient client = new HttpClient();
var osrm5x = new Osrm5x(client, "http://router.project-osrm.org/");
var locations = new Location[] {
    new Location(52.503033, 13.420526),
    new Location(52.516582, 13.429290),
};

var result = await osrm.Trip(locations);
```
