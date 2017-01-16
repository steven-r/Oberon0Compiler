using System;
using System.Reflection;
using Oberon0.Attributes;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Definitions
{
    public partial class Module
    {
        private void DeclareStandardFunctions()
        {  
            // hardwired
            Block.Procedures.Add(FunctionDeclaration.AddHardwiredFunction("WriteInt", this, new ProcedureParameter("any", Block, Block.LookupType("INTEGER"), false)));
            Block.Procedures.Add(FunctionDeclaration.AddHardwiredFunction("WriteString", this, new ProcedureParameter("any", Block, Block.LookupType("STRING"), false)));
            Block.Procedures.Add(FunctionDeclaration.AddHardwiredFunction("WriteReal", this, new ProcedureParameter("any", Block, Block.LookupType("REAL"), false)));
            Block.Procedures.Add(FunctionDeclaration.AddHardwiredFunction("WriteLn", this));
            Block.Procedures.Add(FunctionDeclaration.AddHardwiredFunction("ReadInt", this, new ProcedureParameter("any", Block, Block.LookupType("INTEGER"), true)));
            Block.Procedures.Add(FunctionDeclaration.AddHardwiredFunction("ReadReal", this, new ProcedureParameter("any", Block, Block.LookupType("REAL"), true)));

            var asm = Assembly.Load("Oberon0.System");
            foreach (Type type in asm.GetExportedTypes())
            {
                if (type.GetCustomAttribute<Oberon0LibraryAttribute>() != null)
                {
                    LoadLibraryMembers(type);
                }
            }
        }

        /// <summary>
        /// load all members with that are exported through <see cref="Oberon0ExportAttribute"/>
        /// </summary>
        /// <param name="type">The type.</param>
        private void LoadLibraryMembers(Type type)
        {
            foreach (MethodInfo method in type.GetMethods(BindingFlags.Public|BindingFlags.Static))
            {
                var attr = method.GetCustomAttribute<Oberon0ExportAttribute>();
                if (attr == null) continue; // this method is not officially exported
                if (!method.IsStatic)
                {
                    throw new InvalidOperationException("method not static");
                }
                var rt = Block.LookupType(attr.ReturnType);
                if (rt == null)
                {
                    throw new InvalidOperationException($"return type {attr.ReturnType} not found");
                }
                ProcedureParameter[] procParameters = new ProcedureParameter[attr.Parameters.Length];
                for (int i = 0; i < attr.Parameters.Length; i++)
                {
                    bool isVar = false;
                    string paramType = attr.Parameters[i];
                    if (paramType.StartsWith("VAR ", StringComparison.InvariantCultureIgnoreCase))
                    {
                        isVar = true;
                        paramType = paramType.Substring(4).Trim(); // skip "VAR "
                    } else if (paramType.StartsWith("&"))
                    {
                        isVar = true;
                        paramType = paramType.Substring(1).Trim();
                    }
                    string paramName = "attr" + i;
                    TypeDefinition td = Block.LookupType(paramType);
                    if (td == null)
                    {
                        throw new InvalidOperationException($"type {attr.Parameters[i]} not found");
                    }
                    procParameters[i] = new ProcedureParameter(paramName, Block, td, isVar);
                }
                var fd = new FunctionDeclaration(attr.Name, Block, rt, procParameters)
                {
                    Prototype = $"Oberon0.{type.Name}::{attr.Name}",
                    IsExternal = true,
                };
                Block.Procedures.Add(fd);
            }
        }

        private void DeclareStandardTypes()
        {
            SimpleTypeDefinition.IntType = new SimpleTypeDefinition(BaseType.IntType, "INTEGER", true);
            SimpleTypeDefinition.BoolType = new SimpleTypeDefinition(BaseType.BoolType, "BOOLEAN", true);
            SimpleTypeDefinition.RealType = new SimpleTypeDefinition(BaseType.DecimalType, "REAL", true);
            SimpleTypeDefinition.StringType = new SimpleTypeDefinition(BaseType.StringType, "STRING", true);
            SimpleTypeDefinition.VoidType = new SimpleTypeDefinition(BaseType.VoidType, TypeDefinition.VoidTypeName,
                true);
            Block.Types.Add(SimpleTypeDefinition.IntType);
            Block.Types.Add(SimpleTypeDefinition.BoolType);
            Block.Types.Add(SimpleTypeDefinition.RealType);
            Block.Types.Add(SimpleTypeDefinition.StringType);
            Block.Types.Add(SimpleTypeDefinition.VoidType);
        }

        private void DeclareStandardConsts()
        {
            Block.Declarations.Add(new ConstDeclaration("TRUE", Block.LookupType("BOOLEAN"), new ConstantBoolExpression(true)));
            Block.Declarations.Add(new ConstDeclaration("FALSE", Block.LookupType("BOOLEAN"), new ConstantBoolExpression(false)));
        }
    }
}
