#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Generator.MsilBin.PredefinedFunctions.impl
{
    [StandardFunctionMetadata("Length", "INTEGER", "STRING")]
    // ReSharper disable once UnusedMember.Global
    public class StringFunctions : IStandardFunctionGenerator
    {
        public ExpressionSyntax Generate(IStandardFunctionMetadata metadata,
                                         MsilBinGenerator codeGenerator,
                                         FunctionDeclaration functionDeclaration,
                                         IReadOnlyList<Expression> parameters)
        {
            return SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                codeGenerator.CompileExpression(parameters[0]),
                MsilBinGenerator.MapIdentifierName("Length"));
        }
    }
}
