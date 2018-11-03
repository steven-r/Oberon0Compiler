#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FunctionDeclaration.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/FunctionDeclaration.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Definitions
{
    using System.Linq;

    using JetBrains.Annotations;

    using Oberon0.Compiler.Types;

    public class FunctionDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionDeclaration"/> class defining a function returning <c>VOID</c>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="parameters">The parameters.</param>
        public FunctionDeclaration(string name, Block parent, params ProcedureParameter[] parameters)
        {
            this.Name = name;
            this.Block = new Block(parent);
            if (parameters != null)
            {
                this.Block.Declarations.AddRange(parameters);
            }

            this.ReturnType = new SimpleTypeDefinition(BaseTypes.Void);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionDeclaration"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="returnType">Type of the return.</param>
        /// <param name="parameters">The parameters.</param>
        public FunctionDeclaration(
            string name,
            Block parent,
            TypeDefinition returnType,
            params ProcedureParameter[] parameters)
        {
            this.Name = name;
            this.Block = new Block(parent);
            this.Block.Declarations.AddRange(parameters);
            this.ReturnType = returnType;
        }

        public Block Block { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is external.
        /// </summary>
        /// <value><c>true</c> if this instance is external; otherwise, <c>false</c>.</value>
        public bool IsExternal { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is internal (e.g. <c>WriteInt()</c>
        /// </summary>
        /// <value><c>true</c> if this instance is internal; otherwise, <c>false</c>.</value>
        public bool IsInternal { get; private set; }

        public string Name { get; }

        public string Prototype { [UsedImplicitly] get; internal set; }

        public TypeDefinition ReturnType { get; private set; }

        public static FunctionDeclaration AddHardwiredFunction(
            string name,
            Module module,
            params ProcedureParameter[] parameters)
        {
            var res = new FunctionDeclaration(name, module.Block, parameters)
                          {
                              IsInternal = true,
                              ReturnType = module.Block.LookupType(TypeDefinition.VoidTypeName),
                              Prototype = $"Oberon0.{module.Name}::{name}"
                          };
            return res;
        }

        public override string ToString()
        {
            var @params = string.Join(
                              ", ",
                              this.Block.Declarations.OfType<ProcedureParameter>()
                                  .Select(x => x.Type.Type.ToString("G")));
            return $"{(IsInternal ? "internal " : string.Empty)}{ReturnType:G} {Name}(" + @params + ")";
        }
    }
}