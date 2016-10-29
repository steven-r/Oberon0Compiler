using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Compiler.Definitions
{
    public partial class Module
    {
        void DeclareStandardFunctions()
        {
            Block.Procedures.Add(new FunctionDeclaration("WriteInt", Block, true, new ProcedureParameter("any", Block.LookupType("INTEGER"), false)));
            Block.Procedures.Add(new FunctionDeclaration("WriteString", Block, true, new ProcedureParameter("any", Block.LookupType("STRING"), false)));
            Block.Procedures.Add(new FunctionDeclaration("WriteReal", Block, true, new ProcedureParameter("any", Block.LookupType("REAL"), false)));
            Block.Procedures.Add(new FunctionDeclaration("WriteLn", Block, true));
            Block.Procedures.Add(new FunctionDeclaration("ReadInt", Block, true, new ProcedureParameter("any", Block.LookupType("INTEGER"), true)));
            Block.Procedures.Add(new FunctionDeclaration("ReadReal", Block, true, new ProcedureParameter("any", Block.LookupType("REAL"), true)));

            Block.Procedures.Add(new FunctionDeclaration("eot", Block, Block.LookupType("BOOLEAN"), true));
        }

        private void DeclareStandardTypes()
        {
            Block.Types.Add(new SimpleTypeDefinition(BaseType.IntType, "INTEGER"));
            Block.Types.Add(new SimpleTypeDefinition(BaseType.BoolType, "BOOLEAN"));
            Block.Types.Add(new SimpleTypeDefinition(BaseType.DecimalType, "REAL"));
            Block.Types.Add(new SimpleTypeDefinition(BaseType.StringType, "STRING"));
            Block.Types.Add(new SimpleTypeDefinition(BaseType.VoidType, "VOID", true));
        }

        private void DeclareStandardConsts()
        {
            Block.Declarations.Add(new ConstDeclaration("TRUE", Block.LookupType("BOOLEAN"), new ConstantBoolExpression(true)));
            Block.Declarations.Add(new ConstDeclaration("FALSE", Block.LookupType("BOOLEAN"), new ConstantBoolExpression(false)));
        }
    }
}
