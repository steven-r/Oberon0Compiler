﻿#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpUnaryMinus.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/OpUnaryMinus.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Expressions.Operations
{
    using JetBrains.Annotations;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions.Operations.Internal;
    using Oberon0.Compiler.Types;

    /// <summary>
    /// Handle "~".
    /// </summary>
    /// <seealso cref="IArithmeticOperation" />
    /// <remarks>This function is some kind of exception as - usually takes one parameter. The second is handled as a dummy</remarks>
    [ArithmeticOperation(OberonGrammarLexer.MINUS, BaseType.IntType, BaseType.AnyType, BaseType.IntType)]
    [ArithmeticOperation(OberonGrammarLexer.MINUS, BaseType.DecimalType, BaseType.AnyType, BaseType.DecimalType)]
    [UsedImplicitly]
    internal class OpUnaryMinus : BinaryOperation
    {
        protected override Expression BinaryOperate(
            BinaryExpression e,
            Block block,
            IArithmeticOpMetadata operationParameters)
        {
            if (e.LeftHandSide.IsConst)
            {
                switch (e.LeftHandSide.TargetType.BaseType)
                {
                    case BaseType.IntType:
                        ConstantIntExpression leftInt = (ConstantIntExpression)e.LeftHandSide;
                        leftInt.Value = -(int)leftInt.Value;
                        return leftInt;

                    case BaseType.DecimalType:
                        ConstantDoubleExpression leftDouble = (ConstantDoubleExpression)e.LeftHandSide;
                        leftDouble.Value = -(decimal)leftDouble.Value;
                        return leftDouble;
                }
            }

            return e; // expression remains the same
        }
    }
}