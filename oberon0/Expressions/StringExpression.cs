namespace Oberon0.Compiler.Expressions
{
    public class StringExpression: Expression
    {
        public string Value { get; set; }

        public StringExpression(string value)
        {
            Value = value;
        }
    }
}