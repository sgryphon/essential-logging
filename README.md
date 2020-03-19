![Essential Logging](docs/images/diagnostics-logo-64.png)

# Essential .NET Logging

Guidance, links, utilities, and additional logger providers for .NET `Microsoft.Extensions.Logging`

## Using the loggers

### Logger providers

* PS> **dotnet add package [Essential.LoggerProvider.RollingFile](src/Essential.LoggerProvider.RollingFile)**
* PS> **dotnet add package [Syslog.StructuredData](https://github.com/sgryphon/syslog-structureddata)**

### Examples

General `Microsoft.Extensions.Logging`:

* [Getting Started](examples/GettingStarted)
* [Hello Logging](examples/HelloLogging)

Essential logging `Essential.LoggerProvider`:

* [Hello Rolling File](examples/HelloRollingFile)

### Guidance

* [Theory of Event IDs](docs/Event-Ids.md) 
* [Logging Levels](docs/Logging-Levels.md)

From Microsoft:

* [High performance logging](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/loggermessage)
* [Logging Fundamentals](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/) -- scroll down to see the built in system categories and the built-in logging providers.


## Development getting started

### Pre-requisites

* .NET Core 3.1 development tools

#### Optional requirements

* PowerShell Core, to run scripts
* An editor, e.g. VS Code, if you want to contribute


### Running unit tests

To test:

```
dotnet test test/Essential.LogTemplate.Tests
```

### Test running examples

Run examples and check the output:

```powershell
dotnet run --project examples/HelloRollingFile
```

### Versioning

Versioning is done automatically with GitVersion, so you can determine the build / package 
number based on the git branch.

### Packaging

For updating references across projects, you need to build dependent packages first:

```powershell
dotnet pack src/Essential.LogTemplate --output pack
```

This will output to the `pack` folder in the project.

To build the logger provider package:

```powershell
dotnet pack src/Essential.LoggerProvider.RollingFile --output pack
```

#### Testing packages

To turn off the automatic GitVersion numbering, and override with a specific package version (e.g. if you want to update and test package references before release):

```powershell
dotnet pack src/Essential.LogTemplate -p:GetVersion=false -p:PackageVersion=1.0.0 --output pack
```

Note that during development you may test
multiple builds of the same version, so you need to clear out the
NuGet cache (delete from the .nuget/packages folder) and force restore of packages before running:

```powershell
Get-ChildItem (Join-Path $ENV:HOME '.nuget/packages/essential.logging.*') | Remove-Item -Recurse -Force
dotnet restore examples/HelloRollingFile --force --no-cache
```


## Other logger provider projects

* Rolling log file, by Andrew Lock

```
dotnet add package NetEscapades.Extensions.Logging.RollingFile
```

Project repository at: https://github.com/andrewlock/NetEscapades.Extensions.Logging

* Syslog, by mguinness

```
dotnet add package Syslog.Framework.Logging
```

Project repository at: https://github.com/mguinness/syslog-framework-logging

* Logstash (and Graylog), by Matt Cole  

```
dotnet add package Gelf.Extensions.Logging
```

Project repository at: https://github.com/mattwcole/gelf-extensions-logging

* Seq

https://github.com/datalust/seq-extensions-logging


## Development getting started

### Pre-requisites

* .NET Core 3.0 development tools
### Compile and test


### Optional requirements

* PowerShell Core, to run build scripts
* An editor, e.g. VS Code, if you want to contribute

### Build with version number

To build a release version and nuget package, with a gitversion based version number:

```
./build.ps1
```


## License

Copyright (C) 2019 Gryphon Technology Pty Ltd

This library is free software: you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License and GNU General Public License for more details.

You should have received a copy of the GNU Lesser General Public License and GNU General Public License along with this library. If not, see <https://www.gnu.org/licenses/>.
