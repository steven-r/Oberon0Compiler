namespace Oberon0.Compiler.Definitions
{
    public class SimpleTypeDefinition: TypeDefinition
    {

        public SimpleTypeDefinition(BaseType baseType)
            : base(baseType)
        { }

        public SimpleTypeDefinition(BaseType baseType, string name)
            : base(baseType)
        {
            Name = name;
        }

        public SimpleTypeDefinition(BaseType baseType, string name, bool isInternal)
            : base(baseType, isInternal)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}