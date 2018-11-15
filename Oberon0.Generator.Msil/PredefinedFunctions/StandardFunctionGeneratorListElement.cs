
namespace Oberon0.Generator.Msil.PredefinedFunctions
{
    using System.Diagnostics;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Types;

    [DebuggerDisplay("Function {" + nameof(InstanceKey) + "}")]
    internal class StandardFunctionGeneratorListElement : IStandardFunctionMetadata
    {
        /// <summary>
        /// Gets or sets the instance handling the standard function
        /// </summary>
        /// <value>The instance.</value>
        public IStandardFunctionGenerator Instance { get; set; }

        /// <summary>
        /// Gets or sets the lookup key to find the appropriate function
        /// </summary>
        /// <value>The instance key.</value>
        public string InstanceKey { get; set; }

        /// <inheritdoc />
        public string NameSpace { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public TypeDefinition ReturnType { get; set; }

        /// <inheritdoc />
        public ProcedureParameterDeclaration[] ParameterTypes { get; set; }

        public string AdditionalInfo { get; set; }
    }
}
