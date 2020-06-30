using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions.Operations.Internal
{
    public interface IArithmeticOpMetadata
    {
        int Operation { get; }

        BaseTypes ResultType { get; }
    }
}