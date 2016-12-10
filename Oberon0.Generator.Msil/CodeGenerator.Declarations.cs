using System;
using System.Linq;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Generator.Msil
{
    public partial class CodeGenerator
    {
        private void ProcessDeclarations(Block block, bool isRoot = false)
        {
            //TODO: Type generation
            //foreach (TypeDefinition typeDefinition in block.Types)
            //{
                
            //}
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
                }
                else
                {
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
            }
            if (!isFirst)
                Code.WriteLine(")");
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
                throw new NotImplementedException();
            }
        }
    }
}
