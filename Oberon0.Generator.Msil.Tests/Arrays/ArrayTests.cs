using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using Oberon0.Compiler;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Generator.Msil.Tests.Arrays
{
    [TestFixture]
    public class ArrayTests
    {
        [Test]
        public void SimpleArrayDefinition()
        {
            string expected = @".assembly extern mscorlib { }
.assembly Array { }
// Code compiled for module Array
.module Array.exe
.data TRUE = int32 (1)
.data FALSE = int32 (0)
.field static int32[] a
.field static bool[] b

.method private hidebysig static bool  isEof() cil managed
{
  .maxstack  2
  .locals init ([0] bool V_0)
  IL_0000:  nop
  IL_0001:  call       class [mscorlib]System.IO.TextReader [mscorlib]System.Console::get_In()
  IL_0006:  callvirt   instance int32 [mscorlib]System.IO.TextReader::Peek()
  IL_000b:  ldc.i4.0
  IL_000c:  clt
  IL_000e:  stloc.0
  IL_000f:  br.s       IL_0011
  IL_0011:  ldloc.0
  IL_0012:  ret
} // end of method Program::isEof

.method static public void $O0$main() cil managed
{   .entrypoint 
	ldc.i4.s 32
	newarr	int32
	stsfld int32[] a
	ldc.i4.s 32
	newarr	bool
	stsfld bool[] b
	ret
}
".NlFix();

            string source = @"MODULE Array;
VAR 
  a: ARRAY 32 OF INTEGER;
  b: ARRAY 32 OF BOOLEAN;

END Array.";
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();

            StringBuilder sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb))
            {
                cg.DumpCode(w);
            }
            string outputData;
            Assert.IsTrue(TestHelper.CompileRunTest(sb.ToString(), null, out outputData));
            Assert.IsTrue(string.IsNullOrEmpty(outputData));
            Assert.AreEqual(expected, sb.ToString().NlFix());
        }


        [Test]
        public void SimpleArrayDefinitionLocal()
        {
            string expected = @".assembly extern mscorlib { }
.assembly Array { }
// Code compiled for module Array
.module Array.exe
.data TRUE = int32 (1)
.data FALSE = int32 (0)
.method private static void Test()
{
	.locals (int32[] a, bool[] b)
	ldc.i4.s 32
	newarr	int32
	stloc.0
	ldc.i4.s 32
	newarr	bool
	stloc.1
	ret
}

.method private hidebysig static bool  isEof() cil managed
{
  .maxstack  2
  .locals init ([0] bool V_0)
  IL_0000:  nop
  IL_0001:  call       class [mscorlib]System.IO.TextReader [mscorlib]System.Console::get_In()
  IL_0006:  callvirt   instance int32 [mscorlib]System.IO.TextReader::Peek()
  IL_000b:  ldc.i4.0
  IL_000c:  clt
  IL_000e:  stloc.0
  IL_000f:  br.s       IL_0011
  IL_0011:  ldloc.0
  IL_0012:  ret
} // end of method Program::isEof

.method static public void $O0$main() cil managed
{   .entrypoint 
	ret
}
".NlFix();

            string source = @"MODULE Array;

PROCEDURE Test;
VAR 
  a: ARRAY 32 OF INTEGER;
  b: ARRAY 32 OF BOOLEAN;

END Test;

END Array.";
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            StringBuilder sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb))
            {
                cg.DumpCode(w);
            }
            string outputData;
            Assert.IsTrue(TestHelper.CompileRunTest(sb.ToString(), null, out outputData));
            Assert.IsTrue(string.IsNullOrEmpty(outputData));
            Assert.AreEqual(expected, sb.ToString().NlFix());
        }
        [Test]
        public void SimpleArraySetAndGetValueGlobal()
        {
            string expected = @".assembly extern mscorlib { }
.assembly Array { }
// Code compiled for module Array
.module Array.exe
.data TRUE = int32 (1)
.data FALSE = int32 (0)
.field static int32[] a
.field static int32 n

.method private hidebysig static bool  isEof() cil managed
{
  .maxstack  2
  .locals init ([0] bool V_0)
  IL_0000:  nop
  IL_0001:  call       class [mscorlib]System.IO.TextReader [mscorlib]System.Console::get_In()
  IL_0006:  callvirt   instance int32 [mscorlib]System.IO.TextReader::Peek()
  IL_000b:  ldc.i4.0
  IL_000c:  clt
  IL_000e:  stloc.0
  IL_000f:  br.s       IL_0011
  IL_0011:  ldloc.0
  IL_0012:  ret
} // end of method Program::isEof

.method static public void $O0$main() cil managed
{   .entrypoint 
	ldc.i4.s 32
	newarr	int32
	stsfld int32[] a
	ldsfld int32[] a
	ldc.i4.0
	ldc.i4.1
	stelem.i4
	ldsfld int32[] a
	ldc.i4.0
	ldelem int32
	stsfld int32 n
// Call WriteInt
	ldstr ""{0}""
	ldsfld int32 n
	box int32
	call void [mscorlib]System.Console::Write(string, object)
	ldsfld int32[] a
	ldc.i4.1
	ldsfld int32 n
	stelem.i4
// Call WriteLn
	call void [mscorlib]System.Console::WriteLine()
	ret
}
".NlFix();

            string source = @"MODULE Array;
VAR 
  a: ARRAY 32 OF INTEGER;
  n: INTEGER;

BEGIN
  a[0] := 1;
  n := a[0];
  WriteInt(n);
  a[1] := n;
  WriteLn
END Array.";
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();

            //cg.DumpCode(Console.Out);
            StringBuilder sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb))
            {
                cg.DumpCode(w);
            }
            string outputData;
            Assert.IsTrue(TestHelper.CompileRunTest(sb.ToString(), null, out outputData));
            Assert.AreEqual("1\n", outputData.NlFix());
            Assert.AreEqual(expected, sb.ToString().NlFix());
        }

    }
}