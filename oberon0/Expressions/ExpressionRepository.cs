#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionRepository.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/ExpressionRepository.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Composition;
    using System.Composition.Hosting;

    using JetBrains.Annotations;

    using Oberon0.Compiler.Expressions.Operations.Internal;
    using Oberon0.Compiler.Types;

    internal class ExpressionRepository
    {
        private static ExpressionRepository instance;

        private ExpressionRepository()
        {
            var configuration = new ContainerConfiguration().WithAssembly(typeof(IArithmeticOperation).Assembly);
            var container = configuration.CreateContainer();
            var operations = container.GetExports<ExportFactory<IArithmeticOperation, ArithmeticOpMetadata>>();

            // translate all arithmetic operations to a dictionary
            ArithmeticOperations = new Dictionary<ArithmeticOpKey, ArithmeticOperation>();
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

        /// <summary>
        /// Gets a singleton instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ExpressionRepository Instance { get; } = instance ?? (instance = new ExpressionRepository());

        private Dictionary<ArithmeticOpKey, ArithmeticOperation> ArithmeticOperations { get; }

        /// <summary>
        /// Gets the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>Lazy&lt;IArithmeticOperation, IArithmeticOpMetadata&gt;.</returns>
        [NotNull]
        public ArithmeticOperation Get(int operation, BaseType left, BaseType right)
        {
            var key = new ArithmeticOpKey(operation, left, right);
            if (!ArithmeticOperations.TryGetValue(key, out var op))
                throw new InvalidOperationException($"Cannot find operation {operation} ({left:G}, {right:G})");
            return op;
        }
    }
}