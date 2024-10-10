#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Expressions
{
    public class FunctionCallExpression : Expression
    {
        public FunctionCallExpression(
            FunctionDeclaration functionDeclaration,
            Block block,
            params Expression[] parameters)
        {
            FunctionDeclaration = functionDeclaration;
            Parameters = [..parameters];
            TargetType = block.LookupTypeByBaseType(functionDeclaration.ReturnType.Type);
        }

        public FunctionDeclaration FunctionDeclaration { get; }

        public List<Expression> Parameters { get; }
    }
}
