using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using JetBrains.Annotations;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Generator.Msil.PredefinedFunctions
{
    internal static class StandardFunctionRepository
    {
        private static List<StandardFunctionGeneratorListElement> _standardFunctionList = new List<StandardFunctionGeneratorListElement>();

        internal static void Initialize(Module module)
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(IStandardFunctionGenerator).Assembly));

            var container = new CompositionContainer(catalog,
                CompositionOptions.DisableSilentRejection |
                CompositionOptions.IsThreadSafe);

            _standardFunctionList = new List<StandardFunctionGeneratorListElement>();
            var exports = container.GetExports<IStandardFunctionGenerator, IStandardFunctionMetadataView>();

            foreach (Lazy<IStandardFunctionGenerator, IStandardFunctionMetadataView> mefFunction in exports)
                for (int i = 0; i < mefFunction.Metadata.Name.Length; i++)
                {
                    StandardFunctionGeneratorListElement element = new StandardFunctionGeneratorListElement
                    {
                        Instance = mefFunction.Value,
                        Name = mefFunction.Metadata.Name[i],
                        ReturnType = module.Block.LookupType(mefFunction.Metadata.ReturnType[i], true)
                    };
                    string[] parameters = mefFunction.Metadata.ParameterTypes[i]?.Split(',') ?? new string[0];
                    element.ParameterTypes = new ProcedureParameter[parameters.Length];
                    for (int j = 0; j < parameters.Length; j++)
                    {
                        TypeDefinition td = module.Block.LookupType(parameters[j], true);
                        element.ParameterTypes[j] = new ProcedureParameter(parameters[j], module.Block, td, 
                            parameters[j].StartsWith("&", StringComparison.InvariantCulture));
                    }
                    element.InstanceKey =
                        $"{element.Name}/{element.ReturnType.Name}/{string.Join("/", element.ParameterTypes.Select(x => x.Name))}";
                    _standardFunctionList.Add(element);
                }
        }


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

            var func = _standardFunctionList.FirstOrDefault(x => x.InstanceKey == key);
            if (func == null)
                throw new InvalidOperationException("Cannot find function " + function);
            return func;
        }

        private static object GetFunctionName(ProcedureParameter parameter)
        {
            return $"{(parameter.IsVar ? "&" : string.Empty)}{parameter.Type.Name}";
        }
    }
}
