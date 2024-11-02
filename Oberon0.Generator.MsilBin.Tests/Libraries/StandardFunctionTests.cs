#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.IO;
using Microsoft.CodeAnalysis.CSharp;
using Oberon0.Compiler.Exceptions;
using Oberon0.Compiler.Types;
using Oberon0.Generator.MsilBin.PredefinedFunctions;
using Oberon0.Test.Support;
using Xunit;
using Xunit.Abstractions;

namespace Oberon0.Generator.MsilBin.Tests.Libraries
{
    [Collection("Sequential")]
    public class StandardFunctionTests(ITestOutputHelper output)
    {
        [Fact]
        public void TestReadToRecord()
        {
            const string source = """
                                  MODULE ReadToRecord;
                                  VAR
                                    s: RECORD a : INTEGER END;

                                  BEGIN
                                    ReadInt(s.a);
                                    s.a := s.a + 1;
                                    WriteInt(s.a);
                                    WriteLn
                                  END ReadToRecord.
                                  """;
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            byte[] assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.NotNull(assembly);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1, new StringReader("12" + Environment.NewLine));
            Assert.Equal("13\n", output1.ToString().NlFix());
        }

        [Fact]
        public void TestThrowOnUnknownFunction()
        {
            const string source = """
                                  MODULE ReadToRecord;
                                  BEGIN
                                    WriteInt(1);
                                  END ReadToRecord.
                                  """;
            var m = TestHelper.CompileString(source, output);

            if (m.CompilerInstance == null)
            {
                throw new NullReferenceException(nameof(m.CompilerInstance));
            }
            var cg = new MsilBinGenerator(module: m);

            // remove WriteInt function from repository
            StandardFunctionRepository.RemoveFunction($"WriteInt/{TypeDefinition.VoidTypeName}/{TypeDefinition.IntegerTypeName}");
            var ex = Assert.Throws<InternalCompilerException>(() => cg.GenerateIntermediateCode());
            Assert.Equal("Cannot find function WriteInt/$$VOID/INTEGER", ex.Message);
        }

        [Fact]
        public void TestStandardFunctionReposRemoveFail()
        {
            const string source = """
                                  MODULE ReadToRecord;
                                  BEGIN
                                    WriteInt(1);
                                  END ReadToRecord.
                                  """;
            var m = TestHelper.CompileString(source, output);

            if (m.CompilerInstance == null)
            {
                throw new NullReferenceException(nameof(m.CompilerInstance));
            }
            StandardFunctionRepository.Initialize(m);

            var ex = Assert.Throws<ArgumentException>(() => StandardFunctionRepository.RemoveFunction($"ThisDoesNotExist"));
            Assert.Equal("Key ThisDoesNotExist does not exist (Parameter 'key')", ex.Message);
            Assert.Equal("key", ex.ParamName);
        }

    }
}
