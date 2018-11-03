using Oberon0.Compiler.Generator;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Definitions
{
    public class Declaration
    {
        public Declaration(string name, TypeDefinition type) 
            : this(name, type, null)
        {
        }

        public Declaration(string name, TypeDefinition type, Block block)
        {
            Name = name;
            Type = type;
            Block = block;
        }

        public string Name { get; }

        public TypeDefinition Type { get; }

        public Block Block { get; }

        /// <summary>
        /// Gets or sets additional information used by the generator engine
        /// </summary>
        /// <value>Generator information.</value>
        public IGeneratorInfo GeneratorInfo { get; set; }

        public override string ToString()
        {
            return $"{Name}:{Type}";
        }
    }
}