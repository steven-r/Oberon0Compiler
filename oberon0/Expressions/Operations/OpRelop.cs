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

    [ArithmeticOperation(OberonGrammarLexer.GT, BaseTypes.IntType, BaseTypes.IntType, BaseTypes.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.GT, BaseTypes.IntType, BaseTypes.DecimalType, BaseTypes.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.GT, BaseTypes.DecimalType, BaseTypes.DecimalType, BaseTypes.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.GT, BaseTypes.DecimalType, BaseTypes.IntType, BaseTypes.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.GE, BaseTypes.IntType, BaseTypes.IntType, BaseTypes.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.GE, BaseTypes.IntType, BaseTypes.DecimalType, BaseTypes.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.GE, BaseTypes.DecimalType, BaseTypes.DecimalType, BaseTypes.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.GE, BaseTypes.DecimalType, BaseTypes.IntType, BaseTypes.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.LT, BaseTypes.IntType, BaseTypes.IntType, BaseTypes.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.LT, BaseTypes.IntType, BaseTypes.DecimalType, BaseTypes.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.LT, BaseTypes.DecimalType, BaseTypes.DecimalType, BaseTypes.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.LT, BaseTypes.DecimalType, BaseTypes.IntType, BaseTypes.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.LE, BaseTypes.IntType, BaseTypes.IntType, BaseTypes.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.LE, BaseTypes.IntType, BaseTypes.DecimalType, BaseTypes.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.LE, BaseTypes.DecimalType, BaseTypes.DecimalType, BaseTypes.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.LE, BaseTypes.DecimalType, BaseTypes.IntType, BaseTypes.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.NOTEQUAL, BaseTypes.IntType, BaseTypes.IntType, BaseTypes.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.NOTEQUAL, BaseTypes.IntType, BaseTypes.DecimalType, BaseTypes.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.NOTEQUAL, BaseTypes.DecimalType, BaseTypes.DecimalType, BaseTypes.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.NOTEQUAL, BaseTypes.DecimalType, BaseTypes.IntType, BaseTypes.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.EQUAL, BaseTypes.IntType, BaseTypes.IntType, BaseTypes.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.EQUAL, BaseTypes.IntType, BaseTypes.DecimalType, BaseTypes.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.EQUAL, BaseTypes.DecimalType, BaseTypes.DecimalType, BaseTypes.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.EQUAL, BaseTypes.DecimalType, BaseTypes.IntType, BaseTypes.BoolType)]
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