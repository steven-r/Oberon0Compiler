using System.Collections.Generic;
using JetBrains.Annotations;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Compiler.Statements
{
    public class ProcedureCallStatement : BasicStatement
    {
        public ProcedureCallStatement(FunctionDeclaration functionDeclaration, Block block, List<Expression> parameters)
        {
            Block = block;
            FunctionDeclaration = functionDeclaration;
            Parameters = parameters;
        }

        public List<Expression> Parameters { get; }

        public Block Block { [UsedImplicitly] get; set; }

        public FunctionDeclaration FunctionDeclaration { get; }
    }
}