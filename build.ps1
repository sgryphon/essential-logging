Write-Host 'Building Essential Logging'

dotnet tool restore
$v = (dotnet tool run dotnet-gitversion | ConvertFrom-Json)
Write-Host "Building version $($v.NuGetVersion)"

dotnet test (Join-Path $PSScriptRoot Essential.Logging.sln)
if (!$?) { throw 'Tests failed' }

dotnet pack (Join-Path $PSScriptRoot 'src/Essential.LogTemplate') -c Release -p:GetVersion=false -p:AssemblyVersion=$($v.AssemblySemVer) -p:FileVersion=$($v.AssemblySemFileVer) -p:Version=$($v.SemVer)+$($v.ShortSha) -p:PackageVersion=$($v.NuGetVersion) --output pack
dotnet pack (Join-Path $PSScriptRoot 'src/Essential.LoggerProvider.RollingFile') -c Release -p:GetVersion=false -p:AssemblyVersion=$($v.AssemblySemVer) -p:FileVersion=$($v.AssemblySemFileVer) -p:Version=$($v.SemVer)+$($v.ShortSha) -p:PackageVersion=$($v.NuGetVersion) --output pack
