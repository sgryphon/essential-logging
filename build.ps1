#!/usr/bin/env pwsh
Write-Host 'Building Essential Logging'

dotnet tool restore

Get-ChildItem /tmp/.nuget/packages/gitversion.tool/

dotnet gitversion /output json
$json = (dotnet tool run dotnet-gitversion /output json)
$v = ($json | ConvertFrom-Json)
Write-Host "Building version $($v.NuGetVersion)"

dotnet test (Join-Path $PSScriptRoot Essential.Logging.sln)
if (!$?) { throw 'Tests failed' }

dotnet pack (Join-Path $PSScriptRoot 'src/Essential.LogTemplate') -c Release -p:GetVersion=false -p:AssemblyVersion=$($v.AssemblySemVer) -p:FileVersion=$($v.AssemblySemFileVer) -p:Version=$($v.SemVer)+$($v.ShortSha) -p:PackageVersion=$($v.NuGetVersion) --output pack
dotnet pack (Join-Path $PSScriptRoot 'src/Essential.LoggerProvider.RollingFile') -c Release -p:GetVersion=false -p:AssemblyVersion=$($v.AssemblySemVer) -p:FileVersion=$($v.AssemblySemFileVer) -p:Version=$($v.SemVer)+$($v.ShortSha) -p:PackageVersion=$($v.NuGetVersion) --output pack
