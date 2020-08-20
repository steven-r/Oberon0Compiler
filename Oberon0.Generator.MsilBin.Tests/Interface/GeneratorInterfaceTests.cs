#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Diagnostics;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Oberon0.Generator.MsilBin.Tests.Interface
{
    public class GeneratorInterfaceTests
    {
        public GeneratorInterfaceTests(ITestOutputHelper output)
        {
            this._output = output;
        }

        private readonly ITestOutputHelper _output;

        [Fact]
        public void TestWriterAgainstStringWriter()
        {
            const string source = @"MODULE ReadToRecord;
VAR
  s: RECORD a : INTEGER END;

BEGIN
  ReadInt(s.a);
  s.a := s.a + 1;
  WriteInt(s.a);
  WriteLn
END ReadToRecord.";
            var cg = CompileHelper.CompileOberon0Code(source, out var code, _output);
            using var sw = new StringWriter();
            cg.WriteIntermediateCode(sw);
            Assert.Equal(code, sw.ToString());
        }
    }
}