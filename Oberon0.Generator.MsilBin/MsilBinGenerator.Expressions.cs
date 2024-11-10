#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Solver;
using Oberon0.Compiler.Types;
using Oberon0.Generator.MsilBin.GeneratorInfo;

namespace Oberon0.Generator.MsilBin;

partial class MsilBinGenerator
{
    private static readonly Dictionary<int, SyntaxKind> UnaryExpressionMapping = new()
    {
        {OberonGrammarLexer.MINUS, SyntaxKind.UnaryMinusExpression},
        {OberonGrammarLexer.PLUS, SyntaxKind.UnaryPlusExpression},
        {OberonGrammarLexer.NOT, SyntaxKind.LogicalNotExpression}
    };

    /// <summary>
    /// Mapping table to map an Oberon0 binary operation to the corresponding Roslyn mapping. Works for numbers only.
    ///
    /// Please have a look at <see cref="HandleBinaryExpression"/> for handling
    /// </summary>
    private static readonly Dictionary<int, SyntaxKind> BinaryExpressionMapping = new()
    {
        {OberonGrammarLexer.EQUAL, SyntaxKind.EqualsExpression},
        {OberonGrammarLexer.AND, SyntaxKind.LogicalAndExpression},
        {OberonGrammarLexer.OR, SyntaxKind.LogicalOrExpression},
        {OberonGrammarLexer.LT, SyntaxKind.LessThanExpression},
        {OberonGrammarLexer.LE, SyntaxKind.LessThanOrEqualExpression},
        {OberonGrammarLexer.GT, SyntaxKind.GreaterThanExpression},
        {OberonGrammarLexer.GE, SyntaxKind.GreaterThanOrEqualExpression},
        {OberonGrammarLexer.NOTEQUAL, SyntaxKind.NotEqualsExpression},
        {OberonGrammarLexer.PLUS, SyntaxKind.AddExpression},
        {OberonGrammarLexer.MINUS, SyntaxKind.SubtractExpression},
        {OberonGrammarLexer.STAR, SyntaxKind.MultiplyExpression},
        {OberonGrammarLexer.DIV, SyntaxKind.DivideExpression},
        {OberonGrammarLexer.MOD, SyntaxKind.ModuloExpression}
    };

    internal ExpressionSyntax CompileExpression(Expression compilerExpression)
    {
        return compilerExpression switch
        {
            UnaryExpression ua => SyntaxFactory.PrefixUnaryExpression(UnaryExpressionMapping[ua.Operator],
                CompileExpression(ua.LeftHandSide)),
            BinaryExpression be => HandleBinaryExpression(be),
            VariableReferenceExpression vre => GenerateVariableReference(vre.Declaration, vre.Selector),
            ConstantExpression ce           => GenerateConstantLiteral(ce),
            StringExpression se => SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression,
                SyntaxFactory.Literal(se.Value)),
            FunctionCallExpression fce => CallFunction(fce.FunctionDeclaration, fce.Parameters),
            _ => throw new ArgumentException("Cannot process exception type " + compilerExpression.GetType().Name,
                nameof(compilerExpression))
        };
    }

    private ExpressionSyntax HandleBinaryExpression(BinaryExpression be)
    {
        if (be is { Operator: OberonGrammarLexer.STAR, TargetType.Type: BaseTypes.String })
        {
            // special treatment for string multiplication:
            // implement the following C# code: string.Concat(Enumerable.Repeat(LHS, RHS))
            var argumentList = new SyntaxNodeOrTokenList();
            argumentList = argumentList.AddRange([
                SyntaxFactory.Argument(CompileExpression(be.LeftHandSide)),
                SyntaxFactory.Token(SyntaxKind.CommaToken),
                SyntaxFactory.Argument(CompileExpression(be.RightHandSide!))
            ]);
            var ie = SyntaxFactory.InvocationExpression(
                                       SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                           MapIdentifierName("Enumerable"),
                                           MapIdentifierName("Repeat")))
                                  .WithArgumentList(SyntaxFactory.ArgumentList(
                SyntaxFactory.SeparatedList<ArgumentSyntax>(argumentList)));
            // ReSharper disable once UseCollectionExpression
            argumentList = new SyntaxNodeOrTokenList();
            argumentList = argumentList.Add(SyntaxFactory.Argument(ie));
            return SyntaxFactory.InvocationExpression(
                                     SyntaxFactory.MemberAccessExpression(
                                         SyntaxKind.SimpleMemberAccessExpression,
                                         SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword)),
                                         MapIdentifierName("Concat")))
                                .WithArgumentList(SyntaxFactory.ArgumentList(
                                     SyntaxFactory.SeparatedList<ArgumentSyntax>(argumentList)));
        }
        // standard treatment
        return SyntaxFactory.ParenthesizedExpression(SyntaxFactory.BinaryExpression(
            BinaryExpressionMapping[be.Operator],
            CompileExpression(be.LeftHandSide),
            CompileExpression(be.RightHandSide!)));
    }

    private ExpressionSyntax GenerateVariableReference(Declaration declaration, VariableSelector? selector,
                                                       bool ignoreReplace = false)
    {
        if (!ignoreReplace && declaration.GeneratorInfo is DeclarationGeneratorInfo { ReplacedBy: not null } dgi)
        {
            declaration = dgi.ReplacedBy;
        }

        if (selector == null)
        {
            return MapIdentifierName(declaration.Name);
        }

        ExpressionSyntax? current = null;
        foreach (var selectorElement in selector)
        {
            switch (selectorElement)
            {
                case IndexSelector indexSelector:
                    // add a -1 to the selector (and compile) as c# arrays start at 0
                    var binaryExpression = BinaryExpression.Create(OberonGrammarLexer.MINUS,
                        indexSelector.IndexDefinition,
                        new ConstantIntExpression(1), declaration.Block!);
                    var solvedExpression = ConstantSolver.Solve(binaryExpression!, declaration.Block!);
                    var accessor = SyntaxFactory.ElementAccessExpression(
                        MapIdentifierName(declaration.Name),
                        SyntaxFactory.BracketedArgumentList(
                            SyntaxFactory.SingletonSeparatedList(
                                SyntaxFactory.Argument(CompileExpression(solvedExpression)))));
                    if (current == null)
                    {
                        current = accessor;
                    } else
                    {
                        throw new OperationCanceledException("Cannot process nested selectors");
                    }

                    break;
                case IdentifierSelector identifierSelector:
                    var identAccessor =
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            MapIdentifierName(declaration.Name),
                            MapIdentifierName(identifierSelector.Element.Name));
                    if (current == null)
                    {
                        current = identAccessor;
                    } else
                    {
                        throw new OperationCanceledException("Cannot process nested selectors");
                    }

                    break;
                default:
                    throw new InvalidOperationException("Cannot handle type " + selectorElement.GetType());
            }
        }

        return current!;
    }

    private static LiteralExpressionSyntax GenerateConstantLiteral(ConstantExpression constantExpression) => constantExpression switch
    {
        ConstantIntExpression cie => SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression,
            SyntaxFactory.Literal(cie.ToInt32())),
        ConstantBoolExpression cbe => SyntaxFactory.LiteralExpression(cbe.ToBool()
            ? SyntaxKind.TrueLiteralExpression
            : SyntaxKind.FalseLiteralExpression),
        ConstantDoubleExpression cde => SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression,
            SyntaxFactory.Literal(cde.ToDouble())),
        _ => throw new ArgumentException(
            "Cannot process constant expression type " + constantExpression.GetType().Name,
            nameof(constantExpression))
    };
}