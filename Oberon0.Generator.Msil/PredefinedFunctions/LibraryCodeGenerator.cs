using System;
using System.Collections.Generic;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Generator.Msil.PredefinedFunctions
{
    /// <summary>
    /// Generate code to call a standard function from a library
    /// </summary>
    /// <seealso cref="Oberon0.Generator.Msil.PredefinedFunctions.IStandardFunctionGenerator" />
    internal class LibraryCodeGenerator: IStandardFunctionGenerator
    {
        private static LibraryCodeGenerator _instance;

        internal static LibraryCodeGenerator Instance => _instance ?? (_instance = new LibraryCodeGenerator());

        public void Generate(IStandardFunctionMetadata metadata, CodeGenerator generator, FunctionDeclaration functionDeclaration,
            List<Expression> parameters, Block block)
        {
            throw new NotImplementedException();
        }
    }
}