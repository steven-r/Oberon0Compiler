// ***********************************************************************
// Assembly         : Oberon0.Generator.Msil
// Author           : stephen@stephenreindl.net
// Created          : 10-23-2016
//
// Last Modified By : stephen@stephenreindl.net
// Last Modified On : 01-08-2017
// ***********************************************************************
// <copyright file="CodeGenerator.Declarations.cs" company="Reindl IT">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Linq;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Types;

namespace Oberon0.Generator.Msil
{
    public partial class CodeGenerator
    {
        private void ProcessDeclarations(Block block, bool isRoot = false)
        {
            GenerateTypeDeclarations(block);
            int id = 0;
            bool isFirst = true;
            foreach (Declaration declaration in block.Declarations)
            {
                var pp = declaration as ProcedureParameter;
                if (pp != null)
                {
                    // skip procedure parameters
                    continue;
                }

                var c = declaration as ConstDeclaration;
                if (c != null)
                {
                    Code.ConstField(c);
                    continue;
                }

                declaration.GeneratorInfo = new DeclarationGeneratorInfo(id++);
                if (isRoot)
                {
                    Code.DataField(declaration, true);
                }
                else
                {
                    if (isFirst)
                    {
                        Code.Write("\t.locals (");
                        isFirst = false;
                    }
                    else
                    {
                        Code.Write(", ");
                    }
                    Code.LocalVarDef(declaration, false);
                }
            }
            if (!isFirst)
                Code.WriteLine(")");
        }
        private void GenerateTypeDeclarations(Block block)
        {
            foreach (var typeDefinition in block.Types.Where(x => x is RecordTypeDefinition))
            {
                var recordType = (RecordTypeDefinition)typeDefinition;
                Code.WriteLine($".class nested private {recordType.Name} {{");
                foreach (Declaration declaration in recordType.Elements)
                {
                    Code.Write("\t.field public ");
                    Code.LocalVarDef(declaration, false);
                    Code.WriteLine();
                }
                Code.Write(@"	.method public hidebysig specialname rtspecialname instance void 
      .ctor() cil managed 
    {
      .maxstack 8

		ldarg.0      // this
		call         instance void [mscorlib]System.Object::.ctor()
");
                Code.Emit("nop");
                Code.Emit("ret");
                Code.WriteLine("}");
                Code.WriteLine("}");
            }
        }

        private void InitComplexData(Block block)
        {
            foreach (Declaration declaration in block.Declarations.Where(x => x.Type.BaseType == BaseType.ComplexType))
            {
                var vd = declaration.Type as ArrayTypeDefinition;
                if (vd != null)
                {
                    Code.PushConst(vd.Size);
                    Code.Emit("newarr", Code.GetTypeName(vd.ArrayType.BaseType));
                    StoreVar(block, declaration, null);
                    continue;
                }
                var rd = declaration.Type as RecordTypeDefinition;
                if (rd != null)
                {
                    Code.Emit("newobj", "instance void", $"{Code.ClassName}/{rd.Name}::.ctor()");
                    StoreVar(block, declaration, null);
                }
                //throw new NotImplementedException();
            }
        }
    }
}
