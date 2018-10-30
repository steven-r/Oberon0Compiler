#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardFunctionRepository.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil/StandardFunctionRepository.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Generator.Msil.PredefinedFunctions
{
    using System;
    using System.Collections.Generic;
    using System.Composition;
    using System.Composition.Hosting;
    using System.Linq;

    using JetBrains.Annotations;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Types;

    internal static class StandardFunctionRepository
    {
        private static List<StandardFunctionGeneratorListElement> standardFunctionList =
            new List<StandardFunctionGeneratorListElement>();

        /// <summary>
        /// Gets the specified operation.
        /// </summary>
        /// <param name="function">The operation.</param>
        /// <returns>Lazy&lt;IArithmeticOperation, IArithmeticOpMetadata&gt;.</returns>
        [NotNull]
        public static StandardFunctionGeneratorListElement Get(FunctionDeclaration function)
        {
            string key =
                $"{function.Name}/{function.ReturnType.Name}/{string.Join("/", function.Block.Declarations.OfType<ProcedureParameter>().Select(GetFunctionName))}";

            var func = standardFunctionList.FirstOrDefault(x => x.InstanceKey == key);
            if (func == null)
                throw new InvalidOperationException("Cannot find function " + function);
            return func;
        }

        internal static void Initialize(Module module)
        {
            var configuration = new ContainerConfiguration().WithAssembly(typeof(IStandardFunctionGenerator).Assembly);
            var container = configuration.CreateContainer();
            var exports =
                container.GetExports<ExportFactory<IStandardFunctionGenerator, StandardFunctionMetadataView>>();

            standardFunctionList = new List<StandardFunctionGeneratorListElement>();

            foreach (var mefFunction in exports)
            {
                StandardFunctionGeneratorListElement element = new StandardFunctionGeneratorListElement
                                                                   {
                                                                       Instance = mefFunction.CreateExport().Value,
                                                                       Name = mefFunction.Metadata.Name,
                                                                       ReturnType = module.Block.LookupType(
                                                                           mefFunction.Metadata.ReturnType)
                                                                   };

                var parameters = mefFunction.Metadata.ParameterTypes?.Split(',') ?? new string[0];
                element.ParameterTypes = new ProcedureParameter[parameters.Length];

                for (int j = 0; j < parameters.Length; j++)
                {
                    TypeDefinition td = module.Block.LookupType(parameters[j]);
                    element.ParameterTypes[j] = new ProcedureParameter(
                        parameters[j],
                        module.Block,
                        td,
                        parameters[j].StartsWith("&", StringComparison.InvariantCulture));
                }

                element.InstanceKey =
                    $"{element.Name}/{element.ReturnType.Name}/{string.Join("/", element.ParameterTypes.Select(x => x.Name))}";
                standardFunctionList.Add(element);
            }
        }

        private static object GetFunctionName(ProcedureParameter parameter)
        {
            return $"{(parameter.IsVar ? "&" : string.Empty)}{parameter.Type.Name}";
        }
    }
}