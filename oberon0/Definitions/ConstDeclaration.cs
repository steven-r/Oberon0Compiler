using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Compiler.Definitions
{
    public class ConstDeclaration: Declaration
    {
        public ConstantExpression Value { get; set; }

        public ConstDeclaration(string name, TypeDefinition type, ConstantExpression value = null, Block block = null) 
            : base(name, type, block)
        {
            Value = value;
        }
    }
}
