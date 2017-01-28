using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions
{
    public class StringExpression: Expression
    {
        public StringExpression(string value)
        {
            TargetType = SimpleTypeDefinition.StringType;
            Value = value;
        }

        public string Value { get; }

        public override bool IsConst => true;
    }
}