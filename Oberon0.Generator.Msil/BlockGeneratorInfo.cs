using Oberon0.Compiler.Generator;

namespace Oberon0.Generator.Msil
{
    internal class BlockGeneratorInfo : IGeneratorInfo
    {
        public BlockGeneratorInfo(uint startAddress)
        {
            Offset = startAddress;
        }

        public uint DataSize { get; set; }

        /// <summary>
        /// Data start Address.
        /// </summary>
        /// <value>The offset.</value>
        public uint Offset { get; set; }

        public uint CodeStart { get; set; }
    }
}