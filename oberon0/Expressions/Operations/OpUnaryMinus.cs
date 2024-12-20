﻿#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Exceptions;
using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Expressions.Operations.Internal;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions.Operations
{
    /// <summary>
    ///     Handle "~".
    /// </summary>
    /// <seealso cref="IArithmeticOperation" />
    /// <remarks>This function is some kind of exception as - usually takes one parameter. The second is handled as a dummy</remarks>
    [ArithmeticOperation(OberonGrammarLexer.MINUS, BaseTypes.Int, BaseTypes.Any, BaseTypes.Int)]
    [ArithmeticOperation(OberonGrammarLexer.MINUS, BaseTypes.Real, BaseTypes.Any, BaseTypes.Real)]
    // ReSharper disable once UnusedMember.Global
    internal class OpUnaryMinus : BinaryOperation
    {
        protected override Expression BinaryOperate(
            BinaryExpression bin,
            Block block,
            IArithmeticOpMetadata operationParameters)
        {
            if (!bin.LeftHandSide.IsConst)
            {
                return bin; // expression remains the same
            }

            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (bin.LeftHandSide.TargetType.Type)
            {
                case BaseTypes.Int:
                {
                    var leftInt = (ConstantIntExpression) bin.LeftHandSide;
                    leftInt.Value = -(int) leftInt.Value;
                    return leftInt;
                }
                case BaseTypes.Real:
                {
                    var leftDouble = (ConstantDoubleExpression) bin.LeftHandSide;
                    leftDouble.Value = -(double) leftDouble.Value;
                    if (!leftDouble.MightBeInt)
                    {
                        return leftDouble;
                    }

                    try
                    {
                        int intVal = Convert.ToInt32((double)leftDouble.Value);
                        return new ConstantIntExpression(intVal); // converted to integer
                    }
                    catch (OverflowException)
                    {
                        // ignore value at it is too large
                    }
                    return leftDouble;
                }
                default:
                    throw new InternalCompilerException($"OpUnaryMinus on wrong type {bin.LeftHandSide.TargetType.Type:G}");
            }
        }
    }
}
