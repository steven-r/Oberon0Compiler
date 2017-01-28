using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler
{
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