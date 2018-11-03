#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstDeclaration.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/ConstDeclaration.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Definitions
{
    using Oberon0.Compiler.Expressions.Constant;
    using Oberon0.Compiler.Types;

    public class ConstDeclaration : Declaration
    {
        public ConstDeclaration(string name, TypeDefinition type, ConstantExpression value)
            : base(name, type)
        {
            this.Value = value;
        }

        public ConstDeclaration(string name, TypeDefinition type, ConstantExpression value, Block block)
            : base(name, type, block)
        {
            this.Value = value;
        }

        public ConstantExpression Value { get; }

        public override string ToString()
        {
            return $"Const {this.Name} = {this.Value}";
        }
    }
}