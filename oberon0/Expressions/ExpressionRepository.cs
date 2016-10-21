using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions.Operations;
using JetBrains.Annotations;

namespace Oberon0.Compiler.Expressions
{
    class ExpressionRepository
    {

#pragma warning disable 649
        [ImportMany]
        // ReSharper disable once CollectionNeverUpdated.Local
        // ReSharper disable once MemberCanBePrivate.Local
        private Lazy<IArithmeticOperation, IArithmeticOpMetadataView>[] MefArithmeticOperations { get; set; }
#pragma warning restore 649

        private Dictionary<ArithmeticOpKey, ArithmeticOperation> ArithmeticOperations { get; }

        private ExpressionRepository()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(IArithmeticOperation).Assembly));

            var container = new CompositionContainer(catalog,
                CompositionOptions.DisableSilentRejection |
                CompositionOptions.IsThreadSafe);
            container.ComposeParts(this);

            // translate all arithmentic operations to a dictionary
            ArithmeticOperations = new Dictionary<ArithmeticOpKey, ArithmeticOperation>();
            foreach (Lazy<IArithmeticOperation, IArithmeticOpMetadataView> mefArithmeticOperation in MefArithmeticOperations)
            {
                for (int i = 0; i < mefArithmeticOperation.Metadata.Operation.Length; i++)
                {
                    var key = new ArithmeticOpKey(mefArithmeticOperation.Metadata.Operation[i],
                        mefArithmeticOperation.Metadata.LeftHandType[i],
                        mefArithmeticOperation.Metadata.RightHandType[i],
                        mefArithmeticOperation.Metadata.TargetType[i]);
                    ArithmeticOperations.Add(key, new ArithmeticOperation(mefArithmeticOperation.Value, key));
                }
            }
        }


        private static ExpressionRepository _instance = null;
        /// <summary>
        /// Gets a singleton instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ExpressionRepository Instance { get; } = _instance ?? (_instance = new ExpressionRepository());

        /// <summary>
        /// Gets the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>Lazy&lt;IArithmeticOperation, IArithmeticOpMetadata&gt;.</returns>
        [NotNull]
        public ArithmeticOperation Get(TokenType operation, BaseType left, BaseType right)
        {
            ArithmeticOperation op;
            var key = new ArithmeticOpKey(operation, left, right);
            if (!ArithmeticOperations.TryGetValue(key, out op))
                throw new InvalidOperationException(
                    $"Cannot find operation {operation:G} ({left:G}, {right:G})");
            return op;
        }
    }
}
