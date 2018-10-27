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
    using Oberon0.Compiler.Expressions.Operations.Internal;
    using Oberon0.Compiler.Types;

    [ArithmeticOperation(OberonGrammarLexer.GT, BaseType.IntType, BaseType.IntType, BaseType.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.GT, BaseType.IntType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.GT, BaseType.DecimalType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.GT, BaseType.DecimalType, BaseType.IntType, BaseType.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.GE, BaseType.IntType, BaseType.IntType, BaseType.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.GE, BaseType.IntType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.GE, BaseType.DecimalType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.GE, BaseType.DecimalType, BaseType.IntType, BaseType.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.LT, BaseType.IntType, BaseType.IntType, BaseType.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.LT, BaseType.IntType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.LT, BaseType.DecimalType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.LT, BaseType.DecimalType, BaseType.IntType, BaseType.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.LE, BaseType.IntType, BaseType.IntType, BaseType.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.LE, BaseType.IntType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.LE, BaseType.DecimalType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.LE, BaseType.DecimalType, BaseType.IntType, BaseType.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.NOTEQUAL, BaseType.IntType, BaseType.IntType, BaseType.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.NOTEQUAL, BaseType.IntType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.NOTEQUAL, BaseType.DecimalType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.NOTEQUAL, BaseType.DecimalType, BaseType.IntType, BaseType.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.EQUAL, BaseType.IntType, BaseType.IntType, BaseType.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.EQUAL, BaseType.IntType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.EQUAL, BaseType.DecimalType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(OberonGrammarLexer.EQUAL, BaseType.DecimalType, BaseType.IntType, BaseType.BoolType)]
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