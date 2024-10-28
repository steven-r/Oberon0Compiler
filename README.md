# Oberon-0 Compiler

CI/CD: ![Build Status](https://img.shields.io/github/actions/workflow/status/steven-r/Oberon0Compiler/dotnet.yml)

Quality: [![Quality](https://sonarcloud.io/api/project_badges/measure?project=steven-r_Oberon0Compiler&metric=alert_status)](https://sonarcloud.io/api/project_badges/measure?project=steven-r_Oberon0Compiler&metric=alert_status)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=steven-r_Oberon0Compiler&metric=coverage)](https://sonarcloud.io/summary/new_code?id=steven-r_Oberon0Compiler)

[![GitHub license](https://img.shields.io/github/license/steven-r/Oberon0Compiler.svg)](https://github.com/steven-r/Oberon0Compiler/blob/master/LICENSE.md)
![GitHub release](https://img.shields.io/github/release/steven-r/Oberon0Compiler.svg)


An implementation of N. Wirths Oberon 0 language implemented with C# and ANTLR.

The aim is to support both, the RISC instruction set by N. Wirth (long term) and MSIL (current).

The compiler is based on the ANTLR4 parser (C# version).

The current executable code is generated using [Roslyn](https://github.com/dotnet/roslyn)

Current status:

* All language elements are supported
* Support for REAL (Done --> Missing is a lot of library functions)
* Support for STRING (limited)
* Handling of complex type scenarios (`ARRAY OF RECORD`, ...) is limited (see #12)

# Installation

Starting with 0.60 Oberon 0 can be installed by using the [`dotnet tool`](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-install)-command:

```
dotnet tool install Oberon0.Msil --add-source https://github.com/steven-r/Oberon0Compiler/releases/download/<version>/Oberon0.Msil.nupkg
```

Use `oberon0.msil` to compile and execute code.
s