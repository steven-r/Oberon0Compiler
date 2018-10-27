#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstantExpression{T}.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/ConstantExpression{T}.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Expressions.Constant
{
    using System;
    using System.Globalization;

    using Oberon0.Compiler.Types;

    public abstract class ConstantExpression : Expression
    {
        protected ConstantExpression(TypeDefinition baseType, object value)
            : base(baseType)
        {
            Value = value;
        }

        public object Value { get; set; }

        public override bool IsConst => true;

        public abstract int ToInt32();

        public abstract decimal ToDouble();

        public abstract bool ToBool();

        internal static Expression Create(object value)
        {
            var stringVal = value as string;
            decimal decimalVal;
            bool isFloat;
            if (stringVal != null)
            { // from string
                decimalVal = decimal.Parse(stringVal, CultureInfo.InvariantCulture);
                isFloat = stringVal.IndexOf('.') >= 0;
            }
            else
            {
                isFloat = value is double || value is float || value is decimal;
                decimalVal = Convert.ToDecimal(value);
            }

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (!isFloat)
                return new ConstantIntExpression(Convert.ToInt32(value));
            return new ConstantDoubleExpression(decimalVal);
        }
    }
}