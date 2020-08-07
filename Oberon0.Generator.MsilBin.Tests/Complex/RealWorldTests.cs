#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.IO;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;
using Xunit.Abstractions;

namespace Oberon0.Generator.MsilBin.Tests.Complex
{
    public class RealWorldTests
    {
        private readonly ITestOutputHelper _output;

        public RealWorldTests(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Theory]
        [InlineData(20, 2, 10, 0, false)]
        [InlineData(20, 3, 6, 2, false)]
        [InlineData(15, -5, -3, 0, false)]
        [InlineData(15, 0, -3, 0, true)]
        public void Divide(int a, int b, int res, int rem, bool div0)
        {
            const string source = @"
MODULE DivisionTest;
VAR
  x, y, q, r: INTEGER;

PROCEDURE Divide(x: INTEGER; y: INTEGER; VAR q: INTEGER; VAR r: INTEGER);
VAR 
    w: INTEGER;
    negatex: BOOLEAN;
    negatey: BOOLEAN;

BEGIN 
    negatex := x < 0;
    negatey := y < 0;
    IF (negatex) THEN x := -x END;
    IF (negatey) THEN y := -y END;
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
    END;
    IF (negatex & ~negatey) OR (~negatex & negatey) THEN
        q := -q;
        r := -r
    END
END Divide;

BEGIN 
 ReadInt(x);
 ReadInt(y);
 IF (y # 0) THEN 
     Divide(x, y, q, r);
     WriteInt(q);
     WriteString('/');
     WriteInt(r);
 ELSE
   WriteString('DIV0');
 END;
 WriteLn
END DivisionTest.
";
            var cg = CompileHelper.CompileOberon0Code(source, out var code, _output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.True(assembly != null);

            using var output = new StringWriter();
            Runner.Execute(assembly, output, new StringReader($"{a}{Environment.NewLine}{b}{Environment.NewLine}"));
            if (div0)
                Assert.Equal("DIV0\n", output.ToString().NlFix());
            else
                Assert.Equal($"{res}/{rem}\n", output.ToString().NlFix());
        }

        [Theory]
        [InlineData(5, 3)]
        [InlineData(5, 0)]
        [InlineData(20, 1)]
        [InlineData(1, 20)]
        [InlineData(-10, -20)]
        [InlineData(15, -20)]
        [InlineData(-15, 20)]
        public void Multiply(int a, int b)
        {
            string source = @"
MODULE MultiplyTest;
VAR
  x, y, z: INTEGER;

    PROCEDURE Multiply(x: INTEGER; y: INTEGER; VAR z: INTEGER);
    VAR
      negate : BOOLEAN;
    BEGIN 
        negate := x < 0;
        IF (negate) THEN x := -x END;
        z := 0;
        WHILE x > 0 DO
            IF x MOD 2 = 1 THEN 
                z := z + y 
            END;
            y := 2*y; 
            x := x DIV 2
        END ;
        IF (negate) THEN z := -z END;
    END Multiply;

BEGIN 
 ReadInt(x);
 ReadInt(y);
 Multiply(x, y, z);
 WriteInt(z);
 WriteLn
END MultiplyTest.
";
            var cg = CompileHelper.CompileOberon0Code(source, out var code, _output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.True(assembly != null);

            using var output = new StringWriter();
            Runner.Execute(assembly, output, new StringReader($"{a}{Environment.NewLine}{b}{Environment.NewLine}"));
            Assert.Equal($"{a * b}\n", output.ToString().NlFix());
        }
    }
}