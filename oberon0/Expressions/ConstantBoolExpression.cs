using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions
{
    public class ConstantBoolExpression : ConstantExpression
    {
        public ConstantBoolExpression(bool value)
            : base(SimpleTypeDefinition.BoolType, value)
        {
        }
    }
}