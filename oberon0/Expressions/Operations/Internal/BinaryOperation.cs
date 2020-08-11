﻿#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using JetBrains.Annotations;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Expressions.Operations.Internal
{
    internal abstract class BinaryOperation : IArithmeticOperation
    {
        /// <inheritdoc />
        public Expression Operate(Expression e, Block block, IArithmeticOpMetadata operationParameters)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (!(e is BinaryExpression bin)) throw new InvalidCastException("Cannot cast expression to binary expression");
            return BinaryOperate(bin, block, operationParameters);
        }

        /// <summary>
        /// Run the binary operation
        /// </summary>
        /// <param name="bin">
        /// The bin.
        /// </param>
        /// <param name="block">
        /// The block.
        /// </param>
        /// <param name="operationParameters">
        /// The operation parameters.
        /// </param>
        /// <returns>
        /// The <see cref="Expression"/>.
        /// </returns>
        protected abstract Expression BinaryOperate(
            BinaryExpression bin,
            [UsedImplicitly] Block block,
            IArithmeticOpMetadata operationParameters);
    }
}