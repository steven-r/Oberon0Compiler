using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Oberon0.Compiler.Generator;

namespace Oberon0.Compiler.Definitions
{
    public partial class Module
    {
        public Module()
        {
            Block = new Block();
            ExternalReferences = new List<Assembly>();

            DeclareStandardTypes();
            DeclareStandardConsts();
            DeclareStandardFunctions();
        }

        public string Name { get; set; }
        public Block Block { get; }

        /// <summary>
        /// Additional information used by the generator engine
        /// </summary>
        /// <value>Generator information.</value>
        [UsedImplicitly]
        public IGeneratorInfo GeneratorInfo { get; set; }

        /// <summary>
        /// Gets or sets the external references.
        /// </summary>
        /// <value>The external references.</value>
        public List<Assembly> ExternalReferences { get; }
    }
}