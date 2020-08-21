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

namespace Oberon0.Generator.MsilBin.Tests.Complex
{
    public class FullExampleTests
    {
        private readonly ITestOutputHelper _output;

        public FullExampleTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void FullExample1()
        {
            const string source = @"MODULE Samples;
PROCEDURE Multiply(x: INTEGER; y: INTEGER);
VAR 
  z: INTEGER;
BEGIN 
    WriteString('Mul: ');
    WriteInt(x); 
    WriteString('*');
    WriteInt(y); 
    WriteString('= ');
    z := 0;
    WHILE x > 0 DO
        IF x MOD 2 = 1 THEN 
            z := z + y 
        END ;
        y := 2*y; 
        x := x DIV 2
    END ;
    WriteInt(z); 
    WriteLn
END Multiply;

PROCEDURE Divide(x: INTEGER; y: INTEGER);
    VAR r, q, w: INTEGER;

BEGIN 
    WriteString('Div: ');
    WriteInt(x); 
    WriteString('/');
    WriteInt(y); 
    WriteString('= ');
    r := x; 
    q := 0; 
    w := y;
    WHILE w <= r DO 
        w := 2*w 
    END ;
    WHILE w > y DO
        q := 2*q; 
        w := w DIV 2;
        IF w <= r THEN 
            r := r - w; 
            q := q + 1 
        END
    END ;
    WriteInt(q); 
    WriteString(', rem ');
    WriteInt(r); 
    WriteLn
END Divide;

BEGIN 
    Multiply(5, 10);
    Multiply(-5, 10);
    Multiply(5, 0);
    Divide(100, 2);
    Divide(100, 3);
    Divide(3, 100)
END Samples.";
            string expected = @"Mul: 5*10= 50
Mul: -5*10= 0
Mul: 5*0= 0
Div: 100/2= 50, rem 0
Div: 100/3= 33, rem 1
Div: 3/100= 0, rem 3
".NlFix();
            var cg = CompileHelper.CompileOberon0Code(source, out string code, _output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.True(assembly != null);

            using var output = new StringWriter();
            Runner.Execute(assembly, output);
            Assert.Equal(expected, output.ToString().NlFix());
        }
    }
}
