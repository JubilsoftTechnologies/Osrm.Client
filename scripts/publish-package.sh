#!/usr/bin/env bash

set -euo pipefail

usage() {
  cat <<'EOF'
Usage:
  ./scripts/publish-package.sh <version> [--skip-push]

Environment:
  NUGET_API_KEY   Required unless --skip-push is used
  NUGET_SOURCE    Optional, defaults to https://api.nuget.org/v3/index.json
EOF
}

if [[ $# -lt 1 || $# -gt 2 ]]; then
  usage
  exit 1
fi

version="$1"
skip_push="false"

if [[ $# -eq 2 ]]; then
  if [[ "$2" != "--skip-push" ]]; then
    usage
    exit 1
  fi

  skip_push="true"
fi

script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
repo_root="$(cd "$script_dir/.." && pwd)"
artifacts_dir="$repo_root/artifacts/publish/$version"
nuget_source="${NUGET_SOURCE:-https://api.nuget.org/v3/index.json}"

if [[ "$skip_push" != "true" && -z "${NUGET_API_KEY:-}" ]]; then
  echo "NUGET_API_KEY must be set unless --skip-push is used." >&2
  exit 1
fi

cd "$repo_root"

dotnet restore Src/Osrm.Client.sln
dotnet build Src/Osrm.Client.sln --configuration Release --no-restore
dotnet test Src/Osrm.Client.Tests/Osrm.Client.Tests.csproj --configuration Release --no-build
dotnet pack Src/Osrm.Client/Osrm.Client.csproj \
  --configuration Release \
  --no-build \
  -p:ContinuousIntegrationBuild=true \
  -p:Version="$version" \
  --output "$artifacts_dir"

if [[ "$skip_push" == "true" ]]; then
  echo "Package artifacts created in $artifacts_dir"
  exit 0
fi

dotnet nuget push "$artifacts_dir"/*.nupkg --api-key "$NUGET_API_KEY" --source "$nuget_source" --skip-duplicate
dotnet nuget push "$artifacts_dir"/*.snupkg --api-key "$NUGET_API_KEY" --source "$nuget_source" --skip-duplicate

