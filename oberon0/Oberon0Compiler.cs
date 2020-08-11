#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Antlr4.Runtime;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler
{
    /// <summary>
    /// The Oberon0 compiler core class.
    /// </summary>
    public class Oberon0Compiler
    {
#pragma warning disable CS3003 // Type is not CLS-compliant
        public OberonGrammarParser.ModuleDefinitionContext Context { get; set; }
#pragma warning restore CS3003 // Type is not CLS-compliant

#pragma warning disable CS3003 // Type is not CLS-compliant
        public OberonGrammarLexer Lexer { get; private set; }
#pragma warning restore CS3003 // Type is not CLS-compliant

#pragma warning disable CS3003 // Type is not CLS-compliant
        public OberonGrammarParser Parser { get; private set; }
#pragma warning restore CS3003 // Type is not CLS-compliant

        /// <summary>
        /// Gets or sets a value indicating whether the module has at least one error.
        /// </summary>
        public bool HasError { get; private set; }

        /// <summary>
        /// Compile some source code
        /// </summary>
        /// <param name="source">The source file/code to compile</param>
        /// <param name="options">Options which can be set (see <see cref="Oberon0CompilerOptions"/> for details)</param>
        /// <returns>A compiled module containing the AST of the source file</returns>
        public static Module CompileString(string source, Oberon0CompilerOptions options = null)
        {
            var instance = new Oberon0Compiler();

            AntlrInputStream input = new AntlrInputStream(source);
            instance.Lexer = new OberonGrammarLexer(input);
            CommonTokenStream tokens = new CommonTokenStream(instance.Lexer);

            options?.InitLexer?.Invoke(instance.Lexer);

            instance.Parser = new OberonGrammarParser(tokens) {module = {CompilerInstance = instance}};
            instance.Parser.AddParseListener(new Oberon0CompilerListener(instance.Parser));

            options?.InitParser?.Invoke(instance.Parser);

            instance.Context = instance.Parser.moduleDefinition();

            instance.HasError = instance.Parser.NumberOfSyntaxErrors > 0;

            options?.AfterCompile?.Invoke(instance);

            return instance.Parser.module;
        }
    }
}