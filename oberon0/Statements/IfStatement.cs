using System.Collections.Generic;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Compiler.Statements
{
    public class IfStatement : BasicStatement
    {
        public List<Expression> Conditions { get; set; }

        public List<Block> ThenParts { get; set; }

        public Block ElsePart { get; set; }

        public IfStatement()
        {
            Conditions = new List<Expression>();
            ThenParts = new List<Block>();
        }
    }
}