#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Oberon0ExportAttribute.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.System/Oberon0ExportAttribute.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Attributes
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// Declare a library export for the Oberon0 language
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method)]
    [UsedImplicitly]
    public class Oberon0ExportAttribute : Attribute
    {
        public Oberon0ExportAttribute([NotNull] string name, [NotNull] string returnType, params string[] parameters)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ReturnType = returnType ?? throw new ArgumentNullException(nameof(returnType));
            Parameters = parameters;
        }

        public string Name { get; }

        public string[] Parameters { get; }

        public string ReturnType { get; }
    }
}