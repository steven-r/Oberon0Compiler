#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Compiler.Statements
{
    public class IfStatement : IStatement
    {
        /// <summary>
        ///     Gets the conditions.
        /// </summary>
        public List<Expression> Conditions { get; } = [];

        /// <summary>
        ///     Gets or sets the else part.
        /// </summary>
        public Block? ElsePart { get; set; }

        /// <summary>
        ///     Gets the then parts.
        /// </summary>
        public List<Block> ThenParts { get; } = [];
    }
}
