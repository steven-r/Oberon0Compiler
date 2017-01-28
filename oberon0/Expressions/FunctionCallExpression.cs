using System.Collections.Generic;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Expressions
{
    public class FunctionCallExpression: Expression
    {
        public FunctionCallExpression(FunctionDeclaration functionDeclaration, Block block, params Expression[] parameters)
        {
            FunctionDeclaration = functionDeclaration;
            Block = block;
            Parameters = new List<Expression>(parameters);
            TargetType = block.LookupTypeByBaseType(functionDeclaration.ReturnType.BaseType);
        }

        public FunctionDeclaration FunctionDeclaration { get; set; }

        public Block Block { get; set; }

        public List<Expression> Parameters { get; set; }
    }
}
