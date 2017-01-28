namespace Oberon0.Compiler.Types
{
    public class ArrayTypeDefinition : TypeDefinition
    {
        public ArrayTypeDefinition(int size, TypeDefinition baseType)
            :base(BaseType.ComplexType)
        {
            Size = size;
            ArrayType = baseType;
        }

        public ArrayTypeDefinition(int size, TypeDefinition baseType, string name)
            :base(BaseType.ComplexType)
        {
            Size = size;
            ArrayType = baseType;
            Name = name;
        }

        public int Size { get; }

        public TypeDefinition ArrayType { get; }

        public override string ToString()
        {
            return $"ARRAY {Size} OF {ArrayType}";
        }

        public override TypeDefinition Clone(string name)
        {
            return new ArrayTypeDefinition(Size, ArrayType, name);
        }
    }
}