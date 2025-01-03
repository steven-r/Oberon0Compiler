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
    public class RecordTests(ITestOutputHelper output)
    {
        [Fact]
        public void Test1()
        {
            const string source = """
                                  MODULE Test; 
                                  TYPE 
                                    rType = RECORD
                                      a: INTEGER;
                                      b: STRING
                                    END;

                                  VAR 
                                    demo: rType;

                                  BEGIN
                                    demo.a := 1;
                                    WriteInt(demo.a);
                                    WriteLn
                                  END Test.
                                  """;
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            byte[] assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.NotNull(assembly);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal("1\n", output1.ToString().NlFix());
        }
    }
}
