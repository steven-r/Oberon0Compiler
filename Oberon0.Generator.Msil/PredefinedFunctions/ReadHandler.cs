using System.Collections.Generic;
using System.ComponentModel.Composition;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Generator.Msil.PredefinedFunctions
{
    [Export(typeof(IStandardFunctionGenerator))]
    [StandardFunctionMetadata("ReadInt", "VOID", "&INTEGER")]
    [StandardFunctionMetadata("ReadBool", "VOID", "&BOOL")]
    public class ReadNumHandler : IStandardFunctionGenerator
    {
        public void Generate(IStandardFunctionMetadata metadata, CodeGenerator generator, FunctionDeclaration callExpression, List<Expression> parameters,
            Block block)
        {
            VariableReferenceExpression reference = (VariableReferenceExpression)parameters[0];

            generator.StartStoreVar(block, reference.Declaration, reference.Selector);
            generator.Code.WriteLine("\tcall string [mscorlib]System.Console::ReadLine()");
            if (callExpression.Name == "ReadInt")
                generator.Code.WriteLine("\tcall int32 [mscorlib]System.Int32::Parse(string)");
            if (callExpression.Name == "ReadBool")
                generator.Code.WriteLine("\tcall int32 [mscorlib]System.Bool::Parse(string)");
            generator.StoreVar(block, reference.Declaration, reference.Selector);
        }
    }
}
