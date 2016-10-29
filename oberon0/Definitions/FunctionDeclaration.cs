using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oberon0.Compiler.Definitions
{
    public class FunctionDeclaration
    {
        public Block Block { get; }

        public string Name { get; }

        public TypeDefinition ReturnType { get; }

        public List<ProcedureParameter> Parameters { get; }

        public bool IsInternal { get; set; }

        public FunctionDeclaration(string name, Block parent, params ProcedureParameter[] parameters)
        {
            Name = name;
            Block = new Block { Parent = parent };
            Parameters = new List<ProcedureParameter>(parameters);
            ReturnType = new SimpleTypeDefinition(BaseType.VoidType);
        }

        public FunctionDeclaration(string name, Block parent, BaseType returnType, params ProcedureParameter[] parameters)
        {
            Name = name;
            Block = new Block { Parent = parent };
            Parameters = new List<ProcedureParameter>(parameters);
            ReturnType = new SimpleTypeDefinition(returnType);
        }

        public FunctionDeclaration(string name, Block parent, TypeDefinition returnType, bool isInternal, params ProcedureParameter[] parameters)
        {
            Name = name;
            Block = new Block { Parent = parent };
            Parameters = new List<ProcedureParameter>(parameters);
            IsInternal = isInternal;
            ReturnType = returnType;
        }

        public FunctionDeclaration(string name, Block parent, bool isInternal, params ProcedureParameter[] parameters)
        {
            Name = name;
            Block = new Block { Parent = parent };
            Parameters = new List<ProcedureParameter>(parameters);
            IsInternal = isInternal;
            ReturnType = parent.LookupType("VOID", true);
        }

        public override string ToString()
        {
            return $"{(IsInternal?"internal ": string.Empty)}{ReturnType:G} {Name}("
                + string.Join(", ", Parameters.Select(x => x.Type.BaseType.ToString("G"))) + ")";
        }

    }
}
