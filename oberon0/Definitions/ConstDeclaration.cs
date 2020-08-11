#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Definitions
{
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