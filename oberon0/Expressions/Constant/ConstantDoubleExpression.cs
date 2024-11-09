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
    public class ConstantDoubleExpression : ConstantExpression
    {
        public bool MightBeInt { get; }

        public ConstantDoubleExpression(double value, bool mightBeInt = false)
            : base(value)
        {
            MightBeInt = mightBeInt;
            TargetType = SimpleTypeDefinition.RealType;
        }

        static ConstantDoubleExpression()
        {
            Zero = new ConstantDoubleExpression(0);
        }

        /// <summary>
        ///     Gets the standard Constant to represent a zero constant
        /// </summary>
        /// <value>The zero.</value>
        public static ConstantDoubleExpression Zero { get; }

        public override string ToString()
        {
            return ((double) Value).ToString("G", CultureInfo.InvariantCulture);
        }

        public override int ToInt32()
        {
            return Convert.ToInt32(Value);
        }

        public override double ToDouble()
        {
            return (double) Value;
        }

        public override bool ToBool()
        {
            return !((double) Value).Equals(0.0);
        }
    }
}
