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
    public class ConstantIntExpression : ConstantExpression
    {
        static ConstantIntExpression()
        {
            Zero = new ConstantIntExpression(0);
        }

        public ConstantIntExpression(int value)
            : base(value)
        {
            TargetType = SimpleTypeDefinition.IntType;
            Value = value;
        }

        /// <summary>
        ///     Gets standard Constant to represent a zero constant
        /// </summary>
        /// <value>The zero.</value>
        public static ConstantIntExpression Zero { get; }

        public override string ToString()
        {
            return ((int) Value).ToString("G", CultureInfo.InvariantCulture);
        }

        public override int ToInt32()
        {
            return (int) Value;
        }

        public override double ToDouble()
        {
            return Convert.ToDouble(Value);
        }

        public override bool ToBool()
        {
            return (int) Value != 0;
        }
    }
}
