#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;
using Oberon0.Shared;

namespace Oberon0.Generator.MsilBin.PredefinedFunctions.impl
{
    [StandardFunctionMetadata("ABS", "REAL", "REAL")]
    [StandardFunctionMetadata("ABS", "INTEGER", "INTEGER")]
    [UsedImplicitly]
    public class MathAbs : IStandardFunctionGenerator
    {
        public ExpressionSyntax Generate(
            IStandardFunctionMetadata metadata,
            ICodeGenerator codeGenerator,
            FunctionDeclaration functionDeclaration,
            IReadOnlyList<Expression> parameters)
        {
            return SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        MsilBinGenerator.MapIdentifierName("Math"),
                        MsilBinGenerator.MapIdentifierName("Abs")))
                .WithArgumentList(
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SingletonSeparatedList(
                            SyntaxFactory.Argument(codeGenerator.HandleExpression(parameters[0])))));
        }
    }
}