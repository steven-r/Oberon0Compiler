using System;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Types
{
    public class SimpleTypeDefinition: TypeDefinition
    {
        /// <summary>
        /// Global reference to "INTEGER".
        /// </summary>
        public static TypeDefinition IntType { get; set; }


        /// <summary>
        /// Global reference to "BOOLEAN".
        /// </summary>
        public static TypeDefinition BoolType { get; set; }

        /// <summary>
        /// Global reference to "REAL".
        /// </summary>
        public static TypeDefinition RealType { get; set; }

        /// <summary>
        /// Global reference to "STRING".
        /// </summary>
        public static TypeDefinition StringType { get; set; }

        /// <summary>
        /// Global reference to "VOID".
        /// </summary>
        public static TypeDefinition VoidType { get; set; }

        public SimpleTypeDefinition(BaseType baseType)
            : base(baseType)
        {
            if ((baseType & BaseType.SimpleType) == 0)
            {
                throw new InvalidCastException("Cannot create a nonsimple type with SimpleTypeDefinition");
            }
        }

        public SimpleTypeDefinition(BaseType baseType, string name)
            : base(baseType)
        {
            Name = name;
            if ((baseType & BaseType.SimpleType) == 0)
            {
                throw new InvalidCastException("Cannot create a nonsimple type with SimpleTypeDefinition");
            }
        }

        public SimpleTypeDefinition(BaseType baseType, string name, bool isInternal)
            : base(baseType, isInternal)
        {
            if ((baseType & BaseType.SimpleType) == 0)
            {
                throw new InvalidCastException("Cannot create a nonsimple type with SimpleTypeDefinition");
            }
            Name = name;
        }


        public override string ToString()
        {
            return Name;
        }

        public override TypeDefinition Clone(string name)
        {
            return new SimpleTypeDefinition(BaseType, name);
        }
    }
}