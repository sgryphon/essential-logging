# Logging Primer

A simple ["Hello" example](../README.md) isn’t however very useful for showing the different capabilities of logging, so we will use a slightly more complicated example. First of all, here is the program we will use both without and then with simple logging.

* Hello World with no logging (see below)
* [Hello Logging](Hello-Logging.md)

Once the program has had logging statements added, any of the logging providers can be configured to send the log output to a wide variety of destinations.

# Hello World (no logging)

This version of “Hello World” involves a bunch of Worker classes that Poke() each other to say “Hello World”. Sometimes they get sick of being poked.

Create a solution to hold our project:

```powershell
dotnet new solution --name Essential.Logging.Examples
```

Create the new project and add to the solution:

```powershell
dotnet new console --output HelloWorld
dotnet sln add HelloWorld
```

Add a reference to the Microsoft hosting package:

```powershell
dotnet add HelloWorld package Microsoft.Extensions.Hosting
```

Best practice is to enable the new nullable reference types language feature:

```xml
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <LangVersion>latest</LangVersion>
    <Nullable>enable</Nullable>
  </PropertyGroup>
```

Replace the contents of the application with the following, using the [.NET Generic Host](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host):

**Program.cs**
```c#
using System;
```

Running this program may produce the following:

```powershell
PS> dotnet run --project HelloWorld
Hello World 1
Hello World 2
Hello World 1
Hello World 3
Hi
Hello World 2
Hello World 1
Hi
Hello World 3
Hello World 2
```

Lots of “Hello World”, but a bit difficult to tell which bit of code did what.

**Next: [Hello Logging](Hello-Logging.md)**
