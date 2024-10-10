#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using JetBrains.Annotations;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Expressions.Operations.Internal;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions.Operations
{
    [ArithmeticOperation(OberonGrammarLexer.GT, BaseTypes.Int, BaseTypes.Int, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.GT, BaseTypes.Int, BaseTypes.Real, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.GT, BaseTypes.Real, BaseTypes.Real, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.GT, BaseTypes.Real, BaseTypes.Int, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.GE, BaseTypes.Int, BaseTypes.Int, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.GE, BaseTypes.Int, BaseTypes.Real, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.GE, BaseTypes.Real, BaseTypes.Real, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.GE, BaseTypes.Real, BaseTypes.Int, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.LT, BaseTypes.Int, BaseTypes.Int, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.LT, BaseTypes.Int, BaseTypes.Real, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.LT, BaseTypes.Real, BaseTypes.Real, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.LT, BaseTypes.Real, BaseTypes.Int, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.LE, BaseTypes.Int, BaseTypes.Int, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.LE, BaseTypes.Int, BaseTypes.Real, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.LE, BaseTypes.Real, BaseTypes.Real, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.LE, BaseTypes.Real, BaseTypes.Int, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.NOTEQUAL, BaseTypes.Int, BaseTypes.Int, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.NOTEQUAL, BaseTypes.Int, BaseTypes.Real, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.NOTEQUAL, BaseTypes.Real, BaseTypes.Real, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.NOTEQUAL, BaseTypes.Real, BaseTypes.Int, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.NOTEQUAL, BaseTypes.Bool, BaseTypes.Bool, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.EQUAL, BaseTypes.Int, BaseTypes.Int, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.EQUAL, BaseTypes.Int, BaseTypes.Real, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.EQUAL, BaseTypes.Real, BaseTypes.Real, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.EQUAL, BaseTypes.Real, BaseTypes.Int, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.EQUAL, BaseTypes.Bool, BaseTypes.Bool, BaseTypes.Bool)]
    [UsedImplicitly]
    internal class OpRelOp : BinaryOperation
    {
        protected override Expression BinaryOperate(
            BinaryExpression bin,
            Block block,
            IArithmeticOpMetadata operationParameters)
        {
            if (bin.LeftHandSide.IsConst && bin.RightHandSide!.IsConst)
            {
                var left = (ConstantExpression) bin.LeftHandSide;
                var right = (ConstantExpression) bin.RightHandSide;
                bool res;
                if (left.TargetType.Type == BaseTypes.Bool && right.TargetType.Type == BaseTypes.Bool)
                {
                    res = HandleBoolRelop(operationParameters, left, right);
                } else
                {
                    res = HandleStandardRelop(operationParameters, left, right);
                }

                return new ConstantBoolExpression(res);
            }

            return bin; // expression remains the same
        }

        private static bool HandleBoolRelop(
            IArithmeticOpMetadata operationParameters,
            ConstantExpression left,
            ConstantExpression right)
        {
            if (operationParameters.Operation == OberonGrammarLexer.EQUAL)
            {
                return left.ToBool() == right.ToBool();
            }

            if (operationParameters.Operation == OberonGrammarLexer.NOTEQUAL)
            {
                return left.ToBool() != right.ToBool();
            }

            return false;
        }

        private static bool HandleStandardRelop(
            IArithmeticOpMetadata operationParameters,
            ConstantExpression left,
            ConstantExpression right)
        {
            bool res = operationParameters.Operation switch
            {
                OberonGrammarLexer.GT       => left.ToDouble() > right.ToDouble(),
                OberonGrammarLexer.GE       => left.ToDouble() >= right.ToDouble(),
                OberonGrammarLexer.LT       => left.ToDouble() < right.ToDouble(),
                OberonGrammarLexer.LE       => left.ToDouble() <= right.ToDouble(),
                OberonGrammarLexer.NOTEQUAL => Math.Abs(left.ToDouble() - right.ToDouble()) > double.Epsilon,
                OberonGrammarLexer.EQUAL    => Math.Abs(left.ToDouble() - right.ToDouble()) < double.Epsilon,
                _                           => throw new InvalidOperationException("Unknown comparison")
            };

            return res;
        }
    }
}
