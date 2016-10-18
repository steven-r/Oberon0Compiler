using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Compiler.Statements
{
    public class AssignmentStatement : BasicStatement
    {
        public Declaration Variable { get; set; }

        public Expression Expression { get; set; }
    }

    public class WhileStatement : BasicStatement
    {
        public Expression Condition { get; set; }

        public Block Block { get; set; }
    }

    public class RepeatStatement : BasicStatement
    {
        public Expression Condition { get; set; }

        public Block Block { get; set; }
    }

}
