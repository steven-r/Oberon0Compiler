#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpDivNumber.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/OpDivNumber.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Expressions.Operations
{
    using JetBrains.Annotations;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions.Constant;
    using Oberon0.Compiler.Expressions.Operations.Internal;
    using Oberon0.Compiler.Types;

    [ArithmeticOperation(OberonGrammarLexer.DIV, BaseType.IntType, BaseType.IntType, BaseType.IntType)]
    [ArithmeticOperation(OberonGrammarLexer.DIV, BaseType.IntType, BaseType.DecimalType, BaseType.DecimalType)]
    [ArithmeticOperation(OberonGrammarLexer.DIV, BaseType.DecimalType, BaseType.DecimalType, BaseType.DecimalType)]
    [ArithmeticOperation(OberonGrammarLexer.DIV, BaseType.DecimalType, BaseType.IntType, BaseType.DecimalType)]
    [UsedImplicitly]
    internal class OpDivNumber : BinaryOperation
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
                if (bin.LeftHandSide.TargetType.BaseType == BaseType.IntType
                    && bin.RightHandSide.TargetType.BaseType == BaseType.IntType)
                {
                    return new ConstantIntExpression(left.ToInt32() / right.ToInt32());
                }

                decimal res = left.ToDouble() / right.ToDouble();
                return new ConstantDoubleExpression(res);
            }

            return bin; // expression remains the same
        }
    }
}