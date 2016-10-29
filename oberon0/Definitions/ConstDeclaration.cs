using Oberon0.Compiler.Expressions;

namespace Oberon0.Compiler.Definitions
{
    public class ConstDeclaration: Declaration
    {
        public ConstantExpression Value { get; }

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

        public override string ToString()
        {
            return $"Const {Name} = {Value}";
        }
    }
}
