using Oberon0.Compiler.Generator;

namespace Oberon0.Generator.Msil
{
    internal class ExpressionGeneratorInfo : IGeneratorInfo
    {
        public int LeftRegister { get; set; }

        public int RightRegister { get; set; }
    }
}