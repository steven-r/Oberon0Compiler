#region copyright

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CallParameter.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/CallParameter.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#endregion

namespace Oberon0.Compiler.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using JetBrains.Annotations;

    using Oberon0.Compiler.Expressions;
    using Oberon0.Compiler.Types;

    /// <summary>
    /// A call parameter.
    /// </summary>
    public class CallParameter
    {
        /// <summary>
        /// Gets a value indicating whether the parameter can be used as a var reference.
        /// </summary>
        public bool CanBeVarReference { get; private set; }

        /// <summary>
        /// Gets the target type.
        /// </summary>
        public TypeDefinition TargetType { get; private set; }

        /// <summary>
        /// Gets the type name.
        /// </summary>
        public string TypeName { get; private set; }

        /// <summary>
        /// Create a parameter list from a list of expressions.
        /// </summary>
        /// <param name="expressions">
        /// The expressions.
        /// </param>
        /// <returns>
        /// The <see cref="IReadOnlyList{T}"/>.
        /// </returns>
        public static IReadOnlyList<CallParameter> FromExpressions(params Expression[] expressions)
        {
            var list = new List<CallParameter>(expressions.Length);
            foreach (var expression in expressions)
            {
                list.Add(FromExpression(expression));
            }

            return list;
        }

        /// <summary>
        /// Create a parameter list from a <see cref="string"/>.
        /// </summary>
        /// <param name="block">The referenced <see cref="Block"/></param>
        /// <param name="parameters">
        /// The parameters separated by ",". For reference parameters, prefix the type with an ampersand ('&amp;')
        /// </param>
        /// <returns>
        /// The list of call parameters or <c>null</c> in case of error.
        /// </returns>
        public static IReadOnlyList<CallParameter> FromExpressions([NotNull] Block block, string parameters = null)
        {
            if (block == null) throw new ArgumentNullException(nameof(block));

            var resultList = new List<CallParameter>();
            if (parameters == null)
            {
                return resultList;
            }

            foreach (var element in parameters.Split(','))
            {
                var callParameter = new CallParameter();
                var matches = Regex.Match(
                    element,
                    "(?<ref>&)?(?<name>[A-Za-z][A-Za-z$0-9]*)(?<isarray>\\[(?<size>\\d+)\\])?");
                if (!matches.Success)
                {
                    throw new ArgumentException($"{element} is not a valid type reference", nameof(parameters));
                }

                callParameter.CanBeVarReference = matches.Groups["ref"].Value == "&";

                var type = block.LookupType(matches.Groups["name"].Value);
                if (type == null)
                {
                    throw new ArgumentException($"{element} is not a valid type reference", nameof(parameters));
                }

                callParameter.TargetType = matches.Groups["isarray"].Success
                                               ? new ArrayTypeDefinition(int.Parse(matches.Groups["size"].Value), type)
                                               : type;

                callParameter.TypeName = callParameter.TargetType.Name;

                resultList.Add(callParameter);
            }

            return resultList;
        }

        /// <summary>
        /// Create a <see cref="CallParameter"/> from an <see cref="Expression"/>.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <returns>
        /// The <see cref="CallParameter"/>.
        /// </returns>
        private static CallParameter FromExpression(Expression expression)
        {
            return new CallParameter
                {
                    CanBeVarReference = expression is VariableReferenceExpression,
                    TargetType = expression.TargetType,
                    TypeName = expression.TargetType.Name
                };
        }
    }
}