namespace Oberon0.Compiler.Expressions
{
    public class StringExpression: Expression
    {
        public StringExpression(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }
}