using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions.Operations.Internal
{
    public interface IArithmeticOpMetadataView
    {
        int[] Operation { get;  }

        BaseType[] LeftHandType { get; }

        BaseType[] RightHandType { get; }

        BaseType[] ResultType { get; }
    }
}