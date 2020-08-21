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
using Oberon0.Compiler.Types;

namespace Oberon0.Generator.MsilBin.PredefinedFunctions.impl
{
    [StandardFunctionMetadata("WriteLn", TypeDefinition.VoidTypeName)]
    [UsedImplicitly]
    public class WriteLnHandler : IStandardFunctionGenerator
    {
        public ExpressionSyntax Generate(IStandardFunctionMetadata metadata,
                                         MsilBinGenerator codeGenerator,
                                         FunctionDeclaration functionDeclaration,
                                         IReadOnlyList<Expression> parameters)
        {
            return SyntaxFactory.InvocationExpression(
                SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    MsilBinGenerator.MapIdentifierName("Console"),
                    MsilBinGenerator.MapIdentifierName("WriteLine")));
        }
    }
}
