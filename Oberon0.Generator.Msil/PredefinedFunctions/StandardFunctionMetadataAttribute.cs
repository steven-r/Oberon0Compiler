namespace Oberon0.Generator.Msil.PredefinedFunctions
{
    using System;
    using System.Composition;

    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class StandardFunctionMetadataAttribute : ExportAttribute, IStandardFunctionExportMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardFunctionMetadataAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="returnType">Type of the return.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        public StandardFunctionMetadataAttribute(string name, string returnType, string parameterTypes)
            : base(typeof(IStandardFunctionGenerator))
        {
            Name = name;
            ReturnType = returnType;
            ParameterTypes = parameterTypes;
            Namespace = CodeGenerator.DefaultNamespace;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardFunctionMetadataAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="returnType">Type of the return.</param>
        public StandardFunctionMetadataAttribute(string name, string returnType)
            : base(typeof(IStandardFunctionGenerator))
        {
            Name = name;
            ReturnType = returnType;
            Namespace = CodeGenerator.DefaultNamespace;
        }

        /// <inheritdoc />
        public string Namespace { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public string ReturnType { get; set; }

        /// <inheritdoc />
        public string ParameterTypes { get; set; }
    }
}