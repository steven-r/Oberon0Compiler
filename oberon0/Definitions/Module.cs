#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Module.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/Module.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Definitions
{
    using System.Collections.Generic;
    using System.Reflection;

    using JetBrains.Annotations;

    using Oberon0.Compiler.Generator;

    /// <summary>
    /// The module.
    /// </summary>
    public partial class Module
    {
        public Module()
        {
            this.Block = new Block(null);
            this.ExternalReferences = new List<Assembly>();

            this.DeclareStandardTypes();
            this.DeclareStandardConsts();
            this.DeclareStandardFunctions();
        }

        /// <summary>
        /// Gets the block.
        /// </summary>
        public Block Block { get; }

        /// <summary>
        /// Gets external references.
        /// </summary>
        /// <value>The external references.</value>
        public List<Assembly> ExternalReferences { get; }

        /// <summary>
        /// Gets or sets additional information used by the generator engine
        /// </summary>
        /// <value>Generator information.</value>
        [UsedImplicitly]
        public IGeneratorInfo GeneratorInfo { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the module contains export statements. In this case the output will be a DLL.
        /// </summary>
        public bool HasExports { get; set; }
    }
}