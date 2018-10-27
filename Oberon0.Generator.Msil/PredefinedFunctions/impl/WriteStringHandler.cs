#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteStringHandler.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil/WriteStringHandler.cs
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

    [StandardFunctionMetadata("WriteString", TypeDefinition.VoidTypeName, "STRING")]
    [UsedImplicitly]
    public class WriteStringHandler : IStandardFunctionGenerator
    {
        public void Generate(
            IStandardFunctionMetadata metadata,
            CodeGenerator generator,
            FunctionDeclaration callExpression,
            List<Expression> parameters,
            Block block)
        {
            generator.ExpressionCompiler(block, parameters[0]);
            generator.Code.Emit("call", "void", "[mscorlib]System.Console::Write(string)");
        }
    }
}