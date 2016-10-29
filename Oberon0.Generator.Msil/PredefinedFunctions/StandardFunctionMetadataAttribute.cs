using System;
using System.ComponentModel.Composition;

namespace Oberon0.Generator.Msil.PredefinedFunctions
{
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
        {
            Name = name;
            ReturnType = returnType;
            Namespace = CodeGenerator.DefaultNamespace;
        }

        /// <inheritdoc />
        public string Namespace { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string ReturnType { get; }

        /// <inheritdoc />
        public string ParameterTypes { get; }
    }
}