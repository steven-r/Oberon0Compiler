namespace Oberon0.Compiler.Definitions
{
    public class ArrayTypeDefinition : TypeDefinition
    {
        public ArrayTypeDefinition(int size, TypeDefinition baseType)
            :base(BaseType.ComplexType)
        {
            Size = size;
            ArrayType = baseType;
        }

        public int Size { get; set; }

        public TypeDefinition ArrayType { get; set; }

        public override string ToString()
        {
            return $"ARRAY {Size} OF {ArrayType}";
        }
    }
}