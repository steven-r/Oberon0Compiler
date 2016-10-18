using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Expressions
{
    public class VariableReferenceExpression : Expression
    {
        public string Name { get; set; }

        public Declaration Declaration { get; set; }

        public static Expression Create(Block block, string name)
        {
            Block b = block;
            while (b != null)
            {
                if (b.Declarations.Any(x => x.Name == name))
                {
                    var e = new VariableReferenceExpression
                    {
                        Name = name,
                        Declaration = b.Declarations.FirstOrDefault(x => x.Name == name)
                    };
                    return e;
                }
                b = b.Parent;
            }
            return null;
        }
    }
}
