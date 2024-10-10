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

namespace Oberon0.Generator.MsilBin.Tests.Calls
{
    [Collection("Sequential")]
    public class ArrayCallTests(ITestOutputHelper output)
    {
        [Fact]
        public void TestArrayCallByReference()
        {
            const string source = """
                                  MODULE Test; 
                                  TYPE 
                                    tArray = ARRAY 5 OF INTEGER;
                                  VAR 
                                      arr: tArray; 
                                  
                                      PROCEDURE TestArray(VAR a: tArray);
                                      BEGIN
                                          IF (a[1] # 1) THEN WriteString('a is 0') END;
                                          a[1] := 2
                                      END TestArray;

                                  BEGIN
                                      arr[1] := 1;
                                      TestArray(arr);
                                      WriteBool(arr[1] = 2);
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
            Assert.Equal($"{true}\n", output1.ToString().NlFix());
        }

        [Fact]
        public void TestArrayCallByValue()
        {
            const string source = """
                                  MODULE Test; 
                                  TYPE 
                                      aType = ARRAY 5 OF INTEGER;
                                  VAR 
                                      arr: aType; 
                                  
                                      PROCEDURE TestArray(a: aType);
                                      BEGIN
                                          IF (a[1] # 1) THEN WriteString('a is 0') END;
                                          a[1] := 2
                                      END TestArray;

                                  BEGIN
                                      arr[1] := 1;
                                      TestArray(arr);
                                      WriteBool(arr[1] = 1);
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
            Assert.Equal($"{true}\n", output1.ToString().NlFix());
        }
    }
}
