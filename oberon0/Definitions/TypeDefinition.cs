using Oberon0.Compiler.Generator;

namespace Oberon0.Compiler.Definitions
{
    public abstract class TypeDefinition
    {
        public string Name { get; set;  }

        public BaseType BaseType { get; }

        /// <summary>
        /// Additional information used by the generator engine
        /// </summary>
        /// <value>Generator information.</value>
        public IGeneratorInfo GeneratorInfo { get; set; }

        public TypeDefinition(BaseType baseType)
        {
            BaseType = baseType;
        }
    }
}