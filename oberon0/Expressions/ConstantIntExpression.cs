using JetBrains.Annotations;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Expressions
{
    public class ConstantIntExpression : ConstantExpression
    {
        public ConstantIntExpression(int value)
            : base(BaseType.IntType, value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return ((int)Value).ToString("G");
        }

        static ConstantIntExpression()
        {
            Zero = new ConstantIntExpression(0);
        }

        /// <summary>
        /// Standard Constant to represent a zero constant
        /// </summary>
        /// <value>The zero.</value>
        [NotNull]
        public static ConstantIntExpression Zero { get; }
    }
}