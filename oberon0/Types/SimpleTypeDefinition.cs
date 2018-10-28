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

        public SimpleTypeDefinition(BaseTypes baseTypes)
            : base(baseTypes)
        {
            if ((baseTypes & BaseTypes.SimpleType) == 0)
            {
                throw new InvalidCastException("Cannot create a nonsimple type with SimpleTypeDefinition");
            }
        }

        public SimpleTypeDefinition(BaseTypes baseTypes, string name)
            : base(baseTypes)
        {
            Name = name;
            if ((baseTypes & BaseTypes.SimpleType) == 0)
            {
                throw new InvalidCastException("Cannot create a nonsimple type with SimpleTypeDefinition");
            }
        }

        public SimpleTypeDefinition(BaseTypes baseTypes, string name, bool isInternal)
            : base(baseTypes, isInternal)
        {
            if ((baseTypes & BaseTypes.SimpleType) == 0)
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
            return new SimpleTypeDefinition(this.BaseTypes, name);
        }
    }
}