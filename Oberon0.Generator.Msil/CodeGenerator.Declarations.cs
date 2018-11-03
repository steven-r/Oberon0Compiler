#region copyright

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeGenerator.Declarations.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil/CodeGenerator.Declarations.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#endregion

namespace Oberon0.Generator.Msil
{
    using System.Linq;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Types;

    /// <summary>
    /// The code generator.
    /// </summary>
    public partial class CodeGenerator
    {
        private void GenerateTypeDeclarations(Block block)
        {
            foreach (var typeDefinition in block.Types.Where(x => x is RecordTypeDefinition))
            {
                var recordType = (RecordTypeDefinition)typeDefinition;
                this.Code.WriteLine($".class nested private {recordType.Name} {{");
                foreach (Declaration declaration in recordType.Elements)
                {
                    this.Code.Write("\t.field public ");
                    this.Code.LocalVarDef(declaration, false);
                    this.Code.WriteLine();
                }

                this.Code.Write(
                    @"	.method public hidebysig specialname rtspecialname instance void 
      .ctor() cil managed 
    {
      .maxstack 8

		ldarg.0      // this
		call         instance void [mscorlib]System.Object::.ctor()
");
                this.Code.Emit("nop");
                this.Code.Emit("ret");
                this.Code.WriteLine("}");
                this.Code.WriteLine("}");
            }
        }

        private void InitComplexData(Block block)
        {
            foreach (Declaration declaration in block.Declarations.Where(x => x.Type.Type.HasFlag(BaseTypes.Complex)))
            {
                if (declaration.Type is ArrayTypeDefinition vd)
                {
                    this.Code.PushConst(vd.Size);
                    this.Code.Emit("newarr", Code.GetTypeName(vd.ArrayType.Type));
                    this.StoreVar(block, declaration, null);
                }
                else if (declaration.Type is RecordTypeDefinition rd)
                {
                    this.Code.Emit("newobj", "instance void", $"{this.Code.ClassName}/{rd.Name}::.ctor()");
                    this.StoreVar(block, declaration, null);
                }
            }
        }

        private void ProcessDeclarations(Block block, bool isRoot = false)
        {
            this.GenerateTypeDeclarations(block);
            int id = 0;
            bool isFirst = true;
            foreach (Declaration declaration in block.Declarations)
            {
                if (declaration is ProcedureParameter)
                {
                    // skip procedure parameters
                    continue;
                }

                if (declaration is ConstDeclaration c)
                {
                    this.Code.ConstField(c);
                    continue;
                }

                declaration.GeneratorInfo = new DeclarationGeneratorInfo(id++);
                if (isRoot)
                {
                    this.Code.DataField(declaration, true);
                }
                else
                {
                    if (isFirst)
                    {
                        this.Code.Write("\t.locals (");
                        isFirst = false;
                    }
                    else
                    {
                        this.Code.Write(", ");
                    }

                    this.Code.LocalVarDef(declaration, false);
                }
            }

            if (!isFirst) this.Code.WriteLine(")");
        }
    }
}