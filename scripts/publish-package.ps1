param(
    [Parameter(Mandatory = $true)]
    [string]$Version,

    [string]$ApiKey = $env:NUGET_API_KEY,

    [string]$Source = 'https://api.nuget.org/v3/index.json',

    [switch]$SkipPush
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

if (-not $SkipPush -and [string]::IsNullOrWhiteSpace($ApiKey)) {
    throw 'NUGET_API_KEY must be set, or pass -ApiKey, unless -SkipPush is used.'
}

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = Resolve-Path (Join-Path $scriptDir '..')
$artifactsDir = Join-Path $repoRoot "artifacts/publish/$Version"

Push-Location $repoRoot
try {
    dotnet restore Src/Osrm.Client.sln
    dotnet build Src/Osrm.Client.sln --configuration Release --no-restore
    dotnet test Src/Osrm.Client.Tests/Osrm.Client.Tests.csproj --configuration Release --no-build
    dotnet pack Src/Osrm.Client/Osrm.Client.csproj `
        --configuration Release `
        --no-build `
        -p:ContinuousIntegrationBuild=true `
        -p:Version=$Version `
        --output $artifactsDir

    if ($SkipPush) {
        Write-Host "Package artifacts created in $artifactsDir"
        return
    }

    dotnet nuget push "$artifactsDir/*.nupkg" --api-key $ApiKey --source $Source --skip-duplicate
    dotnet nuget push "$artifactsDir/*.snupkg" --api-key $ApiKey --source $Source --skip-duplicate
}
finally {
    Pop-Location
}
