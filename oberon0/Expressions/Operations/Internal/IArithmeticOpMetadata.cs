using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions.Operations.Internal
{
    public interface IArithmeticOpMetadata
    {
        int Operation { get; }

        BaseType ResultType { get; }
    }
}
