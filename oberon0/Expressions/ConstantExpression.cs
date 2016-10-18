using System;
using System.Globalization;
using Loyc;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Solver;

namespace Oberon0.Compiler.Expressions
{
    public abstract class ConstantExpression : Expression, ICalculatable
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
            string sVal = t.Value as string;
            double dValue;
            bool isFloat;
            if (sVal != null)
            { // from string
                dValue = double.Parse(sVal, CultureInfo.InvariantCulture);
                isFloat = sVal.IndexOf('.') >= 0;
            }
            else
            {
                isFloat = t.Value is double || t.Value is float || t.Value is decimal;
                dValue = Convert.ToDouble(t.Value);
            }

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (!isFloat)
            {
                return new ConstantIntExpression(Convert.ToInt32(t.Value));
            }
            else
            {
                return new ConstantDoubleExpression(dValue);
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