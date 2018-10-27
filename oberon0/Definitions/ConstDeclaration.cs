using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Definitions
{
    using Oberon0.Compiler.Expressions.Constant;

    public class ConstDeclaration : Declaration
    {
        public ConstDeclaration(string name, TypeDefinition type) 
            : this(name, type, null, null)
        {
        }

        public ConstDeclaration(string name, TypeDefinition type, Block block) 
            : this(name, type, null, block)
        {
        }

        public ConstDeclaration(string name, TypeDefinition type, ConstantExpression value)
            : base(name, type)
        {
            Value = value;
        }

        public ConstDeclaration(string name, TypeDefinition type, ConstantExpression value, Block block) 
            : base(name, type, block)
        {
            Value = value;
        }

        public ConstantExpression Value { get; }

        public override string ToString()
        {
            return $"Const {Name} = {Value}";
        }
    }
}
