namespace Oberon0.Compiler.Expressions.Operations.Internal
{
    internal class ArithmeticOperation
    {
        public ArithmeticOperation(IArithmeticOperation operation, IArithmeticOpMetadata metadata)
        {
            Operation = operation;
            Metadata = metadata;
        }

        public IArithmeticOperation Operation { get; }
        public IArithmeticOpMetadata Metadata { get; }
    }
}