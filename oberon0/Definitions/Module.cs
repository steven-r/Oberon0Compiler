using JetBrains.Annotations;
using Oberon0.Compiler.Generator;

namespace Oberon0.Compiler.Definitions
{
    public partial class Module
    {
        public Module()
        {
            Block = new Block();

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
    }
}