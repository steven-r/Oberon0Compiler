#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Compiler.Generator;
using System.Diagnostics.CodeAnalysis;

namespace Oberon0.Generator.MsilBin.GeneratorInfo;

/// <summary>
/// Options for binary file generation
/// </summary>
[ExcludeFromCodeCoverage]
public class MsilCreateBinaryOptions: CreateBinaryOptionsBase
{
    /// <summary>
    /// The framework to be used
    /// </summary>
    public static string FrameworkVersion => "net10.0";
}
