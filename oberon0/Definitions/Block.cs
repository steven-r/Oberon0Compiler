#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using JetBrains.Annotations;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Generator;
using Oberon0.Compiler.Statements;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Definitions
{
    public class Block
    {
        public Block(Block parent, Module module)
        {
            Module = module;
            InitLists();
            Parent = parent;
        }

        public Module Module { get; }

        public List<Declaration> Declarations { get; private set; }


        public Block Parent { get; }

        public List<FunctionDeclaration> Procedures { get; private set; }

        public List<IStatement> Statements { get; private set; }

        public List<TypeDefinition> Types { get; private set; }

#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public FunctionDeclaration LookupFunction(string name, IToken token, params Expression[] parameters)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            var callParameters = CallParameter.FromExpressions(parameters);
            return this.LookupFunction(name, token, SimpleTypeDefinition.VoidType, callParameters);
        }

#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public FunctionDeclaration LookupFunction(string name, IToken token, string parameters)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            var callParameters = CallParameter.FromExpressions(this, parameters);
            return this.LookupFunction(name, token, SimpleTypeDefinition.VoidType, callParameters);
        }

#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public FunctionDeclaration LookupFunction(string procedureName, IToken token, TypeDefinition returnType, IReadOnlyList<CallParameter> callParameters)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            Block b = this;
            FunctionDeclaration resFunc = null;
            var score = -1;
            while (b != null)
            {
                var res = b.Procedures.Where(x => x.Name == procedureName);
                foreach (FunctionDeclaration func in res)
                {
                    var newScore = FunctionParameterMatch(func, callParameters);

                    if (newScore <= score) continue;

                    resFunc = func;
                    score = newScore;
                }

                if (score >= 0) break; // found

                b = b.Parent;
            }

            if (resFunc == null)
            {
                var parameterList = new List<ProcedureParameterDeclaration>(callParameters.Count);
                var n = 0;

                parameterList.AddRange(
                    callParameters.Select(
                        expression => new ProcedureParameterDeclaration("param_" + n++, this, expression.TargetType, false)));

                var prototype = FunctionDeclaration.BuildPrototype(
                    procedureName,
                    returnType,
                    parameterList.ToArray());
                Module.CompilerInstance.Parser.NotifyErrorListeners(
                    token,
                    $"No procedure/function with prototype '{prototype}' found",
                    null);
            }

            return resFunc;
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
            while (b.Parent != null) b = b.Parent;

            var res = b.Types.FirstOrDefault(x => x.Type == baseTypes && x.IsInternal);
            return res;
        }

        /// <summary>
        /// Lookups a variable.
        /// </summary>
        /// <param name="name">The variable name.</param>
        /// <returns><see cref="Declaration"/>.</returns>
        internal Declaration LookupVar(string name)
        {
            return LookupVar(name, true);
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
                if (!lookupParents) return null; // nothing in local env

                b = b.Parent;
            }

            return null;
        }

        private int FunctionParameterMatch(FunctionDeclaration func, IReadOnlyList<CallParameter> parameters)
        {
            var paramNo = 0;
            var score = 0;
            var procParams = func.Block.Declarations.OfType<ProcedureParameterDeclaration>().ToArray();
            if (parameters.Count != procParams.Length) return -1; // will never match

            foreach (var procedureParameter in procParams)
                if (procedureParameter.IsVar)
                {
                    if (parameters[paramNo].CanBeVarReference)
                    {
                        if (!procedureParameter.Type.Equals(parameters[paramNo].TargetType))
                            // VAR parameter need to have the same type as calling parameter
                            return -1;

                        score += 1000;
                    }
                    else
                    {
                        // VAR parameter cannot have expression as source
                        return -1;
                    }
                }
                else if (procedureParameter.Type.ToString() == parameters[paramNo].TypeName)
                {
                    score += 1000;
                }
                else if (!procedureParameter.Type.IsAssignable(parameters[paramNo].TargetType))
                {
                    return -1;
                }
                else
                {
                    score++; // match as assignable --> small increment
                }

            return score;
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