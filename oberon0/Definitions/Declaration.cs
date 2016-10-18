namespace Oberon0.Compiler.Definitions
{
    public class Declaration
    {
        public string Name { get; set; }

        public TypeDefinition Type { get; set; }

        public Block Block { get; set; }

        public Declaration(string name, TypeDefinition type, Block block = null)
        {
            Name = name;
            Type = type;
            Block = block;
        }
    }
}