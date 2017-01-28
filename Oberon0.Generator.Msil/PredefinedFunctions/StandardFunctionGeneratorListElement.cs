using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Types;

namespace Oberon0.Generator.Msil.PredefinedFunctions
{
    internal class StandardFunctionGeneratorListElement: IStandardFunctionMetadata
    {
        /// <summary>
        /// The instance handling the standard function
        /// </summary>
        /// <value>The instance.</value>
        public IStandardFunctionGenerator Instance { get; set; }

        /// <summary>
        /// The lookup key to find the appropriate function
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
        public ProcedureParameter[] ParameterTypes { get; set; }

        public string AdditionalInfo { get; set; }
    }
}
