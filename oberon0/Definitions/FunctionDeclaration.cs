using System.Collections.Generic;
using System.Linq;

namespace Oberon0.Compiler.Definitions
{
    public class FunctionDeclaration
    {
        public FunctionDeclaration(string name, Block parent, params ProcedureParameter[] parameters)
        {
            Name = name;
            Block = new Block { Parent = parent };
            Block.Declarations.AddRange(parameters);
            ReturnType = new SimpleTypeDefinition(BaseType.VoidType);
        }

        public FunctionDeclaration(string name, Block parent, BaseType returnType, params ProcedureParameter[] parameters)
        {
            Name = name;
            Block = new Block { Parent = parent };
            Block.Declarations.AddRange(parameters);
            ReturnType = new SimpleTypeDefinition(returnType);
        }

        public FunctionDeclaration(string name, Block parent, TypeDefinition returnType, bool isInternal, params ProcedureParameter[] parameters)
        {
            Name = name;
            Block = new Block { Parent = parent };
            Block.Declarations.AddRange(parameters);
            IsInternal = isInternal;
            ReturnType = returnType;
        }

        public FunctionDeclaration(string name, Block parent, bool isInternal, params ProcedureParameter[] parameters)
        {
            Name = name;
            Block = new Block { Parent = parent };
            Block.Declarations.AddRange(parameters);
            IsInternal = isInternal;
            ReturnType = parent.LookupType("VOID", true);
        }

        public Block Block { get; }

        public string Name { get; }

        public TypeDefinition ReturnType { get; }

        public bool IsInternal { get; set; }

        public override string ToString()
        {
            return $"{(IsInternal?"internal ": string.Empty)}{ReturnType:G} {Name}("
                + string.Join(", ", Block.Declarations.OfType<ProcedureParameter>().Select(x => x.Type.BaseType.ToString("G"))) + ")";
        }
    }
}
