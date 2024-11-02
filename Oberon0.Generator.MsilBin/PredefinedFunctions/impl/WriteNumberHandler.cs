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
using Oberon0.Compiler.Types;

namespace Oberon0.Generator.MsilBin.PredefinedFunctions.impl
{
    [StandardFunctionMetadata("WriteInt", TypeDefinition.VoidTypeName, "INTEGER")]
    [StandardFunctionMetadata("WriteBool", TypeDefinition.VoidTypeName, "BOOLEAN")]
    [StandardFunctionMetadata("WriteReal", TypeDefinition.VoidTypeName, "REAL")]
    [StandardFunctionMetadata("WriteString", TypeDefinition.VoidTypeName, "STRING")]
    // ReSharper disable once UnusedMember.Global
    public class WriteHandler : IStandardFunctionGenerator
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
                                         MsilBinGenerator.MapIdentifierName("Write")))
                                .WithArgumentList(
                                     SyntaxFactory.ArgumentList(
                                         SyntaxFactory.SingletonSeparatedList(
                                             SyntaxFactory.Argument(
                                                 codeGenerator.CompileExpression(parameters[0])))));
        }
    }
}
