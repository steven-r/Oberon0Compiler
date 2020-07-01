using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;
using System.Collections.Generic;

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