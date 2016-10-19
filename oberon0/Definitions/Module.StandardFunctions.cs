using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oberon0.Compiler.Definitions
{
    public partial class Module
    {
        void DeclareStandardFunctions()
        {
            Block.Procedures.Add(new ProcedureDeclaration("WriteInt", Block, true, new ProcedureParameter("any", new SimpleTypeDefinition(BaseType.IntType), false)));
            Block.Procedures.Add(new ProcedureDeclaration("WriteReal", Block, true, new ProcedureParameter("any", new SimpleTypeDefinition(BaseType.DecimalType), false)));
            Block.Procedures.Add(new ProcedureDeclaration("WriteLn", Block, true));
            Block.Procedures.Add(new ProcedureDeclaration("ReadInt", Block, true, new ProcedureParameter("any", new SimpleTypeDefinition(BaseType.IntType), true)));
            Block.Procedures.Add(new ProcedureDeclaration("ReadReal", Block, true, new ProcedureParameter("any", new SimpleTypeDefinition(BaseType.DecimalType), true)));
        }
    }
}
