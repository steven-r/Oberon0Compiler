using JetBrains.Annotations;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions
{
    public class ConstantDoubleExpression : ConstantExpression
    {
        public ConstantDoubleExpression(double value)
            : base(SimpleTypeDefinition.RealType, value)
        {
        }

        public override string ToString()
        {
            return ((double)Value).ToString("G");
        }


        static ConstantDoubleExpression()
        {
            Zero = new ConstantDoubleExpression(0);
        }

        /// <summary>
        /// Standard Constant to represent a zero constant
        /// </summary>
        /// <value>The zero.</value>
        [NotNull]
        public static ConstantDoubleExpression Zero { get; }

    }
}