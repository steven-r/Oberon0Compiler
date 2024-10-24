﻿#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.IO;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;
using Xunit.Abstractions;

namespace Oberon0.Generator.MsilBin.Tests.Types
{
    public class VarTests(ITestOutputHelper output)
    {
        [Fact]
        public void TestReservedWordIssue23()
        {
            const string source = """
                                  MODULE Test; 
                                  VAR 
                                      int: INTEGER; 

                                  BEGIN
                                      int := 1;
                                      WriteInt(int);
                                      WriteLn 
                                  END Test.
                                  """;
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            byte[] assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.True(assembly != null);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal("1\n", output1.ToString().NlFix());
        }
    }
}
