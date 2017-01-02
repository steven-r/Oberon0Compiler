using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Generator;

namespace Oberon0.Compiler.Types
{
    public abstract class TypeDefinition
    {
        /// <summary>
        /// The name for the type "VOID"
        /// </summary>
        public const string VoidTypeName = "$$VOID";

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

        /// <summary>
        /// Clones the current type and name it <see cref="name"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>TypeDefinition.</returns>
        public abstract TypeDefinition Clone(string name);
    }
}