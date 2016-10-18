using System;
using Loyc;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Solver;

namespace Oberon0.Compiler.Expressions
{
    abstract class ConstantExpression : Expression, ICalculatable
    {
        public BaseType BaseType { get; set; }
        public TokenType Operator { get; set; }

        protected ConstantExpression(BaseType baseType)
        {
            BaseType = TargetType = baseType;
        }

        private ConstantExpression() {}

        internal static Expression Create(Token t)
        {
            if (t.Type != TokenType.Num) throw new LogException(null, "Number expected");
            double val = (double) t.Value;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if ((val%1) == 0)
            {
                return new ConstantIntExpression(Convert.ToInt32(val));
            }
            else
            {
                return new ConstantDoubleExpression(val);
            }
        }

        public int ToInt32()
        {
            var iconst = this as ConstantIntExpression;
            if (iconst == null)
            {
                throw new InvalidOperationException("Value is not of type int32");
            }
            return iconst.Value;
        }

        public double ToDouble()
        {
            var dconst = this as ConstantDoubleExpression;
            if (dconst == null)
            {
                if (this is ConstantIntExpression)
                    return ToInt32();
                throw new InvalidOperationException("Value is not of type int32");
            }
            return dconst.Value;
        }

        public abstract Expression Calc(Block block);
    }
}