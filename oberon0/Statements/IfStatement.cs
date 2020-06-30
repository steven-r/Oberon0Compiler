#region copyright

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfStatement.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/IfStatement.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#endregion copyright

namespace Oberon0.Compiler.Statements
{
    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions;
    using System.Collections.Generic;

    public class IfStatement : IStatement
    {
        public IfStatement()
        {
            this.Conditions = new List<Expression>();
            this.ThenParts = new List<Block>();
        }

        /// <summary>
        /// Gets the conditions.
        /// </summary>
        public List<Expression> Conditions { get; }

        /// <summary>
        /// Gets or sets the else part.
        /// </summary>
        public Block ElsePart { get; set; }

        /// <summary>
        /// Gets the then parts.
        /// </summary>
        public List<Block> ThenParts { get; }
    }
}