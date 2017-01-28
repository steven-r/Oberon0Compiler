using System.Linq;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Expressions
{
    public class VariableReferenceExpression : Expression
    {
        private string Name { get; set; }

        public Declaration Declaration { get; private set; }

        public VariableSelector Selector { get; private set; }

        public static Expression Create(Block block, string name, VariableSelector s)
        {
            Block b = block;
            while (b != null)
            {
                var v = b.Declarations.FirstOrDefault(x => x.Name == name);
                if (v == null)
                {
                    b = b.Parent;
                    continue;
                }
                var c = v as ConstDeclaration;
                if (c != null)
                    return c.Value;
                var e = new VariableReferenceExpression
                {
                    Name = name,
                    Declaration = v,
                    TargetType = s?.SelectorResultType ?? v.Type,
                    Selector = s
                };
                return e;
            }
            return null;
        }

        public override string ToString()
        {
            return $"{Name}({TargetType:G})";
        }
    }
}
