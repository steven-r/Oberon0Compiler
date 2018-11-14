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
    using System.Collections.Generic;
    using System.Linq;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions.Constant;
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
                this.Code.WriteLine($".class nested private __{recordType.Name} {{");
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
                if (declaration is ProcedureParameterDeclaration)
                {
                    continue;
                }

                if (declaration.Type is ArrayTypeDefinition vd)
                {
                    this.Code.PushConst(vd.Size);
                    this.Code.Emit("newarr", Code.GetTypeName(vd.ArrayType.Type));
                    this.StoreVar(block, declaration, null);
                }
                else if (declaration.Type is RecordTypeDefinition rd)
                {
                    this.Code.Emit("newobj", "instance void", $"{this.Code.ClassName}/__{rd.Name}::.ctor()");
                    this.StoreVar(block, declaration, null);
                }

                if (declaration.GeneratorInfo is DeclarationGeneratorInfo dgi && dgi.OriginalField != null)
                {
                    CopyVarComplexData(declaration);
                }
            }
        }

        /// <summary>
        /// Copy data from parent.
        /// </summary>
        /// <param name="declaration">The given parameter</param>
        private void CopyVarComplexData(Declaration declaration)
        {
            PerformDeepCopy(declaration, declaration.Type);
        }

        private void PerformDeepCopy(Declaration declaration, TypeDefinition type)
        {
            if (type is ArrayTypeDefinition arrayType)
            {
                for (int i = 0; i < arrayType.Size; i++)
                {
                    var vs = new VariableSelector { new IndexSelector(new ConstantIntExpression(i + 1), null) };
                    DeclarationGeneratorInfo dgi = (DeclarationGeneratorInfo)declaration.GeneratorInfo;
                    this.Load(declaration.Block, declaration, vs, isStore: true);
                    this.Load(declaration.Block, dgi.OriginalField, vs, isVarParam: true, ignoreReplacement: true);
                    StoreVar(declaration.Block, declaration, vs);
                }
            }
            else if (type is RecordTypeDefinition recordType)
            {
                foreach (Declaration element in recordType.Elements)
                {
                    var vs = new VariableSelector
                        {
                            new IdentifierSelector(element.Name, null)
                                {
                                    TypeDefinition = element.Type,
                                    BasicTypeDefinition = recordType,
                                    Element = element
                                }
                        };
                    DeclarationGeneratorInfo dgi = (DeclarationGeneratorInfo)declaration.GeneratorInfo;
                    this.Load(declaration.Block, declaration, vs, isStore: true);
                    this.Load(declaration.Block, dgi.OriginalField, vs, isVarParam: true, ignoreReplacement: true);
                    StoreVar(declaration.Block, declaration, vs);
                }
            }
        }

        private void ProcessDeclarations(Block block, bool isRoot = false)
        {
            this.GenerateTypeDeclarations(block);
            GenerateComplexTypeMappings(block);
            int varId = block.Declarations.Where(x => !(x is ProcedureParameterDeclaration)).Select(x => x.GeneratorInfo)
                            .OfType<DeclarationGeneratorInfo>().DefaultIfEmpty(new DeclarationGeneratorInfo(-1))
                            .Max(y => y.Offset) + 1;
            int paramId = block.Declarations.Where(x => x is ProcedureParameterDeclaration).Select(x => x.GeneratorInfo)
                            .OfType<DeclarationGeneratorInfo>().DefaultIfEmpty(new DeclarationGeneratorInfo(-1))
                            .Max(y => y.Offset) + 1;
            bool isFirst = true;
            foreach (Declaration declaration in block.Declarations)
            {
                if (declaration is ProcedureParameterDeclaration)
                {
                    if (declaration.GeneratorInfo == null)
                    {
                        declaration.GeneratorInfo = new DeclarationGeneratorInfo(paramId++);
                    }

                    // skip value procedure parameters
                    continue;
                }

                if (declaration is ConstDeclaration c)
                {
                    this.Code.ConstField(c);
                    continue;
                }

                if (declaration.GeneratorInfo == null)
                {
                    declaration.GeneratorInfo = new DeclarationGeneratorInfo(varId++);
                }

                if (isRoot)
                {
                    Code.DataField(declaration, true);
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

        private void GenerateComplexTypeMappings(Block block)
        {
            List<Declaration> addDeclarations = new List<Declaration>();
            int paramId = 0;
            int varId = 0;
            foreach (ProcedureParameterDeclaration pp in block.Declarations.OfType<ProcedureParameterDeclaration>())
            {
                if (pp.IsVar || pp.Type.Type.HasFlag(BaseTypes.Simple))
                {
                    continue;
                }

                var name = pp.Name;
                pp.Name = "param__" + name;
                pp.GeneratorInfo = new DeclarationGeneratorInfo(paramId++);

                // rename parameter and create a new field
                var field = new Declaration(name, pp.Type, block)
                    {
                        GeneratorInfo = new DeclarationGeneratorInfo(varId++)
                    };
                ((DeclarationGeneratorInfo)field.GeneratorInfo).OriginalField = pp;
                ((DeclarationGeneratorInfo)pp.GeneratorInfo).ReplacedBy = field;
                addDeclarations.Add(field);
            }

            block.Declarations.AddRange(addDeclarations);
        }
    }
}