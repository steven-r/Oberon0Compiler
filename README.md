# Oberon-0 Compiler

[![Build status-dev](https://ci.appveyor.com/api/projects/status/pcc7f0s3bq7m6sfh/branch/develop?svg=true)](https://ci.appveyor.com/project/steven-r/oberon0compiler/branch/develop)
[![Build status-main](https://ci.appveyor.com/api/projects/status/pcc7f0s3bq7m6sfh/branch/main?svg=true)](https://ci.appveyor.com/project/steven-r/oberon0compiler/branch/main)
[![QUality](https://sonarqube.com/api/badges/gate?key=steven-r:Oberon0Compiler)](https://sonarqube.com/api/badges/gate?key=steven-r:Oberon0Compiler)

An implementation of N. Wirths Oberon 0 language implemented with C# and ANTLR.

The aim is to support both, the RISC instruction set by N. Wirth (long term) and MSIL (current).

The compiler is based on the ANTLR4 parser (C# version).

Current status:

* All language elements are supported
* Support for REAL (limited, implemented as `decimal`)
* Support for STRING (limited)
* Handling of complex type scenarios (`ARRAY OF RECORD`, ...) is limited
