using System.Collections.Generic;
using JetBrains.Annotations;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Compiler.Statements
{
    public class IfStatement : BasicStatement
    {
        public IfStatement(Block parent)
        {
            ParentBlock = parent;
            Conditions = new List<Expression>();
            ThenParts = new List<Block>();
        }

        private Block ParentBlock { [UsedImplicitly] get; set; }

        public List<Expression> Conditions { get; }

        public List<Block> ThenParts { get; }

        public Block ElsePart { get; set; }
    }
}