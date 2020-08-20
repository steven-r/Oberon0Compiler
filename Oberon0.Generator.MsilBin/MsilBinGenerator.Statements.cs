#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Oberon0.Compiler;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Solver;
using Oberon0.Compiler.Statements;
using Oberon0.Generator.MsilBin.PredefinedFunctions;

namespace Oberon0.Generator.MsilBin
{
    partial class MsilBinGenerator
    {
        private SyntaxList<StatementSyntax> GenerateBlockStatements(Block block, SyntaxList<StatementSyntax>? givenStatements = null)
        {
            var statements = new SyntaxList<StatementSyntax>(givenStatements);

            foreach (var blockStatement in block.Statements)
                switch (blockStatement)
                {
                    case WhileStatement whileStatement:
                        statements = statements.Add(
                            SyntaxFactory.WhileStatement(CompileExpression<ExpressionSyntax>(whileStatement.Condition),
                                SyntaxFactory.Block(GenerateBlockStatements(whileStatement.Block))));
                        break;
                    case IfStatement ifStatement:
                        var current = SyntaxFactory.IfStatement(CompileExpression<ExpressionSyntax>(ifStatement.Conditions[^1]),
                            SyntaxFactory.Block(GenerateBlockStatements(ifStatement.ThenParts[^1])));
                        if (ifStatement.ElsePart != null)
                            current = current.WithElse(SyntaxFactory.ElseClause(
                                SyntaxFactory.Block(GenerateBlockStatements(ifStatement.ElsePart))));
                        for (var i = ifStatement.Conditions.Count -2; i >= 0; i--)
                        {
                            var expression = CompileExpression<ExpressionSyntax>(ifStatement.Conditions[i]);
                            current = SyntaxFactory.IfStatement(default, expression,
                                SyntaxFactory.Block(GenerateBlockStatements(ifStatement.ThenParts[i])),
                                SyntaxFactory.ElseClause(SyntaxFactory.Block(current)));
                        }

                        statements = statements.Add(current);
                        break;
                    case AssignmentStatement assignmentStatement:
                        var assignment = SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                            GenerateVariableReference(assignmentStatement.Variable, assignmentStatement.Selector),
                            CompileExpression<ExpressionSyntax>(assignmentStatement.Expression));
                        statements = statements.Add(SyntaxFactory.ExpressionStatement(assignment));
                        break;
                    case ProcedureCallStatement procedureCallStatement:
                        statements = statements.Add(SyntaxFactory.ExpressionStatement(
                            CallFunction(procedureCallStatement.FunctionDeclaration,
                                procedureCallStatement.Parameters)));
                        break;
                    case RepeatStatement repeatStatement:
                        statements = HandleRepeatStatement(statements, repeatStatement);
                        break;
                    default:
                        throw new NotImplementedException("Following not handled: " + blockStatement.GetType());
                }

            return statements;
        }

        private SyntaxList<StatementSyntax> HandleRepeatStatement(SyntaxList<StatementSyntax> statements, RepeatStatement repeatStatement)
        {
            var expression = BinaryExpression.Create(OberonGrammarLexer.NOT, repeatStatement.Condition, null,
                repeatStatement.Block.Parent);
            var compiled = ConstantSolver.Solve(expression, repeatStatement.Block.Parent);
            statements = statements.Add(
                SyntaxFactory.DoStatement(
                    SyntaxFactory.Block(GenerateBlockStatements(repeatStatement.Block)),
                    CompileExpression<ExpressionSyntax>(compiled)));
            return statements;
        }

        private ExpressionSyntax CallFunction(FunctionDeclaration functionDeclaration, List<Expression> parameters)
        {
            if (functionDeclaration.IsInternal) return CallInternalFunction(functionDeclaration, parameters);


            var argumentList = new SyntaxNodeOrTokenList();
            bool first = true;
            int i = 0;
            var parameterDeclarations =
                functionDeclaration.Block.Declarations.OfType<ProcedureParameterDeclaration>().ToArray();
            foreach (var parameter in parameters)
            {

                if (!first) argumentList = argumentList.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));

                first = false;
                var argument = SyntaxFactory.Argument(CompileExpression<ExpressionSyntax>(parameter));
                if (parameterDeclarations[i].IsVar) argument = argument.WithRefKindKeyword(SyntaxFactory.Token(SyntaxKind.RefKeyword));
                argumentList = argumentList.Add(argument);
                i++;
            }

            if (functionDeclaration is ExternalFunctionDeclaration efd) return CallExternalFunction(efd, argumentList);

            // local function/procedure
            return
                SyntaxFactory.InvocationExpression(
                    MapIdentifierName(functionDeclaration.Name)).WithArgumentList(
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SeparatedList<ArgumentSyntax>(argumentList)));
        }

        private static ExpressionSyntax CallExternalFunction(ExternalFunctionDeclaration efd,
            SyntaxNodeOrTokenList argumentList)
        {
            string[] names = efd.ClassName.Split(".");
            ExpressionSyntax current = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                MapIdentifierName(names[0]),
                MapIdentifierName(names.Length > 1 ? names[1] : efd.MethodName));

            if (names.Length < 2)
            // this case can be handled by a single MemberAccessExpression
                return
                    SyntaxFactory.InvocationExpression(
                        current).WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(argumentList)));

            int index = names.Length - 1;
            while (index > 2)
                current = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, current,
                    MapIdentifierName(names[index--]));

            current = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, current,
                MapIdentifierName(efd.MethodName));
            return
                SyntaxFactory.InvocationExpression(
                    current).WithArgumentList(
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SeparatedList<ArgumentSyntax>(argumentList)));
        }

        private ExpressionSyntax CallInternalFunction(FunctionDeclaration functionDeclaration, List<Expression> parameters)
        {
            var func = StandardFunctionRepository.Get(functionDeclaration);
            return func.Instance.Generate(func, this, functionDeclaration,
                parameters);
        }
    }
}
