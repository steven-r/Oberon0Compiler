using Oberon0.Compiler.Generator;

namespace Oberon0.Compiler.Definitions
{
    public class Declaration
    {
        public string Name { get; set; }

        public TypeDefinition Type { get; set; }

        public Block Block { get; set; }

        /// <summary>
        /// Additional information used by the generator engine
        /// </summary>
        /// <value>Generator information.</value>
        public IGeneratorInfo GeneratorInfo { get; set; }

        public Declaration(string name, TypeDefinition type, Block block = null)
        {
            Name = name;
            Type = type;
            Block = block;
        }

        public override string ToString()
        {
            return $"var {Name} = {Type}";
        }
    }
}