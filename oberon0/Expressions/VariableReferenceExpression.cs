#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Expressions
{
    public class VariableReferenceExpression : Expression
    {
        public required Declaration Declaration { get; init; }

        public VariableSelector? Selector { get; init; }

        public required string Name { get; init; }

        public static Expression? Create(Declaration? declaration, VariableSelector? s)
        {
            if (declaration == null)
            {
                return null;
            }

            if (declaration is ConstDeclaration c)
            {
                return c.Value;
            }

            var e = new VariableReferenceExpression
            {
                Name = declaration.Name,
                Declaration = declaration,
                TargetType = s?.SelectorResultType ?? declaration.Type,
                Selector = s
            };
            return e;
        }

        public override string ToString()
        {
            return $"{Name}({TargetType:G})";
        }
    }
}
