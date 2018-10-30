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
            if (value is string stringVal)
            { // from string
                if (int.TryParse(stringVal, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out int intVal))
                {
                    return new ConstantIntExpression(intVal);
                }

                if (decimal.TryParse(stringVal, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out decimal decimalVal))
                {
                    return new ConstantDoubleExpression(decimalVal);
                }

                if (bool.TryParse(stringVal, out bool boolVal))
                {
                    return new ConstantBoolExpression(boolVal);
                }

                throw new InvalidOperationException($"Unknown constant '{stringVal}'");
            }

            if (value is float || value is double || value is decimal)
            {
                return new ConstantDoubleExpression(Convert.ToDecimal(value));
            }

            return new ConstantIntExpression(Convert.ToInt32(value));
        }
    }
}