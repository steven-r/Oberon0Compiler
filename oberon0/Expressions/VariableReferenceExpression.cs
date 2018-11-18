﻿#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VariableReferenceExpression.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/VariableReferenceExpression.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Expressions
{
    using System.Linq;

    using Oberon0.Compiler.Definitions;

    public class VariableReferenceExpression : Expression
    {
        public Declaration Declaration { get; private set; }

        public VariableSelector Selector { get; private set; }

        private string Name { get; set; }

        public static Expression Create(Block block, Declaration declaration, VariableSelector s)
        {
            if (declaration == null) return null;

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
            return $"{this.Name}({this.TargetType:G})";
        }
    }
}