﻿#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpModNumber.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/OpModNumber.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Expressions.Operations
{
    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions.Constant;
    using Oberon0.Compiler.Expressions.Operations.Internal;
    using Oberon0.Compiler.Types;

    [ArithmeticOperation(OberonGrammarLexer.MOD, BaseTypes.Int, BaseTypes.Int, BaseTypes.Int)]
    [ArithmeticOperation(OberonGrammarLexer.MOD, BaseTypes.Int, BaseTypes.Decimal, BaseTypes.Decimal)]
    [ArithmeticOperation(OberonGrammarLexer.MOD, BaseTypes.Decimal, BaseTypes.Decimal, BaseTypes.Decimal)]
    [ArithmeticOperation(OberonGrammarLexer.MOD, BaseTypes.Decimal, BaseTypes.Int, BaseTypes.Decimal)]
    internal class OpModNumber : BinaryOperation
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
                if (bin.LeftHandSide.TargetType.Type == BaseTypes.Int
                    && bin.RightHandSide.TargetType.Type == BaseTypes.Int)
                {
                    return new ConstantIntExpression(left.ToInt32() % right.ToInt32());
                }

                decimal res = left.ToDouble() % right.ToDouble();
                return new ConstantDoubleExpression(res);
            }

            return bin; // expression remains the same
        }
    }
}