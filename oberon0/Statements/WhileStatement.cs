#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Compiler.Statements
{
    /// <summary>
    ///     handle WHILE
    /// </summary>
    /// <seealso cref="IStatement" />
    public class WhileStatement(Block parent) : IStatement
    {
        /// <summary>
        ///     Gets or sets the condition.
        /// </summary>
        /// <value>The condition.</value>
        public Expression Condition { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the block.
        /// </summary>
        /// <value>The block.</value>
        public Block Block { get; } = new(parent, parent.Module);
    }
}
