using System.Collections.Generic;
using JetBrains.Annotations;
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