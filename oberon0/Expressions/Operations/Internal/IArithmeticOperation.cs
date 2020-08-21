#region copyright

// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------

#endregion

using JetBrains.Annotations;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Expressions.Operations.Internal
{
    /// <summary>
    ///     Implementations of this interface are responsible to convert data from type a to b.
    /// </summary>
    internal interface IArithmeticOperation
    {
        /// <summary>
        ///     Operate on an expression based on the parameters given in <see cref="IArithmeticOpMetadata" />.
        /// </summary>
        /// <param name="e">The expression to operate on</param>
        /// <param name="block">The block to operate on</param>
        /// <param name="operationParameters">The operation to work on</param>
        /// <returns>The expression based on target type.</returns>
        Expression Operate([NotNull] BinaryExpression e, [NotNull] Block block,
                           IArithmeticOpMetadata operationParameters);
    }
}
