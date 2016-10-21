using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Expressions.Operations
{
    public interface IArithmeticOpMetadataView
    {
        TokenType[] Operation { get;  }

        BaseType[] LeftHandType { get; }

        BaseType[] RightHandType { get; }

        BaseType[] TargetType { get; }

    }
}