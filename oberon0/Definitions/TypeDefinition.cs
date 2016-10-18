namespace Oberon0.Compiler.Definitions
{
    public abstract class TypeDefinition
    {
        public string Name { get; set;  }

        public BaseType BaseType { get; }

        public TypeDefinition(BaseType baseType)
        {
            BaseType = baseType;
        }
    }
}