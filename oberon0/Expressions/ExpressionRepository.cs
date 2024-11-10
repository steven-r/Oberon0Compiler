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
using Oberon0.Compiler.Expressions.Functions.Internal;
using Oberon0.Compiler.Expressions.Operations.Internal;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions
{
    internal class ExpressionRepository
    {
        private static ExpressionRepository? _instance;

        private ExpressionRepository()
        {
            var configuration = new ContainerConfiguration().WithAssembly(typeof(IArithmeticOperation).Assembly);
            var container = configuration.CreateContainer();
            LoadOperations(container);
            LoadFunctions(container);
        }

        private void LoadOperations(CompositionHost container)
        {
            var operations = container.GetExports<ExportFactory<IArithmeticOperation, ArithmeticOpMetadata>>();

            // translate all arithmetic operations to a dictionary
            ArithmeticOperations = [];
            foreach (var mefArithmeticOperation in operations)
            {
                var key = new ArithmeticOpKey(
                    mefArithmeticOperation.Metadata.Operation,
                    mefArithmeticOperation.Metadata.LeftHandType,
                    mefArithmeticOperation.Metadata.RightHandType,
                    mefArithmeticOperation.Metadata.ResultType);
                ArithmeticOperations.Add(
                    key,
                    new ArithmeticOperation(mefArithmeticOperation.CreateExport().Value, key));
            }
        }

        private void LoadFunctions(CompositionHost container)
        {
            var functions= container.GetExports<ExportFactory<IInternalFunction, InternalFunctionMetadata>>();

            // translate all arithmetic operations to a dictionary
            InternalFunctions = [];
            foreach (var mefFunc in functions)
            {
                InternalFunctions.Add(
                    mefFunc.Metadata.Prototype,
                    new Tuple<IInternalFunction, InternalFunctionMetadata>(mefFunc.CreateExport().Value, mefFunc.Metadata));
            }
        }

        /// <summary>
        ///     Gets a singleton instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ExpressionRepository Instance { get; } = _instance ??= new ExpressionRepository();

        private Dictionary<string, Tuple<IInternalFunction, InternalFunctionMetadata>>
            InternalFunctions { get; set; } = null!;
        
        private Dictionary<ArithmeticOpKey, ArithmeticOperation> ArithmeticOperations { get; set; } = null!;

        /// <summary>
        ///     Gets the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>ArithmeticOperation</returns>
        public ArithmeticOperation? Get(int operation, BaseTypes left, BaseTypes right)
        {
            var key = new ArithmeticOpKey(operation, left, right);
            return ArithmeticOperations.GetValueOrDefault(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prototype"></param>
        /// <returns></returns>
        public Tuple<IInternalFunction, InternalFunctionMetadata>? GetInternalFunction(string prototype)
        {
            return InternalFunctions.GetValueOrDefault(prototype);
        }
    }
}
