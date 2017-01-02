using System;

namespace Oberon0.Compiler.Types
{
    /// <summary>
    /// Standard types 
    /// </summary>
    [Flags]
    public enum BaseType
    {
        /// <summary>
        /// If set, the given instance is failing
        /// </summary>
        ErrorType,

        /// <summary>
        /// Standard integer
        /// </summary>
        IntType = SimpleType + 1,
        /// <summary>
        /// The string type - Not in use
        /// </summary>
        StringType = SimpleType + 2,
        /// <summary>
        /// The decimal type
        /// </summary>
        DecimalType = SimpleType + 3,

        /// <summary>
        /// The bool type
        /// </summary>
        BoolType = SimpleType + 4,

        /// <summary>
        /// a "non" type. This means no value (like an empty return value for a function)
        /// </summary>
        VoidType = SimpleType + 5,

        /// <summary>
        /// Any type - used for internal functions (like WRITELN)
        /// </summary>
        AnyType = 0x20000,


        /// <summary>
        /// A non-base type (array or record)
        /// </summary>
        ComplexType = 0x40000,

        SimpleType = 0x10000,
    }
}