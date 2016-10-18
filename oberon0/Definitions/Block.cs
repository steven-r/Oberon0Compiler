using System.Collections.Generic;
using System.Linq;
using Oberon0.Compiler.Statements;

namespace Oberon0.Compiler.Definitions
{
    public class Block
    {
        public List<Declaration> Declarations { get; }

        public List<TypeDefinition> Types { get; }

        public List<BasicStatement> Statements { get; }

        public List<ProcedureDeclaration> Procedures { get; }

        public Block Parent { get; set; }

        public Block()
        {
            Declarations = new List<Declaration>();
            Types = new List<TypeDefinition>();
            Statements = new List<BasicStatement>();
            Procedures = new List<ProcedureDeclaration>();
        }

        public TypeDefinition LookupType(string name)
        {
            Block b = this;
            while (b != null)
            {
                var res = b.Types.FirstOrDefault(x => x.Name == name);
                if (res != null)
                {
                    return res;
                }
                b = b.Parent;
            }
            return null;
        }

        public Declaration LookupVar(string name)
        {
            Block b = this;
            while (b != null)
            {
                var res = b.Declarations.FirstOrDefault(x => x.Name == name);
                if (res != null)
                {
                    return res;
                }
                b = b.Parent;
            }
            return null;
        }
    }
}