# Oberon-0 Compiler

[![Build status](https://ci.appveyor.com/api/projects/status/pcc7f0s3bq7m6sfh?svg=true)](https://ci.appveyor.com/project/steven-r/oberon0compiler)
[![QUality](https://sonarcloud.io/api/project_badges/measure?project=steven-r_Oberon0Compiler&metric=alert_status)](https://sonarcloud.io/api/project_badges/measure?project=steven-r_Oberon0Compiler&metric=alert_status)
[![GitHub license](https://img.shields.io/github/license/steven-r/Oberon0Compiler.svg)](https://github.com/steven-r/Oberon0Compiler/blob/master/LICENSE.md)
![GitHub release](https://img.shields.io/github/release/steven-r/Oberon0Compiler.svg)


An implementation of N. Wirths Oberon 0 language implemented with C# and ANTLR.

The aim is to support both, the RISC instruction set by N. Wirth (long term) and MSIL (current).

The compiler is based on the ANTLR4 parser (C# version).

Current status:

* All language elements are supported
* Support for REAL (Done --> Missing is a lot of library functions)
* Support for STRING (limited)
* Handling of complex type scenarios (`ARRAY OF RECORD`, ...) is limited (see #12)

Use `Oberon0Msil.exe` to compile and execute code.