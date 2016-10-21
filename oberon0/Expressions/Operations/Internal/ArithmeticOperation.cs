namespace Oberon0.Compiler.Expressions.Operations
{
    internal class ArithmeticOperation
    {
        public IArithmeticOperation Operation { get; }
        public IArithmeticOpMetadata Metadata { get; }

        public ArithmeticOperation(IArithmeticOperation operation, IArithmeticOpMetadata metadata)
        {
            Operation = operation;
            Metadata = metadata;
        }
    }
}