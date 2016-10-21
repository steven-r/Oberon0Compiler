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

        public override string ToString()
        {
            return $"{Variable} := {Expression}";
        }
    }
}
