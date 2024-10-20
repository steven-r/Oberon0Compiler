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

namespace Oberon0.Generator.MsilBin.Tests.Calls
{
    [Collection("Sequential")]
    public class ProcedureTests(ITestOutputHelper output)
    {
        [Fact]
        public void TestProcedureBoolean()
        {
            const string source = """

                                  MODULE ProcTest;
                                  CONST
                                    true_ = TRUE;

                                  VAR
                                    x, y: BOOLEAN;
                                    i, j: BOOLEAN;
                                  
                                    PROCEDURE Test(x: BOOLEAN; y: BOOLEAN; VAR a: BOOLEAN; b: BOOLEAN);
                                    BEGIN
                                      x := FALSE; y := true_;
                                      a := TRUE; b := FALSE;
                                    END Test;

                                  BEGIN
                                      x := TRUE; y := x = true_;
                                      i := FALSE; j := FALSE;
                                      Test(y, x, i, j);
                                      WriteBool(x);
                                      WriteString(', ');
                                      WriteBool(y);
                                      WriteString(', ');
                                      WriteBool(i);
                                      WriteString(', ');
                                      WriteBool(j);
                                      WriteLn
                                  END ProcTest.

                                  """;
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            byte[] assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.NotNull(assembly);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal($"{true}, {true}, {true}, {false}\n", output1.ToString().NlFix());
        }

        [Fact]
        public void TestProcedureInteger()
        {
            const string source = """
                                  MODULE ProcTest;
                                  VAR
                                    x, y: INTEGER;
                                    i, j: INTEGER;
                                  
                                    PROCEDURE Test(x: INTEGER; y: INTEGER; VAR a: INTEGER; b: INTEGER);
                                    BEGIN
                                      x := 0; y := -1;
                                      a := 10; b := 20;
                                    END Test;

                                  BEGIN
                                      x := 42; y := 1;
                                      i := 2; j := 0;
                                      Test(y, x, i, j);
                                      WriteBool(x = 42);
                                      WriteString(', ');
                                      WriteBool(y = 1);
                                      WriteString(', ');
                                      WriteBool(i = 10);
                                      WriteString(', ');
                                      WriteBool(j = 0);
                                      WriteLn
                                  END ProcTest.
                                  """;

            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            byte[] assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.NotNull(assembly);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal($"{true}, {true}, {true}, {true}\n", output1.ToString().NlFix());
        }
    }
}
