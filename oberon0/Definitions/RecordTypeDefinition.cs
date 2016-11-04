using System.Collections.Generic;

namespace Oberon0.Compiler.Definitions
{
    public class RecordTypeDefinition : TypeDefinition
    {
        public RecordTypeDefinition()
            : base(BaseType.ComplexType)
        {
            Elements = new List<Declaration>();
        }

        public List<Declaration> Elements { get; }
    }

}