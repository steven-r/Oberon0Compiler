﻿#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpAddNumber.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/OpAddNumber.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Expressions.Operations
{
    using JetBrains.Annotations;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions.Operations.Internal;
    using Oberon0.Compiler.Types;

    [ArithmeticOperation(OberonGrammarLexer.PLUS, BaseType.IntType, BaseType.IntType, BaseType.IntType)]
    [ArithmeticOperation(OberonGrammarLexer.PLUS, BaseType.IntType, BaseType.DecimalType, BaseType.DecimalType)]
    [ArithmeticOperation(OberonGrammarLexer.PLUS, BaseType.DecimalType, BaseType.DecimalType, BaseType.DecimalType)]
    [ArithmeticOperation(OberonGrammarLexer.PLUS, BaseType.DecimalType, BaseType.IntType, BaseType.DecimalType)]
    [UsedImplicitly]
    internal class OpAddNumber : BinaryOperation
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
                    return new ConstantIntExpression(left.ToInt32() + right.ToInt32());
                return new ConstantDoubleExpression(left.ToDouble() + right.ToDouble());
            }

            return bin; // expression remains the same
        }
    }
}