# Osrm.Client repository instructions

## Build and test commands

- The repository is a single .NET solution rooted at `Src/Osrm.Client.sln`.
- The library multi-targets `net9.0` and `netstandard2.0`. The demo and test projects target `net9.0`.
- `NuGet.Config` is checked in and points restores at `https://api.nuget.org/v3/index.json`.
- Restore from the repo root with `dotnet restore Src/Osrm.Client.sln`.
- Build the full solution from the repo root with `dotnet build Src/Osrm.Client.sln`.
- Run the full test project with `dotnet test Src/Osrm.Client.Tests/Osrm.Client.Tests.csproj`.
- After restore/build, run one MSTest method with `dotnet test Src/Osrm.Client.Tests/Osrm.Client.Tests.csproj --no-restore --filter "FullyQualifiedName=Osrm.Client.Tests.RequestModelsTests.RouteRequest_Defaults"`.
- Create NuGet artifacts explicitly with `dotnet pack Src/Osrm.Client/Osrm.Client.csproj -c Release`.
- There is no dedicated repo lint command or analyzer step checked in.

## High-level architecture

- `Src/Osrm.Client` is the library. `Osrm5x` implements `IOsrmClient` and exposes async wrappers for the OSRM 5.x `route`, `table`, `match`, `nearest`, and `trip` endpoints.
- All service methods eventually call `Osrm5x.Send<T>()`, which builds the request URL with `OsrmRequestBuilder.GetUrl(...)`, performs an `HttpClient.GetAsync(...)`, throws explicit `HttpRequestException`s for non-success responses, and deserializes the JSON payload with `System.Text.Json`.
- Request models live under `Models/Requests`. Every service-specific request derives from `BaseRequest`, which owns shared coordinate/bearing/radius/hint handling plus `CoordinatesUrlPart`.
- Response models live under `Models/Responses` plus shared domain types in `Models/`. `Route` and `RouteStep` store encoded geometry strings from OSRM and decode them on demand through `OsrmPolylineConverter`.
- `Src/Osrm.Client.Demo` is the executable usage sample. `Src/Osrm.Client.Tests` is the regression suite for request defaults, URL generation, and deterministic response deserialization/HTTP behavior.

## Key conventions

- The public API uses `new Location(latitude, longitude)`, but OSRM URLs must serialize coordinates as `longitude,latitude`. Preserve that translation when changing request builders, polyline handling, or request tests.
- OSRM response location arrays are also `longitude,latitude`; `Waypoint.Location` and `StepManeuver.Location` must convert them back into the public `Location(latitude, longitude)` shape.
- Request constructors establish the default OSRM option values, and `UrlParams` intentionally omits values that are still at their defaults. Existing URL tests rely on "default values are not sent" behavior.
- The library is intentionally OSRM 5.x only. Keep new API surface area aligned with the `Osrm5x` client and the existing `Osrm.Client`, `Osrm.Client.Models`, `Osrm.Client.Models.Requests`, and `Osrm.Client.Models.Responses` namespaces.
- `ResponseTests`, `RequestModelsTests`, and `RequestUrlsTests` are all offline by default. `ResponseTests` use stubbed `HttpClient` responses and are the preferred place to extend response/error-path coverage.
- Packaging is explicit now: use `dotnet pack` locally, `ci.yml` for restore/build/test/pack validation, and `publish-package.yml` for manual NuGet publishing with `NUGET_API_KEY`.
