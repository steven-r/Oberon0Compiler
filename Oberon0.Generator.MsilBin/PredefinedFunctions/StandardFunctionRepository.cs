#region copyright

// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------

#endregion

using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Linq;
using JetBrains.Annotations;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Generator.MsilBin.PredefinedFunctions
{
    [UsedImplicitly]
    internal static class StandardFunctionRepository
    {
        private static List<StandardFunctionGeneratorListElement> _standardFunctionList =
            new List<StandardFunctionGeneratorListElement>();

        /// <summary>
        ///     Gets the specified operation.
        /// </summary>
        /// <param name="function">The operation.</param>
        /// <returns>Lazy&lt;IArithmeticOperation, IArithmeticOpMetadata&gt;.</returns>
        // ReSharper disable once UnusedMethodReturnValue.Global
        [NotNull]
        public static StandardFunctionGeneratorListElement Get(FunctionDeclaration function)
        {
            string key =
                $"{function.Name}/{function.ReturnType.Name}/{string.Join("/", function.Block.Declarations.OfType<ProcedureParameterDeclaration>().Select(x => x.TypeName))}";

            var func = _standardFunctionList.FirstOrDefault(x => x.InstanceKey == key);
            if (func == null)
            {
                throw new ArgumentException("Cannot find function " + function, nameof(function));
            }

            return func;
        }

        internal static void Initialize(Module module)
        {
            var configuration = new ContainerConfiguration().WithAssembly(typeof(IStandardFunctionGenerator).Assembly);
            var container = configuration.CreateContainer();
            var exports =
                container.GetExports<ExportFactory<IStandardFunctionGenerator, StandardFunctionMetadataView>>();

            _standardFunctionList = new List<StandardFunctionGeneratorListElement>();

            foreach (var mefFunction in exports)
            {
                var element = new StandardFunctionGeneratorListElement
                {
                    Instance = mefFunction.CreateExport().Value,
                    Name = mefFunction.Metadata.Name,
                    ReturnType = module.Block.LookupType(
                        mefFunction.Metadata.ReturnType)
                };

                var parameters = mefFunction.Metadata.ParameterTypes?.Split(',') ?? new string[0];
                element.ParameterTypes = new ProcedureParameterDeclaration[parameters.Length];

                for (int j = 0; j < parameters.Length; j++)
                {
                    element.ParameterTypes[j] =
                        Module.GetProcedureParameterByName("__param__" + j, parameters[j], module.Block);
                }

                element.InstanceKey =
                    $"{element.Name}/{element.ReturnType.Name}/{string.Join("/", element.ParameterTypes.Select(x => x.TypeName))}";
                _standardFunctionList.Add(element);
            }
        }
    }
}
