#region copyright

// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------

#endregion

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using JetBrains.Annotations;
using Oberon0.Compiler.Generator;

namespace Oberon0.Compiler.Definitions
{
    /// <summary>
    ///     The module.
    /// </summary>
    public partial class Module
    {
        public Module(Oberon0Compiler compilerInstance)
        {
            CompilerInstance = compilerInstance;

            Block = new Block(null, this);
            ExternalReferences = new List<Assembly>();

            DeclareStandardTypes();
            DeclareStandardConstants();
            DeclareStandardFunctions();
        }

        /// <summary>
        ///     Gets the compiler instance this Module has been build with
        /// </summary>
        public Oberon0Compiler CompilerInstance { get; set; }

        /// <summary>
        ///     Gets the block.
        /// </summary>
        public Block Block { get; }

        /// <summary>
        ///     Gets external references.
        /// </summary>
        /// <value>The external references.</value>
        public List<Assembly> ExternalReferences { get; }

        /// <summary>
        ///     Gets or sets additional information used by the generator engine
        /// </summary>
        /// <value>Generator information.</value>
        [UsedImplicitly]
        [ExcludeFromCodeCoverage]
        public IGeneratorInfo GeneratorInfo { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the module contains export statements. In this case the output will be a
        ///     DLL.
        /// </summary>
        public bool HasExports { get; set; }
    }
}
