#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpRelop.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/OpRelop.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Expressions.Operations
{
    using System;

    using JetBrains.Annotations;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions.Constant;
    using Oberon0.Compiler.Expressions.Operations.Internal;
    using Oberon0.Compiler.Types;

    [ArithmeticOperation(OberonGrammarLexer.GT, BaseTypes.Int, BaseTypes.Int, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.GT, BaseTypes.Int, BaseTypes.Decimal, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.GT, BaseTypes.Decimal, BaseTypes.Decimal, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.GT, BaseTypes.Decimal, BaseTypes.Int, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.GE, BaseTypes.Int, BaseTypes.Int, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.GE, BaseTypes.Int, BaseTypes.Decimal, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.GE, BaseTypes.Decimal, BaseTypes.Decimal, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.GE, BaseTypes.Decimal, BaseTypes.Int, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.LT, BaseTypes.Int, BaseTypes.Int, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.LT, BaseTypes.Int, BaseTypes.Decimal, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.LT, BaseTypes.Decimal, BaseTypes.Decimal, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.LT, BaseTypes.Decimal, BaseTypes.Int, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.LE, BaseTypes.Int, BaseTypes.Int, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.LE, BaseTypes.Int, BaseTypes.Decimal, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.LE, BaseTypes.Decimal, BaseTypes.Decimal, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.LE, BaseTypes.Decimal, BaseTypes.Int, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.NOTEQUAL, BaseTypes.Int, BaseTypes.Int, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.NOTEQUAL, BaseTypes.Int, BaseTypes.Decimal, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.NOTEQUAL, BaseTypes.Decimal, BaseTypes.Decimal, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.NOTEQUAL, BaseTypes.Decimal, BaseTypes.Int, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.EQUAL, BaseTypes.Int, BaseTypes.Int, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.EQUAL, BaseTypes.Int, BaseTypes.Decimal, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.EQUAL, BaseTypes.Decimal, BaseTypes.Decimal, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.EQUAL, BaseTypes.Decimal, BaseTypes.Int, BaseTypes.Bool)]
    [UsedImplicitly]
    internal class OpRelOp : BinaryOperation
    {
        protected override Expression BinaryOperate(
            BinaryExpression bin,
            Block block,
            IArithmeticOpMetadata operationParameters)
        {
            if (bin.LeftHandSide.IsConst && bin.RightHandSide.IsConst)
            {
                var left = (ConstantExpression)bin.LeftHandSide;
                var right = (ConstantExpression)bin.RightHandSide;
                bool res;
                switch (operationParameters.Operation)
                {
                    case OberonGrammarLexer.GT:
                        res = left.ToDouble() > right.ToDouble();
                        break;
                    case OberonGrammarLexer.GE:
                        res = left.ToDouble() >= right.ToDouble();
                        break;
                    case OberonGrammarLexer.LT:
                        res = left.ToDouble() < right.ToDouble();
                        break;
                    case OberonGrammarLexer.LE:
                        res = left.ToDouble() <= right.ToDouble();
                        break;
                    case OberonGrammarLexer.NOTEQUAL:
                        res = left.ToDouble() != right.ToDouble();
                        break;
                    case OberonGrammarLexer.EQUAL:
                        res = left.ToDouble() == right.ToDouble();
                        break;
                    default:
                        throw new InvalidOperationException("Unknown comparison");
                }

                return new ConstantBoolExpression(res);
            }

            return bin; // expression remains the same
        }
    }
}