using Oberon0.Compiler.Generator;

namespace Oberon0.Generator.Msil
{
    internal class AssignmentGeneratorInfo: IGeneratorInfo
    {
        // ReSharper disable once InconsistentNaming
        private uint PC;

        public AssignmentGeneratorInfo(uint pC)
        {
            this.PC = pC;
        }
    }
}