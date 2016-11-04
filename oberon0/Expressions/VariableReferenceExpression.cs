using System;
using System.Linq;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Expressions
{
    public class VariableReferenceExpression : Expression
    {
        public string Name { get; set; }

        public Declaration Declaration { get; set; }

        public VariableSelector Selector { get; set; }

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
                    TargetType = GetTargetType(v.Type, s),
                    Selector = s
                };
                return e;
            }
            return null;
        }

        private static BaseType GetTargetType(TypeDefinition var, VariableSelector selectors)
        {
            if (selectors.Count == 0)
                return var.BaseType;
            foreach (var selector in selectors)
            {
                if (var == null)
                    throw new InvalidCastException("No element for selector");
                if (var.BaseType != BaseType.ComplexType)
                    throw new InvalidCastException("You cannot use {var.GetType()} on a reference of {var.Type.BaseType}");
                var arr = var as ArrayTypeDefinition;
                if (arr != null)
                {
                    var offsetSel = selector as IndexSelector;
                    if (offsetSel == null)
                        throw new InvalidCastException("Offset required");
                    var = arr.ArrayType;

                }
                else
                {
                    var rec = var as RecordTypeDefinition;
                    if (rec != null)
                    {
                        var rSel = selector as IdentifierSelector;
                        if (rSel == null)
                            throw new InvalidCastException("Identifier for record expected, found " + selector.GetType().Name);
                        var elem = rec.Elements.FirstOrDefault(x => x.Name == rSel.Name);
                        if (elem == null)
                            throw new InvalidCastException($"There's no element {rSel.Name} in record");
                        var = elem.Type;
                    }
                }
            }
            return var.BaseType;
        }

        public override string ToString()
        {
            return $"{Name}({TargetType:G})";
        }
    }
}
