using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oberon0.Compiler.Expressions
{
    class UnaryExpression: BinaryExpression
    {
        /// <summary>
        /// For unary expressions this value is true.
        /// </summary>
        /// <value><c>true</c> if this instance is unary; otherwise, <c>false</c>.</value>
        public override bool IsUnary => true;
    }
}
