
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Compiler.Solver
{
    internal interface ICalculatable
    {
        TokenType Operator { get; set; }

        /// <summary>
        /// Calculates this operation.
        /// </summary>
        Expression Calc(Block block);
    }
}