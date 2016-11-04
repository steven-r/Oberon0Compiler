using System.Collections.Generic;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Generator.Msil.PredefinedFunctions
{
    /// <summary>
    /// Interface used to generate code for a specific standard function
    /// </summary>
    public interface IStandardFunctionGenerator
    {
        /// <summary>
        /// Generates code for the given function/procedure
        /// </summary>
        /// <param name="metadata">The function metadata.</param>
        /// <param name="generator">The generator.</param>
        /// <param name="functionDeclaration"></param>
        /// <param name="parameters"></param>
        /// <param name="block">The block.</param>
        void Generate(IStandardFunctionMetadata metadata, CodeGenerator generator, FunctionDeclaration functionDeclaration, List<Expression> parameters,  Block block);
    }
}
