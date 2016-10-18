using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Expressions
{
    public abstract class Expression
    {

        public BaseType TargetType { get; set; }

        internal static Expression CreateUnary(TokenType op, Expression operand)
        {
            return new UnaryExpression {Operator = op, Operand = operand};
        }
    }
}
