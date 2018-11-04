#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Block.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/Block.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using JetBrains.Annotations;

    using Oberon0.Compiler.Expressions;
    using Oberon0.Compiler.Generator;
    using Oberon0.Compiler.Statements;
    using Oberon0.Compiler.Types;

    public class Block
    {
        public Block(Block parent)
        {
            this.InitLists();
            this.Parent = parent;
        }

        protected Block()
        {
            this.InitLists();
        }

        public List<Declaration> Declarations { get; private set; }

        /// <summary>
        /// Gets or sets additional information used by the generator engine
        /// </summary>
        /// <value>Generator information.</value>
        [UsedImplicitly]
        public IGeneratorInfo GeneratorInfo { get; set; }

        public Block Parent { get; set; }

        public List<FunctionDeclaration> Procedures { get; private set; }

        public List<IStatement> Statements { get; private set; }

        public List<TypeDefinition> Types { get; private set; }

        /// <summary>
        /// Lookup a procedure definition
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <returns>the <see cref="FunctionDeclaration"/>.</returns>
        public FunctionDeclaration LookupFunction(string procedureName)
        {
            Block b = this;
            while (b != null)
            {
                var res = b.Procedures.FirstOrDefault(x => x.Name == procedureName);
                if (res != null)
                    return res;
                b = b.Parent;
            }

            return null;
        }

        public TypeDefinition LookupType(string name)
        {
            Block b = this;
            if (name.StartsWith("&", StringComparison.InvariantCulture))
                name = name.Substring(1);
            while (b != null)
            {
                var res = b.Types.FirstOrDefault(x => x.Name == name);
                if (res != null)
                    return res;
                b = b.Parent;
            }

            return null;
        }

        /// <summary>
        /// Lookups the type based on <see cref="BaseTypes"/>.
        /// </summary>
        /// <param name="baseTypes">The base type.</param>
        /// <returns>The <see cref="TypeDefinition"/>.</returns>
        public TypeDefinition LookupTypeByBaseType(BaseTypes baseTypes)
        {
            Block b = this;

            // internal types are only available on level 0
            while (b.Parent != null)
            {
                b = b.Parent;
            }

            var res = b.Types.FirstOrDefault(x => x.Type == baseTypes && x.IsInternal);
            return res;
        }

        internal FunctionDeclaration LookupFunction(string procedureName, IList<Expression> parameters)
        {
            Block b = this;
            while (b != null)
            {
                var functionDeclaration = CheckForFunction(procedureName, parameters, b);
                if (functionDeclaration != null)
                {
                    return functionDeclaration;
                }

                b = b.Parent;
            }

            return null;
        }

        /// <summary>
        /// Lookups a variable.
        /// </summary>
        /// <param name="name">The variable name.</param>
        /// <returns><see cref="Declaration"/>.</returns>
        internal Declaration LookupVar(string name)
        {
            return this.LookupVar(name, true);
        }

        /// <summary>
        /// Lookups a variable.
        /// </summary>
        /// <param name="name">The variable name.</param>
        /// <param name="lookupParents">The parents</param>
        /// <returns>The <see cref="Declaration"/>.</returns>
        internal Declaration LookupVar(string name, bool lookupParents)
        {
            Block b = this;
            while (b != null)
            {
                var res = b.Declarations.FirstOrDefault(x => x.Name == name);
                if (res != null)
                    return res;
                if (!lookupParents)
                {
                    return null; // nothing in local env
                }

                b = b.Parent;
            }

            return null;
        }

        private static FunctionDeclaration CheckForFunction(string procedureName, IList<Expression> parameters, Block b)
        {
            var res = b.Procedures.Where(x => x.Name == procedureName);
            foreach (FunctionDeclaration func in res)
            {
                var paramList = func.Block.GetParameters();
                if (paramList.Count != parameters.Count) continue;
                bool found = true;
                for (int i = 0; i < parameters.Count; i++)
                {
                    if (paramList[i].Type.Type != parameters[i].TargetType.Type)
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                    return func;
            }

            return null;
        }

        private IList<ProcedureParameter> GetParameters()
        {
            return this.Declarations.OfType<ProcedureParameter>().ToList();
        }

        private void InitLists()
        {
            Declarations = new List<Declaration>();
            Types = new List<TypeDefinition>();
            Statements = new List<IStatement>();
            Procedures = new List<FunctionDeclaration>();
        }
    }
}