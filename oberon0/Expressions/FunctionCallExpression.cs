#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using Antlr4.Runtime;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Expressions
{
    public class FunctionCallExpression : Expression
    {
#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public FunctionCallExpression(
            FunctionDeclaration functionDeclaration,
            Block block,
            IToken token,
            params Expression[] parameters)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            this.FunctionDeclaration = functionDeclaration;
            this.Block = block;
            this.Parameters = new List<Expression>(parameters);
            this.TargetType = block.LookupTypeByBaseType(functionDeclaration.ReturnType.Type);
            this.Token = token;
        }

        public Block Block { get; set; }

        public FunctionDeclaration FunctionDeclaration { get; set; }

        public List<Expression> Parameters { get; set; }
    }
}