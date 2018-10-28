#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Oberon0Compiler.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/Oberon0Compiler.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler
{
    using Antlr4.Runtime;

    using Oberon0.Compiler.Definitions;

    public static class Oberon0Compiler
    {
        public static Module CompileString(string source)
        {
            AntlrInputStream input = new AntlrInputStream(source);
            OberonGrammarLexer lexer = new OberonGrammarLexer(input);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            OberonGrammarParser parser = new OberonGrammarParser(tokens);
            parser.AddParseListener(new Oberon0CompilerListener(parser));
            OberonGrammarParser.ModuleContext context = parser.module();
            return context.modres;
        }
    }
}