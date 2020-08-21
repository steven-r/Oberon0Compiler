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
    public class RepeatStatement : IStatement
    {
        public RepeatStatement(Block parent)
        {
            Block = new Block(parent, parent.Module);
        }

        public Expression Condition { get; set; }

        public Block Block { get; set; }
    }
}
