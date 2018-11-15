#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MathAbs.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil/MathAbs.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Generator.Msil.PredefinedFunctions.impl
{
    using System.Collections.Generic;

    using JetBrains.Annotations;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions;
    using Oberon0.Compiler.Types;

    [StandardFunctionMetadata("ABS", "REAL", "REAL")]
    [StandardFunctionMetadata("ABS", "INTEGER", "INTEGER")]
    [UsedImplicitly]
    public class MathAbs : IStandardFunctionGenerator
    {
        public void Generate(
            IStandardFunctionMetadata metadata,
            CodeGenerator generator,
            FunctionDeclaration functionDeclaration,
            IReadOnlyList<Expression> parameters,
            Block block)
        {
            if (metadata.ReturnType.Type == BaseTypes.Int)
            {
                generator.Code.Emit("call", "int32", "[mscorlib]System.Math::Abs(int32)");
            }
            else
            {
                generator.Code.Emit("call", "float64", "[mscorlib]System.Math::Abs(float64)");
            }
        }
    }
}