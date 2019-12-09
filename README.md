# Essential Logging

Guidance, links, and additional log destinations for .NET Microsoft.Extensions.Logging

## Using the loggers

Use Nuget:

## Guidance

* High performance logging: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/loggermessage

See the Microsoft documentation and scroll down to see the built in system categories, the built-in logging providers

https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/

### Other d

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

To test:

```
dotnet test ...
```

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
