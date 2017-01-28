using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Types;

namespace Oberon0.Generator.Msil.PredefinedFunctions
{
    public interface IStandardFunctionMetadata
    {
        /// <summary>
        /// The object/namespace this function belongs to (<see cref="CodeGenerator.DefaultNamespace"/> for default namespace)
        /// </summary>
        /// <value>The name space.</value>
        string NameSpace { get; }

        ///  <summary>
        /// The name of the function.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// The name of the return type
        /// </summary>
        TypeDefinition ReturnType { get; }

        /// <summary>
        /// The types of the parameter
        /// </summary>
        /// <value>The parameter types.</value>
        ProcedureParameter[] ParameterTypes { get; }
    }
}