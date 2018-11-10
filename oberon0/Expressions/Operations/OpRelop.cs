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
            if (bin.LeftHandSide.IsConst && bin.RightHandSide.IsConst)
            {
                var left = (ConstantExpression)bin.LeftHandSide;
                var right = (ConstantExpression)bin.RightHandSide;
                bool res;
                if (left.TargetType.Type == BaseTypes.Bool && right.TargetType.Type == BaseTypes.Bool)
                {
                    res = HandleBoolRelop(operationParameters, left, right);
                }
                else
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
            bool res;
            switch (operationParameters.Operation)
            {
                case OberonGrammarLexer.EQUAL:
                    res = left.ToBool() == right.ToBool();
                    break;
                case OberonGrammarLexer.NOTEQUAL:
                    res = left.ToBool() != right.ToBool();
                    break;
                default:
                    throw new NotImplementedException();
            }

            return res;
        }

        private static bool HandleStandardRelop(
            IArithmeticOpMetadata operationParameters,
            ConstantExpression left,
            ConstantExpression right)
        {
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

            return res;
        }
    }
}