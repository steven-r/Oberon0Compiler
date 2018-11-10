#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstantDoubleExpression.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/ConstantDoubleExpression.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Expressions
{
    using System;

    using JetBrains.Annotations;

    using Oberon0.Compiler.Expressions.Constant;
    using Oberon0.Compiler.Types;

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
        /// Gets the standard Constant to represent a zero constant
        /// </summary>
        /// <value>The zero.</value>
        [NotNull]
        public static ConstantDoubleExpression Zero { get; }

        public override string ToString()
        {
            return ((double)Value).ToString("G");
        }

        public override int ToInt32()
        {
            return Convert.ToInt32(Value);
        }

        public override double ToDouble()
        {
            return (double)Value;
        }

        public override bool ToBool()
        {
            return (double)Value != 0;
        }
    }
}