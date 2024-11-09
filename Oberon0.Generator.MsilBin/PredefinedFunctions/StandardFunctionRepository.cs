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
using Oberon0.Compiler.Exceptions;

namespace Oberon0.Generator.MsilBin.PredefinedFunctions
{
    [UsedImplicitly]
    internal static class StandardFunctionRepository
    {
        private static List<StandardFunctionGeneratorListElement> _standardFunctionList = [];

        /// <summary>
        ///     Gets the specified operation.
        /// </summary>
        /// <param name="function">The operation.</param>
        /// <exception cref="InternalCompilerException">Called if function is not found</exception>
        /// <returns>Lazy&lt;IArithmeticOperation, IArithmeticOpMetadata&gt;.</returns>
        // ReSharper disable once UnusedMethodReturnValue.Global
        public static StandardFunctionGeneratorListElement Get(FunctionDeclaration function)
        {
            string key =
                $"{function.Name}/{function.ReturnType.Name}/{string.Join("/", function.Block.Declarations.OfType<ProcedureParameterDeclaration>().Select(x => x.TypeName))}";

            var func = _standardFunctionList.Find(x => x.InstanceKey == key);

            return func ?? throw new InternalCompilerException("Cannot find function " + key);
        }

        internal static void Initialize(Module module)
        {
            var configuration = new ContainerConfiguration().WithAssembly(typeof(IStandardFunctionGenerator).Assembly);
            var container = configuration.CreateContainer();
            var exports =
                container.GetExports<ExportFactory<IStandardFunctionGenerator, StandardFunctionMetadataView>>();

            _standardFunctionList = [];

            foreach (var mefFunction in exports)
            {
                var element = new StandardFunctionGeneratorListElement
                {
                    Instance = mefFunction.CreateExport().Value,
                    Name = mefFunction.Metadata.Name,
                    ReturnType = module.Block.LookupType(
                        mefFunction.Metadata.ReturnType)
                };

                string[] parameters = [];
                if (!string.IsNullOrWhiteSpace(mefFunction.Metadata.ParameterTypes))
                {
                    parameters = mefFunction.Metadata.ParameterTypes.Split(',');
                }
                element.ParameterTypes = new ProcedureParameterDeclaration[parameters.Length];

                for (int j = 0; j < parameters.Length; j++)
                {
                    element.ParameterTypes[j] =
                        Module.GetProcedureParameterByName("__param__" + j, parameters[j], module.Block);
                }

                element.InstanceKey =
                    $"{element.Name}/{element.ReturnType!.Name}/{string.Join("/", element.ParameterTypes.Select(x => x.TypeName))}";
                _standardFunctionList.Add(element);
            }
        }

        /// <summary>
        /// Remove a function from the repository. 
        /// </summary>
        /// <remarks>This function is mainly used for testing purposes</remarks>
        /// <param name="key">The key of the function to be removed</param>
        /// <exception cref="ArgumentException"></exception>
        public static void RemoveFunction(string key)
        {
            var item = 
                _standardFunctionList.Find(x => x.InstanceKey == key) 
             ?? throw new ArgumentException($"Key {key} does not exist", nameof(key));

            _standardFunctionList.Remove(item);
        }
    }
}
