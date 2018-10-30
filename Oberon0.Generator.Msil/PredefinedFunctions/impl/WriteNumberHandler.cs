#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteNumberHandler.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil/WriteNumberHandler.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Generator.Msil.PredefinedFunctions.impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using JetBrains.Annotations;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions;
    using Oberon0.Compiler.Types;

    [StandardFunctionMetadata("WriteInt", TypeDefinition.VoidTypeName, "INTEGER")]
    [StandardFunctionMetadata("WriteBool", TypeDefinition.VoidTypeName, "BOOLEAN")]
    [StandardFunctionMetadata("WriteReal", TypeDefinition.VoidTypeName, "REAL")]
    [UsedImplicitly]
    public class WriteNumberHandler : IStandardFunctionGenerator
    {
        public void Generate(
            IStandardFunctionMetadata metadata,
            CodeGenerator generator,
            FunctionDeclaration functionDeclaration,
            List<Expression> parameters,
            Block block)
        {
            ProcedureParameter parameter = functionDeclaration.Block.Declarations.OfType<ProcedureParameter>().First();
            if ((parameter.Type.BaseTypes & BaseTypes.SimpleType) == 0)
            {
                throw new ArgumentException("Argument must be simple type", nameof(parameters));
            }

            if (parameter.IsVar)
            {
                VariableReferenceExpression reference = (VariableReferenceExpression)parameters[0];
                generator.Load(block, reference.Declaration, reference.Selector);
            }
            else
            {
                generator.ExpressionCompiler(functionDeclaration.Block.Parent, parameters[0]);
            }

            generator.Code.Emit("call", "void", $"[mscorlib]System.Console::Write({Code.GetTypeName(parameter.Type.BaseTypes)})");
        }
    }
}