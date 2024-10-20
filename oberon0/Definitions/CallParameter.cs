﻿#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Definitions
{
    /// <summary>
    ///     A call parameter.
    /// </summary>
    public class CallParameter
    {
        /// <summary>
        ///     Gets a value indicating whether the parameter can be used as a var reference.
        /// </summary>
        public bool CanBeVarReference { get; init; }

        /// <summary>
        ///     Gets the target type.
        /// </summary>
        public required TypeDefinition TargetType { get; init; }

        /// <summary>
        ///     Gets the type name.
        /// </summary>
        public required string TypeName { get; init; }

        /// <summary>
        ///     Create a parameter list from a list of expressions.
        /// </summary>
        /// <param name="expressions">
        ///     The expressions.
        /// </param>
        /// <returns>
        ///     The <see cref="IReadOnlyList{T}" />.
        /// </returns>
        public static IReadOnlyList<CallParameter> CreateFromExpressionList(params Expression[] expressions)
        {
            var list = new List<CallParameter>(expressions.Length);
            list.AddRange(expressions.Select(FromExpression));

            return list;
        }

        /// <summary>
        ///     Create a parameter list from a <see cref="string" />.
        /// </summary>
        /// <param name="block">The referenced <see cref="Block" /></param>
        /// <param name="parameters">
        ///     The parameters separated by ",". For reference parameters, prefix the type with an ampersand ('&amp;')
        /// </param>
        /// <returns>
        ///     The list of call parameters or <c>null</c> in case of error.
        /// </returns>
        /// <remarks>This function is for testing only</remarks>
        [ExcludeFromCodeCoverage]
        internal static IReadOnlyList<CallParameter> CreateFromStringExpression(
            Block block, string? parameters = null)
        {
#if !DEBUG
            throw new InvalidOperationException("CreateFromStringExpression is only valid for test configurations");
#else
            ArgumentNullException.ThrowIfNull(block);

            var resultList = new List<CallParameter>();
            if (parameters == null)
            {
                return resultList;
            }

            foreach (string element in parameters.Split(','))
            {
                (var targetType, bool isVar) = Module.GetParameterDeclarationFromString(element, block);
                var callParameter = new CallParameter()
                {
                    CanBeVarReference = isVar,
                    TargetType = targetType,
                    TypeName = targetType.Name ?? "<unset>"
                };

                resultList.Add(callParameter);
            }

            return resultList;
#endif
        }

        /// <summary>
        ///     Create a <see cref="CallParameter" /> from an <see cref="Expression" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <returns>
        ///     The <see cref="CallParameter" />.
        /// </returns>
        private static CallParameter FromExpression(Expression expression)
        {
            return new CallParameter
            {
                CanBeVarReference = expression is VariableReferenceExpression,
                TargetType = expression.TargetType,
                TypeName = expression.TargetType.Name ?? "<unset>"
            };
        }
    }
}
