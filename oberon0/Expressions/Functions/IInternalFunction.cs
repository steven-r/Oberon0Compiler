#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Expressions.Functions;

/// <summary>
///     Implementations of this interface are responsible to convert data from type a to b.
/// </summary>
public interface IInternalFunction
{
    /// <summary>
    /// Operate on a function call that can be handled internally based
    /// on the parameters given in <see cref="InternalFunctionMetadata" />.
    /// </summary>
    /// <param name="e">The expression to operate on</param>
    /// <param name="block">The block to operate on</param>
    /// <param name="functionMetadata">The function to work on</param>
    /// <returns>The expression based on target type.</returns>
    Expression Operate(
        FunctionCallExpression e, Block block,
        InternalFunctionMetadata functionMetadata);
}