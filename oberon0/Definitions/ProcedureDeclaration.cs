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

        public bool IsInternal { get; set; }

        public ProcedureDeclaration(string name, Block parent, params ProcedureParameter[] parameters)
        {
            Name = name;
            Block = new Block { Parent = parent };
            Parameters = new List<ProcedureParameter>(parameters);
        }

        public ProcedureDeclaration(string name, Block parent, bool isInternal, params ProcedureParameter[] parameters)
        {
            Name = name;
            Block = new Block { Parent = parent };
            Parameters = new List<ProcedureParameter>(parameters);
            IsInternal = isInternal;
        }
    }
}
