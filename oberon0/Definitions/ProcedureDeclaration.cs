using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oberon0.Compiler.Definitions
{
    public class ProcedureDeclaration
    {
        public Block Block { get; }

        public string Name { get; }

        public List<ProcedureParameter> Parameters { get; }

        public ProcedureDeclaration(string name, Block parent)
        {
            Name = name;
            Block = new Block {Parent = parent};
            Parameters = new List<ProcedureParameter>();
        }
    }
}
