using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Expressions.Operations.Internal
{
    /// <summary>
    /// Implementations of this interface are responsible to convert data from type a to b.
    /// </summary>
    internal interface IArithmeticOperation
    {
        /// <summary>
        /// Operate on an expression based on the parameters given in <see cref="IArithmeticOpMetadata"/>. 
        /// </summary>
        /// <param name="e">The expression to operate on</param>
        /// <param name="block">The block to operate on</param>
        /// <param name="operationParameters">The operation to work on</param>
        /// <returns>The expression based on target type.</returns>
        Expression Operate(Expression e, Block block, IArithmeticOpMetadata operationParameters);
    }
}