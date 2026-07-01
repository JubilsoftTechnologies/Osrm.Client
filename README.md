# Osrm.Client

Modern .NET client for the OSRM 5.x HTTP API.

## Package

- **NuGet package**: `Osrm.Client.NetCore`
- **Primary target**: `net9.0`
- **Compatibility target**: `netstandard2.0`

Version 4 is a major modernization release. It is built for current .NET first, while still offering a low-maintenance compatibility target for older consumers that can use `netstandard2.0`.

## Supported OSRM services

- **Route**
- **Table**
- **Match**
- **Nearest**
- **Trip**

This package targets the JSON-based OSRM 5.x HTTP API surface exposed through `Osrm5x` and `IOsrmClient`.

## Feature coverage highlights

- Shared request options across services:
  - bearings, radiuses, hints
  - `generate_hints`
  - `approaches`
  - `exclude`
  - `snapping`
  - `skip_waypoints`
  - polyline-encoded coordinate input
- Route options:
  - boolean or numeric alternatives
  - steps
  - annotations
  - geometries / overview
  - continue-straight
  - explicit waypoint indexes
- Table options:
  - sources / destinations
  - duration or distance annotations
  - `fallback_speed`
  - `fallback_coordinate`
  - `scale_factor`
- Match options:
  - timestamps
  - steps
  - annotations
  - geometries / overview
  - `gaps`
  - `tidy`
  - explicit waypoint indexes
- Trip options:
  - steps
  - annotations
  - geometries / overview
  - `roundtrip`
  - `source`
  - `destination`

## Response model coverage highlights

- Top-level `data_version`
- Route `weight` and `weight_name`
- Leg `annotation`
- Step metadata including intersections, lanes, pronunciations, refs, exits, and driving side
- Waypoint metadata including nodes and alternatives count
- Table `distances`, nullable matrix cells, and `fallback_speed_cells`
- Geometry decoding for `polyline`, `polyline6`, and `geojson`

The public API always uses `Location(latitude, longitude)`. OSRM wire-format coordinates are translated internally from `[longitude, latitude]`.

## Quick start

```csharp
using Osrm.Client;
using Osrm.Client.Models;
using Osrm.Client.Models.Requests;

using HttpClient httpClient = new();
var osrm = new Osrm5x(httpClient, "https://router.project-osrm.org/");

var locations = new[]
{
    new Location(52.503033, 13.420526),
    new Location(52.516582, 13.429290),
};

var route = await osrm.Route(new RouteRequest
{
    Coordinates = locations,
    Steps = true,
    Annotations = "distance,duration",
    Geometries = "geojson"
});

var matrix = await osrm.Table(new TableRequest
{
    Coordinates = locations,
    Annotations = "distance,duration"
});
```

## Installation

```bash
dotnet add package Osrm.Client.NetCore
```

## Validation and runtime behavior

- Requests are validated before HTTP calls are sent.
- Non-success OSRM responses raise `HttpRequestException`.
- `Osrm5x.Timeout` is enforced per request and raises `TimeoutException` when exceeded.

## Build, test, and pack

From the repository root:

```bash
dotnet restore Src/Osrm.Client.sln
dotnet build Src/Osrm.Client.sln --configuration Release
dotnet test Src/Osrm.Client.Tests/Osrm.Client.Tests.csproj --configuration Release
dotnet pack Src/Osrm.Client/Osrm.Client.csproj -c Release
```

The default test suite runs offline. Response tests use stubbed `HttpClient` responses instead of the public OSRM demo server.

## Publishing

- CI validation is defined in `.github/workflows/ci.yml`.
- NuGet publishing is defined in `.github/workflows/publish-package.yml`.
- Publishing requires `NUGET_API_KEY` and pushes both the main package and symbols package.
- Local publish scripts are available for either shell:

```bash
./scripts/publish-package.sh 4.0.0 --skip-push
NUGET_API_KEY=your-key ./scripts/publish-package.sh 4.0.0
```

```powershell
pwsh ./scripts/publish-package.ps1 -Version 4.0.0 -SkipPush
pwsh ./scripts/publish-package.ps1 -Version 4.0.0 -ApiKey $env:NUGET_API_KEY
```

## Intentional gaps

- Tile service is not implemented.
- Flatbuffers output is not implemented.

## Migration notes for v4

- The package now targets `net9.0` and `netstandard2.0`.
- Validation is stricter for malformed request options and mismatched per-coordinate arrays.
- Response handling now correctly maps OSRM coordinate arrays into `Location(latitude, longitude)`.
- Geometry parsing now supports `polyline6` and `geojson` in addition to traditional polyline responses.
- `TripRequest.Annotate` is retained as a compatibility shim, but emits the correct OSRM `annotations` query parameter.

## Origin

Forked from `narfunikita/Osrm.Client`, then modernized and extended for current .NET and OSRM 5.x usage.
