﻿using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Compiler.Statements
{
    public class RepeatStatement : BasicStatement
    {
        public RepeatStatement(Block parent)
        {
            Block = new Block {Parent = parent};
        }

        public Expression Condition { get; set; }

        public Block Block { get; set; }
    }
}