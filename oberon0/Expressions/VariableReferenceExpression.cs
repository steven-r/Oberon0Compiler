#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Exceptions;

namespace Oberon0.Compiler.Expressions
{
    public class VariableReferenceExpression : Expression
    {
        public required Declaration Declaration { get; init; }

        public VariableSelector? Selector { get; init; }

        /// <summary>
        /// The name of the variable
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Create a new variable reference expression based on a given declaration
        /// and selector element for the variable defined.
        /// </summary>
        /// <param name="declaration">The variable declaration</param>
        /// <param name="s">if not null, a selector (or Array or Record declarations)</param>
        /// <returns>A VariableReferenceExpression</returns>
        public static Expression Create(Declaration? declaration, VariableSelector? s)
        {
            switch (declaration)
            {
                case null:
                    throw new InternalCompilerException("Null declaration is not allowed here!");
                case ConstDeclaration { Value.IsConst: true } c:
                    return c.Value;
                default:
                {
                    var e = new VariableReferenceExpression
                    {
                        Name = declaration.Name,
                        Declaration = declaration,
                        TargetType = s?.SelectorResultType ?? declaration.Type,
                        Selector = s
                    };
                    return e;
                }
            }
        }

        public override string ToString()
        {
            return $"{Name}({TargetType:G})";
        }
    }
}
