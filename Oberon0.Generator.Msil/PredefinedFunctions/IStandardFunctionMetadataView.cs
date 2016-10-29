namespace Oberon0.Generator.Msil.PredefinedFunctions
{
    public interface IStandardFunctionMetadataView
    {
        /// <summary>
        /// Gets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
        string[] Namespace { get; }

        /// <summary>
        /// The name of the function.
        /// </summary>
        /// <value>The name.</value>
        string[] Name { get; }

        /// <summary>
        /// The name of the return type
        /// </summary>
        string[] ReturnType { get; }

        /// <summary>
        /// The types of the parameter
        /// </summary>
        /// <value>The parameter types.</value>
        string[] ParameterTypes { get; }
    }
}