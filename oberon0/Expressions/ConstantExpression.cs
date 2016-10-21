using System;
using System.Globalization;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Expressions
{
    public abstract class ConstantExpression : Expression
    {
        protected ConstantExpression(BaseType baseType)
        {
            TargetType = baseType;
        }

        /// <summary>
        /// Constant expressions are const by default
        /// </summary>
        /// <value><c>true</c> if this instance is constant; otherwise, <c>false</c>.</value>
        public override bool IsConst => true;

        private ConstantExpression() {}

        internal static Expression Create(object value)
        {
            string sVal = value as string;
            double dValue;
            bool isFloat;
            if (sVal != null)
            { // from string
                dValue = double.Parse(sVal, CultureInfo.InvariantCulture);
                isFloat = sVal.IndexOf('.') >= 0;
            }
            else
            {
                isFloat = value is double || value is float || value is decimal;
                dValue = Convert.ToDouble(value);
            }

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (!isFloat)
            {
                return new ConstantIntExpression(Convert.ToInt32(value));
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

        public bool ToBool()
        {
            var bval = this as ConstantBoolExpression;
            if (bval != null) return bval.Value;
            throw new InvalidOperationException("Value is not of type bool");
        }
    }
}