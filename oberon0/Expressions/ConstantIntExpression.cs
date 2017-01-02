using JetBrains.Annotations;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions
{
    public class ConstantIntExpression : ConstantExpression
    {
        public ConstantIntExpression(int value)
            : base(SimpleTypeDefinition.IntType, value)
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