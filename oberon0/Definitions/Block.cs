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
using Oberon0.Compiler.Exceptions;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Statements;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Definitions
{
    public class Block(Block? parent, Module module)
    {
        public Module Module { get; } = module;

        public List<Declaration> Declarations { get; } = [];


        public Block? Parent { get; } = parent;

        public List<FunctionDeclaration> Procedures { get; } = [];

        public List<IStatement> Statements { get; } = [];

        public List<TypeDefinition> Types { get; } = [];

        public FunctionDeclaration? LookupFunction(string name, IToken token, params Expression[] parameters)
        {
            var callParameters = CallParameter.CreateFromExpressionList(parameters);
            return LookupFunction(name, token, callParameters);
        }

        // test only
        public FunctionDeclaration? LookupFunction(string name)
        {
            return LookupFunction(name, new CommonToken(0), new List<CallParameter>());
        }

        // test only
        public FunctionDeclaration? LookupFunction(string name, string parameters)
        {
            var callParameters = CallParameter.CreateFromStringExpression(this, parameters);
            return LookupFunction(name, new CommonToken(0), callParameters);
        }

        private FunctionDeclaration? LookupFunction(string procedureName, IToken token,
                                                    IReadOnlyList<CallParameter> callParameters)
        {
            var b = this;
            FunctionDeclaration? resFunc = null;
            // try to find the function with the best match (score) (e.g. ABS(REAL) and ABS(INTEGER) share the same function name
            // of not found on current block level, move up (until root) to find something appropriate.
            int score = -1;
            while (b != null)
            {
                var res = b.Procedures.Where(x => x.Name == procedureName);
                foreach (var func in res)
                {
                    int newScore = GenerateFunctionParameterScore(func, callParameters);

                    if (newScore <= score)
                    {
                        continue;
                    }

                    resFunc = func;
                    score = newScore;
                }

                if (score >= 0)
                {
                    break; // found
                }

                b = b.Parent;
            }

            if (resFunc != null)
            {
                return resFunc;
            }

            var parameterList = new List<ProcedureParameterDeclaration>(callParameters.Count);
            int n = 0;

            parameterList.AddRange(
                callParameters.Select(
                    expression =>
                        new ProcedureParameterDeclaration("param_" + n++, this, expression.TargetType, false)));

            string prototype = FunctionDeclaration.BuildPrototype(
                procedureName,
                SimpleTypeDefinition.VoidType,
                parameterList.ToArray());
            if (Module.CompilerInstance != null)
            {
                Module.CompilerInstance.Parser.NotifyErrorListeners(
                    token,
                    $"No procedure/function with prototype '{prototype}' found",
                    null);
            } else
            {
                throw new InvalidOperationException($"No procedure/function with prototype '{prototype}' found");
            }

            return resFunc;
        }

        public TypeDefinition? LookupType(string name)
        {
            var b = this;
            while (b != null)
            {
                var res = b.Types.Find(x => x.Name == name);
                if (res != null)
                {
                    return res;
                }

                b = b.Parent;
            }

            return null;
        }

        /// <summary>
        ///     Lookups the type based on <see cref="BaseTypes" />.
        /// </summary>
        /// <param name="baseTypes">The base type.</param>
        /// <returns>The <see cref="TypeDefinition" />.</returns>
        public TypeDefinition LookupTypeByBaseType(BaseTypes baseTypes)
        {
            var b = this;

            // internal types are only available on level 0
            while (b.Parent != null)
            {
                b = b.Parent;
            }

            var res = b.Types.Find(x => x.Type == baseTypes && x.IsInternal);
            if (res == null)
            {
                throw new InternalCompilerException($"LookupTypeByBaseType: Cannot lookup type {baseTypes:G}");
            }

            return res;
        }

        /// <summary>
        ///     Lookups a variable.
        /// </summary>
        /// <param name="name">The variable name.</param>
        /// <param name="lookupParents">The parents</param>
        /// <returns>The <see cref="Declaration" />.</returns>
        internal Declaration? LookupVar(string name, bool lookupParents = true)
        {
            var b = this;
            while (b != null)
            {
                var res = b.Declarations.Find(x => x.Name == name);
                if (res != null)
                {
                    return res;
                }

                if (!lookupParents)
                {
                    return null; // nothing in local env
                }

                b = b.Parent;
            }

            return null;
        }

        /// <summary>
        ///     Create a score to create the best fit for a function given a parameter list.
        /// </summary>
        /// <param name="func">The function to check the parameters for</param>
        /// <param name="parameters">The list of parameters</param>
        /// <returns>A score (see remarks). In case of -1 no match is possible (see cases in code)</returns>
        /// <remarks>
        ///     How the score is calculated:
        ///     * Start with a score of 0
        ///     * For each parameter
        ///     ** If IsVar and types match exactly --> +1000 (super match)
        ///     ** If ByValue and types match exactly --> + 1000 (only checked for simple types)
        ///     ** If ByValue and types are compatible (e.g. INTEGER can be assigned to REAL) --> +1
        ///     In the unlikely event of having more than 1000 parameters, this algorithm might fail.
        /// </remarks>
        private static int GenerateFunctionParameterScore(FunctionDeclaration func,
                                                          IReadOnlyList<CallParameter> parameters)
        {
            int paramNo = 0;
            int score = 0;
            var procParams = func.Block.Declarations.OfType<ProcedureParameterDeclaration>().ToArray();
            if (parameters.Count != procParams.Length)
            {
                return -1; // will never match
            }

            foreach (var procedureParameter in procParams)
            {
                if (procedureParameter.IsVar)
                {
                    if (!GenerateVarParameterCount(parameters[paramNo], procedureParameter, ref score))
                    {
                        return -1;
                    }
                } else if (procedureParameter.Type.Type.HasFlag(BaseTypes.Simple) &&
                    procedureParameter.Type.Name == parameters[paramNo].TypeName)
                {
                    score += 1000;
                } else if (!procedureParameter.Type.IsAssignable(parameters[paramNo].TargetType))
                {
                    return -1;
                } else
                {
                    score++; // match as assignable --> small increment
                }

                paramNo++;
            }

            return score;
        }

        private static bool GenerateVarParameterCount(CallParameter callParameter, Declaration procedureParameter, ref int score)
        {
            if (callParameter.CanBeVarReference)
            {
                if (!procedureParameter.Type.Equals(callParameter.TargetType))
                {
                    // VAR parameter need to have the same type as calling parameter
                    return false;
                }

                score += 1000;
            } else
            {
                // VAR parameter cannot have expression as source
                return false;
            }

            return true;
        }
    }
}
