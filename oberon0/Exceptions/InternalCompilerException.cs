#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Diagnostics.CodeAnalysis;

namespace Oberon0.Compiler.Exceptions;

[ExcludeFromCodeCoverage]
public class InternalCompilerException(string message) : Exception(message);
