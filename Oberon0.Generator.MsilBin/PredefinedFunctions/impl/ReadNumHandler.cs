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
    /// <summary>
    ///     Internal functions to read numbers
    /// </summary>
    [StandardFunctionMetadata("ReadInt", TypeDefinition.VoidTypeName, "&INTEGER")]
    [StandardFunctionMetadata("ReadReal", TypeDefinition.VoidTypeName, "&REAL")]
    [StandardFunctionMetadata("ReadBool", TypeDefinition.VoidTypeName, "&BOOLEAN")]
    [UsedImplicitly]
    public class ReadNumHandler : IStandardFunctionGenerator
    {
        private static readonly Dictionary<string, string> MapTypeNames = new Dictionary<string, string>
        {
            {"ReadInt", "Int32"},
            {"ReadReal", "Double"},
            {"ReadBool", "Boolean"}
        };

        /// <summary>
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="codeGenerator"></param>
        /// <param name="functionDeclaration"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ExpressionSyntax Generate(IStandardFunctionMetadata metadata,
                                         MsilBinGenerator codeGenerator,
                                         FunctionDeclaration functionDeclaration,
                                         IReadOnlyList<Expression> parameters)
        {
            var reference = (VariableReferenceExpression) parameters[0];
            // generates {reference} = System.{type}.Parse(Console.ReadLine())
            return SyntaxFactory.AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                codeGenerator.CompileExpression(reference),
                SyntaxFactory.InvocationExpression(
                                  SyntaxFactory.MemberAccessExpression(
                                      SyntaxKind.SimpleMemberAccessExpression,
                                      SyntaxFactory.MemberAccessExpression(
                                          SyntaxKind.SimpleMemberAccessExpression,
                                          MsilBinGenerator.MapIdentifierName("System"),
                                          MsilBinGenerator.MapIdentifierName(MapTypeNames[functionDeclaration.Name])),
                                      MsilBinGenerator.MapIdentifierName("Parse")))
                             .WithArgumentList(
                                  SyntaxFactory.ArgumentList(
                                      SyntaxFactory.SingletonSeparatedList(
                                          SyntaxFactory.Argument(
                                              SyntaxFactory.InvocationExpression(
                                                  SyntaxFactory.MemberAccessExpression(
                                                      SyntaxKind.SimpleMemberAccessExpression,
                                                      MsilBinGenerator.MapIdentifierName("Console"),
                                                      MsilBinGenerator.MapIdentifierName("ReadLine"))))))));
        }
    }
}
