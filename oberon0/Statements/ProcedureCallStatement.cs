#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Compiler.Statements
{
    public class ProcedureCallStatement : IStatement
    {
        public ProcedureCallStatement(FunctionDeclaration functionDeclaration, List<Expression> parameters)
        {
            FunctionDeclaration = functionDeclaration;
            Parameters = parameters;
        }

        public List<Expression> Parameters { get; }

        public FunctionDeclaration FunctionDeclaration { get; }
    }
}
