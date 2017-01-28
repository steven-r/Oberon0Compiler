using System.Collections.Generic;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Types
{
    public class RecordTypeDefinition : TypeDefinition
    {
        public RecordTypeDefinition()
            : base(BaseType.ComplexType)
        {
            Elements = new List<Declaration>();
        }

        public RecordTypeDefinition(string name)
            :base(BaseType.ComplexType)
        {
            Name = name;
            Elements = new List<Declaration>();
        }

        public List<Declaration> Elements { get; }

        public override TypeDefinition Clone(string name)
        {
            var r = new RecordTypeDefinition(name);
            r.Elements.AddRange(Elements);
            return r;
        }
    }

}