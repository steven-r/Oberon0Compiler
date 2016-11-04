using Oberon0.Compiler.Generator;

namespace Oberon0.Generator.Msil
{
    internal class DeclarationGeneratorInfo : IGeneratorInfo
    {
        public DeclarationGeneratorInfo(int offset)
        {
            Offset = offset;
        }

        public int Offset { get; }
    }
}