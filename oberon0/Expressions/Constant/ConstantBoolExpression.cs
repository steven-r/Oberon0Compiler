#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions.Constant
{
    public class ConstantBoolExpression : ConstantExpression
    {
        public ConstantBoolExpression(bool value)
            : base(SimpleTypeDefinition.BoolType, value)
        {
        }

        public override int ToInt32()
        {
            throw new NotImplementedException();
        }

        public override double ToDouble()
        {
            throw new NotImplementedException();
        }

        public override bool ToBool()
        {
            return (bool) Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
