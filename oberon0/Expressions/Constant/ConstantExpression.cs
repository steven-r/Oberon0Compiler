#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Globalization;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions.Constant
{
    public abstract class ConstantExpression(object value) : Expression
    {
        public object Value { get; set; } = value;

        public override bool IsConst => true;

        public abstract int ToInt32();

        public abstract double ToDouble();

        public abstract bool ToBool();

        internal static Expression Create(object value, bool expectInt = false)
        {
            if (value is string stringVal)
            {
                // from string
                if (uint.TryParse(stringVal, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture,
                    out uint intVal))
                {
                    if (intVal > int.MaxValue && expectInt)
                    {
                        // might be int.MinValue, else real
                        return new ConstantDoubleExpression(intVal, true);
                    }
                    return new ConstantIntExpression((int)intVal);
                }

                if (double.TryParse(stringVal, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                    CultureInfo.InvariantCulture, out double doubleVal))
                {
                    return new ConstantDoubleExpression(doubleVal);
                }

                if (bool.TryParse(stringVal, out bool boolVal))
                {
                    return new ConstantBoolExpression(boolVal);
                }

                throw new InvalidOperationException($"Unknown constant '{stringVal}'");
            }

            if (value is float or double or decimal)
            {
                return new ConstantDoubleExpression(Convert.ToDouble(value));
            }

            if (value is bool)
            {
                return new ConstantBoolExpression(Convert.ToBoolean(value));
            }

            return new ConstantIntExpression(Convert.ToInt32(value));
        }
    }
}
