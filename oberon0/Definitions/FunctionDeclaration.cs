using System.Linq;
using JetBrains.Annotations;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Definitions
{
    public class FunctionDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionDeclaration"/> class defining a function returning <c>VOID</c>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="parameters">The parameters.</param>
        public FunctionDeclaration(string name, Block parent, params ProcedureParameter[] parameters)
        {
            Name = name;
            Block = new Block { Parent = parent };
            if (parameters != null)
            {
                Block.Declarations.AddRange(parameters);
            }
            ReturnType = new SimpleTypeDefinition(BaseType.VoidType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionDeclaration"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="returnType">Type of the return.</param>
        /// <param name="parameters">The parameters.</param>
        public FunctionDeclaration(string name, Block parent, TypeDefinition returnType, params ProcedureParameter[] parameters)
        {
            Name = name;
            Block = new Block { Parent = parent };
            Block.Declarations.AddRange(parameters);
            ReturnType = returnType;
        }

        public static FunctionDeclaration AddHardwiredFunction(string name, Module module, params ProcedureParameter[] parameters)
        {
            var res = new FunctionDeclaration(name, module.Block, parameters)
            {
                IsInternal = true,
                ReturnType = module.Block.LookupType(TypeDefinition.VoidTypeName),
                Prototype = $"Oberon0.{module.Name}::{name}"
            };
            return res;
        }

        public Block Block { get; }

        public string Name { get; }

        public TypeDefinition ReturnType { get; private set; }

        public bool IsInternal { get; private set; }

        public string Prototype { [UsedImplicitly] get; internal set; }

        public override string ToString()
        {
            return $"{(IsInternal?"internal ": string.Empty)}{ReturnType:G} {Name}("
                + string.Join(", ", Block.Declarations.OfType<ProcedureParameter>().Select(x => x.Type.BaseType.ToString("G"))) + ")";
        }
    }
}
