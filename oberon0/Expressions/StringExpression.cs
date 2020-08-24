#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions
{
    public class StringExpression : Expression
    {
        public StringExpression(string value)
        {
            TargetType = SimpleTypeDefinition.StringType;
            Value = value;
        }

        public string Value { get; }

        public override bool IsConst => true;
    }
}
