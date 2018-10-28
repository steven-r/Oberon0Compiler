#region copyright
//--------------------------------------------------------------------------------------------------------------------
// <copyright file="ArithmeticOpMetadata.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/ArithmeticOpMetadataView.cs
// </summary>
//--------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Expressions.Operations.Internal
{
    using Oberon0.Compiler.Types;

    public class ArithmeticOpMetadata
    {
        public BaseTypes LeftHandType { get; set; }

        public int Operation { get; set; }

        public BaseTypes ResultType { get; set; }

        public BaseTypes RightHandType { get; set; }
    }
}