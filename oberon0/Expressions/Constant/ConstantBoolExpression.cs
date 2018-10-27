#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstantBoolExpression.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/ConstantBoolExpression.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Expressions.Constant
{
    using Oberon0.Compiler.Types;

    public class ConstantBoolExpression : ConstantExpression
    {
        public ConstantBoolExpression(bool value)
            : base(SimpleTypeDefinition.BoolType, value)
        {
        }

        public override int ToInt32()
        {
            throw new System.NotImplementedException();
        }

        public override decimal ToDouble()
        {
            throw new System.NotImplementedException();
        }

        public override bool ToBool()
        {
            return (bool)Value;
        }
    }
}