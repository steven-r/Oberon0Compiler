﻿#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadNumHandler.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil/ReadHandler.cs
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

    [StandardFunctionMetadata("ReadInt", TypeDefinition.VoidTypeName, "&INTEGER")]
    [StandardFunctionMetadata("ReadBool", TypeDefinition.VoidTypeName, "&BOOL")]
    [UsedImplicitly]
    public class ReadNumHandler : IStandardFunctionGenerator
    {
        public void Generate(
            IStandardFunctionMetadata metadata,
            CodeGenerator generator,
            FunctionDeclaration functionDeclaration,
            List<Expression> parameters,
            Block block)
        {
            VariableReferenceExpression reference = (VariableReferenceExpression)parameters[0];

            generator.StartStoreVar(block, reference.Declaration, reference.Selector);
            generator.Code.WriteLine("\tcall string [mscorlib]System.Console::ReadLine()");
            switch (functionDeclaration.Name)
            {
                case "ReadInt":
                    generator.Code.WriteLine("\tcall int32 [mscorlib]System.Int32::Parse(string)");
                    break;
                case "ReadBool":
                    generator.Code.WriteLine("\tcall int32 [mscorlib]System.Bool::Parse(string)");
                    break;
            }

            generator.StoreVar(block, reference.Declaration, reference.Selector);
        }
    }
}