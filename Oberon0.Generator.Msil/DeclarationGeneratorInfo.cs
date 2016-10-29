using Oberon0.Compiler.Generator;

namespace Oberon0.Generator.Msil
{
    internal class DeclarationGeneratorInfo : IGeneratorInfo
    {
        public int Offset { get; }

        public DeclarationGeneratorInfo(int offset)
        {
            Offset = offset;
        }
    }
}