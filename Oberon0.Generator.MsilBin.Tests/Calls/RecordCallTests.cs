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
    public class RecordCallTests
    {
        private readonly ITestOutputHelper _output;

        public RecordCallTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void TestRecordCallByReference()
        {
            string source = @"MODULE Test; 
TYPE
   rType = RECORD
     a: INTEGER;
     b: REAL;
     c: BOOLEAN
   END;

VAR 
    r: rType;

    PROCEDURE TestRecord(VAR r: rType);
    BEGIN
        IF (r.a # 1) THEN WriteString(' a error') END;
        IF ~r.c THEN WriteString('c error') END;
        IF ABS(r.b - 1.2345) > EPSILON THEN WriteString('b error') END;
        r.a := -1;
        r.b := -1.2345;
        r.c := FALSE;
    END TestRecord;

BEGIN
    r.a := 1;
    r.b := 1.2345;
    r.c := TRUE;
    TestRecord(r);
    WriteBool((r.a = -1) & (ABS(r.b - 1.2345) > EPSILON) & ~r.c);
    WriteLn 
END Test.";
            var cg = CompileHelper.CompileOberon0Code(source, out string code, _output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.True(assembly != null);

            using var output = new StringWriter();
            Runner.Execute(assembly, output);
            Assert.Equal($"{true}\n", output.ToString().NlFix());
        }


        [Fact]
        public void TestRecordCallByValue()
        {
            string source = @"MODULE Test;
TYPE
   rType = RECORD
     a: INTEGER;
     b: REAL;
     c: BOOLEAN
   END;

VAR 
    r: rType;

    PROCEDURE TestRecord(r: rType);
    BEGIN
        IF (r.a # 1) THEN WriteString(' a error') END;
        IF ~r.c THEN WriteString('c error') END;
        IF ABS(r.b - 1.2345) > EPSILON THEN WriteString('b error') END;
        r.a := -1;
        r.b := -1.2345;
        r.c := FALSE;
    END TestRecord;

BEGIN
    r.a := 1;
    r.b := 1.2345;
    r.c := TRUE;
    TestRecord(r);
    WriteBool((r.a = 1) & (ABS(r.b - 1.2345) < EPSILON) & r.c);
    WriteLn 
END Test.";
            var cg = CompileHelper.CompileOberon0Code(source, out string code, _output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.True(assembly != null);

            using var output = new StringWriter();
            Runner.Execute(assembly, output);
            Assert.Equal($"{true}\n", output.ToString().NlFix());
        }
    }
}
