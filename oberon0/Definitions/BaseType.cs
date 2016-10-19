namespace Oberon0.Compiler.Definitions
{
    /// <summary>
    /// Standard types 
    /// </summary>
    public enum BaseType
    {
        /// <summary>
        /// If set, the given instance is failing
        /// </summary>
        ErrorType,

        /// <summary>
        /// Standard integer
        /// </summary>
        IntType,
        /// <summary>
        /// The string type - Not in use
        /// </summary>
        StringType,
        /// <summary>
        /// The decimal type
        /// </summary>
        DecimalType,
        /// <summary>
        /// A non-base type (array or record)
        /// </summary>
        ComplexType,

        /// <summary>
        /// The bool type
        /// </summary>
        BoolType,

        /// <summary>
        /// Any type - used for internal functions (like WRITELN)
        /// </summary>
        AnyType
    }
}