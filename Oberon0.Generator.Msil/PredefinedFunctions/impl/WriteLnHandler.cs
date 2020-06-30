namespace Oberon0.Generator.Msil.PredefinedFunctions
{
    using JetBrains.Annotations;
    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions;
    using Oberon0.Compiler.Types;
    using System.Collections.Generic;

    [StandardFunctionMetadata("WriteLn", TypeDefinition.VoidTypeName)]
    [UsedImplicitly]
    public class WriteLnHandler : IStandardFunctionGenerator
    {
        public void Generate(
            IStandardFunctionMetadata metadata,
            CodeGenerator generator,
            FunctionDeclaration functionDeclaration,
            IReadOnlyList<Expression> parameters,
            Block block)
        {
            generator.Code.Emit("call", "void", "[mscorlib]System.Console::WriteLine()");
        }
    }
}