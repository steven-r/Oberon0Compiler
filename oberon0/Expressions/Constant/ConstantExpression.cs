#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Globalization;

namespace Oberon0.Compiler.Expressions.Constant
{
    public abstract class ConstantExpression(object value) : Expression
    {
        public object Value { get; set; } = value;

        public override bool IsConst => !Locked;
        
        /// <summary>
        /// If set, this property has a special meaning to be interpreted by the generator, if needed.
        /// </summary>
        public bool Internal { get; set; }

        /// <summary>
        /// If set, this constant cannot be used as part of solving.
        /// </summary>
        public bool Locked { get; set; }


        /// <summary>
        /// Return the value as an integer, if possible
        /// </summary>
        /// <returns>The integer representation of the value.</returns>
        /// <exception cref="NotImplementedException">Thrown if there's no integer representation of the value</exception>
        public abstract int ToInt32();

        /// <summary>
        /// Return the value as a double value, if possible
        /// </summary>
        /// <returns>The double representation of the value.</returns>
        /// <exception cref="NotImplementedException">Thrown if there's no double representation of the value</exception>
        public abstract double ToDouble();

        /// <summary>
        /// Return the value as a boolean value, if possible
        /// </summary>
        /// <returns>The boolean representation of the value.</returns>
        /// <exception cref="NotImplementedException">Thrown if there's no boolean representation of the value</exception>
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

                if (double.TryParse(stringVal, CultureInfo.InvariantCulture, out double doubleVal))
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
