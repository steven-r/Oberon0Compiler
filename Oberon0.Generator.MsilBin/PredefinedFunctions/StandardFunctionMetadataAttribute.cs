#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Composition;

namespace Oberon0.Generator.MsilBin.PredefinedFunctions
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
            : base(typeof(IStandardFunctionGenerator))
        {
            Name = name;
            ReturnType = returnType;
            ParameterTypes = parameterTypes;
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
        }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public string ReturnType { get; set; }

        /// <inheritdoc />
        public string ParameterTypes { get; set; }
    }
}