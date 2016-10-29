using Oberon0.Compiler.Generator;

namespace Oberon0.Compiler.Definitions
{
    public abstract class TypeDefinition
    {
        public TypeDefinition(BaseType baseType)
        {
            BaseType = baseType;
        }

        public TypeDefinition(BaseType baseType, bool isInternal)
        {
            BaseType = baseType;
            IsInternal = isInternal;
        }

        public string Name { get; set;  }

        public BaseType BaseType { get; }

        public bool IsInternal { get; set; }

        /// <summary>
        /// Additional information used by the generator engine
        /// </summary>
        /// <value>Generator information.</value>
        public IGeneratorInfo GeneratorInfo { get; set; }
    }
}