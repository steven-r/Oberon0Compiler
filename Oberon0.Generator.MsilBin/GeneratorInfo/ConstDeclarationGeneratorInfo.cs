#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Generator;

namespace Oberon0.Generator.MsilBin.GeneratorInfo;

/// <summary>
/// This generator info is applied to all const definitions and are applied during code generation.
/// </summary>
public class ConstDeclarationGeneratorInfo: IGeneratorInfo
{
    /// <summary>
    /// If true, this constant will not be generated to target
    /// </summary>
    public bool DropGeneration { get; set; } = false;
    
    /// <summary>
    /// If set, instead of a constant, the generated code will be applied to the target expression
    /// </summary>
    public Func<Declaration, ExpressionSyntax>? GeneratorFunc { get; set; }
}
