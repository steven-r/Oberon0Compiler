using Oberon0.Shared;

namespace Oberon0.Generator.MsilBin
{
    public partial class MsilBinGenerator
    {
        /// <inheritdoc />
        public bool GenerateBinary(CreateBinaryOptions createBinaryOptions = null)
        {
            var binary = new CreateBinary(this, createBinaryOptions);
            return binary.Execute();
        }


    }
}
