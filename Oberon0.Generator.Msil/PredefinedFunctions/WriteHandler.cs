using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Generator.Msil.PredefinedFunctions
{
    [Export(typeof(IStandardFunctionGenerator))]
    [StandardFunctionMetadata("WriteInt", "VOID", "INTEGER")]
    [StandardFunctionMetadata("WriteBool", "VOID", "BOOLEAN")]
    [StandardFunctionMetadata("WriteReal", "VOID", "REAL")]
    public class WriteNumberHandler : IStandardFunctionGenerator
    {
        public void Generate(IStandardFunctionMetadata metadata, CodeGenerator generator, FunctionDeclaration callExpression, List<Expression> parameters,
            Block block)
        {
            ProcedureParameter parameter = callExpression.Block.Declarations.OfType<ProcedureParameter>().First();
            generator.Code.Emit("ldstr", "\"{0}\"");
            if (parameter.IsVar)
            {
                VariableReferenceExpression reference = (VariableReferenceExpression)parameters[0];
                generator.Load(block, reference.Declaration, reference.Selector);
            }
            else
            {
                generator.ExpressionCompiler(callExpression.Block.Parent, parameters[0]);
            }
            generator.Code.Emit("box", Code.GetTypeName(parameter.Type.BaseType));
            generator.Code.Emit("call", "void", "[mscorlib]System.Console::Write(string, object)");
        }
    }

    [Export(typeof(IStandardFunctionGenerator))]
    [StandardFunctionMetadata("WriteString", "VOID", "STRING")]
    public class WriteStringHandler : IStandardFunctionGenerator
    {
        public void Generate(IStandardFunctionMetadata metadata, CodeGenerator generator, FunctionDeclaration callExpression, List<Expression> parameters,
            Block block)
        {
            generator.ExpressionCompiler(block, parameters[0]);
            generator.Code.Emit("call", "void", "[mscorlib]System.Console::Write(string)");
        }
    }

    [Export(typeof(IStandardFunctionGenerator))]
    [StandardFunctionMetadata("WriteLn", "VOID")]
    public class WriteLnHandler : IStandardFunctionGenerator
    {
        public void Generate(IStandardFunctionMetadata metadata, CodeGenerator generator, FunctionDeclaration callExpression, List<Expression> parameters,
            Block block)
        {
            generator.Code.Emit("call", "void", "[mscorlib]System.Console::WriteLine()");
        }
    }
}
