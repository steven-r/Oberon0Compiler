#region copyright
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
    public class RealTests(ITestOutputHelper output)
    {
        private void CheckCodeReal(string sourceCode, string expectedResults)
        {
            // ReSharper disable once StringLiteralTypo
            string source = $"""
                             MODULE Test; 
                                CONST c = 1.2; i = 42; 
                                VAR r, s, t, x, y, z: REAL; 
                             BEGIN {sourceCode} 
                                IF isinfinity(z) THEN WriteString('Infinity') ELSE WriteReal(z) END; 
                                WriteLn
                             END Test.
                             """;

            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);
            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.True(assembly != null);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal(expectedResults, output1.ToString().NlFix());
        }

        [Fact]
        public void TestEpsilon()
        {
            const string source = """
                                  MODULE Test; 
                                  CONST 
                                      expected = 10.8511834932;
                                  VAR 
                                      r, s, z: REAL; 
                                      b, c: BOOLEAN;

                                  BEGIN
                                      r := 1.5;
                                      s := 7.2341223288;
                                      z := r * s;
                                      b := (r - expected) < EPSILON;
                                      c := r = expected;
                                      WriteBool(b);
                                      WriteString(',');
                                      WriteBool(c);
                                      WriteLn 
                                  END Test.
                                  """;
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);
            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.True(assembly != null);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal($"{true},{false}\n", output1.ToString().NlFix());
        }

        [Fact]
        public void TestRealComplex1()
        {
            CheckCodeReal("r := 1.2345678; s := 7.2; z := r*s DIV 3;", $"{2.96296272}\n");
        }

        [Fact]
        public void TestRealDiv0()
        {
            CheckCodeReal("r := 1.2345678; s := 0; z := r DIV s;", "Infinity\n");
        }

        [Fact]
        public void TestRealFromConst()
        {
            CheckCodeReal("z := c;", $"{1.2}\n");
        }

        [Fact]
        public void TestRealFromInt()
        {
            CheckCodeReal("z := 1;", "1\n");
        }

        [Fact]
        public void TestRealFromIntConst()
        {
            CheckCodeReal("z := i;", "42\n");
        }

        [Fact]
        public void TestRealFromPlus1()
        {
            CheckCodeReal("z := 1.2+1.2;", $"{2.4}\n");
        }

        [Fact]
        public void TestRealFromPlus2()
        {
            CheckCodeReal("r := 1.2; z := r+1.2;", $"{2.4}\n");
        }

        [Fact]
        public void TestRealFromPlus3()
        {
            CheckCodeReal("z := c+c;", $"{2.4}\n");
        }

        [Fact]
        public void TestRealFromReal()
        {
            CheckCodeReal("z := 1.2;", $"{1.2}\n");
        }
    }
}
