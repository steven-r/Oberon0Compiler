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
            Block.Procedures.Add(new FunctionDeclaration("WriteInt", Block, true, new ProcedureParameter("any", new SimpleTypeDefinition(BaseType.IntType), false)));
            Block.Procedures.Add(new FunctionDeclaration("WriteReal", Block, true, new ProcedureParameter("any", new SimpleTypeDefinition(BaseType.DecimalType), false)));
            Block.Procedures.Add(new FunctionDeclaration("WriteLn", Block, true));
            Block.Procedures.Add(new FunctionDeclaration("ReadInt", Block, true, new ProcedureParameter("any", new SimpleTypeDefinition(BaseType.IntType), true)));
            Block.Procedures.Add(new FunctionDeclaration("ReadReal", Block, true, new ProcedureParameter("any", new SimpleTypeDefinition(BaseType.DecimalType), true)));

            Block.Procedures.Add(new FunctionDeclaration("eot", Block, BaseType.BoolType, true));
        }

        private void DeclareStandardTypes()
        {
            Block.Types.Add(new SimpleTypeDefinition(BaseType.IntType, "INTEGER"));
            Block.Types.Add(new SimpleTypeDefinition(BaseType.StringType, "BOOLEAN"));
            Block.Types.Add(new SimpleTypeDefinition(BaseType.DecimalType, "REAL"));
        }

        private void DeclareStandardConsts()
        {
            Block.Declarations.Add(new ConstDeclaration("TRUE", Block.LookupType("BOOLEAN"), new ConstantBoolExpression(true)));
            Block.Declarations.Add(new ConstDeclaration("FALSE", Block.LookupType("BOOLEAN"), new ConstantBoolExpression(false)));
        }
    }
}
