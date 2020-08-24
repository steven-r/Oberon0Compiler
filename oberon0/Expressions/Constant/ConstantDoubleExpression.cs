#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using JetBrains.Annotations;
using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions
{
    public class ConstantDoubleExpression : ConstantExpression
    {
        static ConstantDoubleExpression()
        {
            Zero = new ConstantDoubleExpression(0);
        }

        public ConstantDoubleExpression(double value)
            : base(SimpleTypeDefinition.RealType, value)
        {
        }

        /// <summary>
        ///     Gets the standard Constant to represent a zero constant
        /// </summary>
        /// <value>The zero.</value>
        [NotNull]
        public static ConstantDoubleExpression Zero { get; }

        public override string ToString()
        {
            return ((double) Value).ToString("G");
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
            return (double) Value != 0;
        }
    }
}
