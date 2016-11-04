using System.IO;
using System.Text;
using NUnit.Framework;
using Oberon0.Compiler;
using Oberon0.Compiler.Definitions;
using Oberon0.Generator.Msil.Tests.Arrays;

namespace Oberon0.Generator.Msil.Tests.Complex
{

    [TestFixture]
    public class FullExample
    {
        [Test]
        public void FullExample1()
        {
            string expected = @".assembly extern mscorlib { }
.assembly Samples { }
// Code compiled for module Samples
.module Samples.exe
.data TRUE = int32 (1)
.data FALSE = int32 (0)
.field static int32 n
.method private static void Multiply()
{
	.locals (int32 x, int32 y, int32 z)
// Call ReadInt
	call string [mscorlib]System.Console::ReadLine()
	call int32 [mscorlib]System.Int32::Parse(string)
	stloc.0
// Call ReadInt
	call string [mscorlib]System.Console::ReadLine()
	call int32 [mscorlib]System.Int32::Parse(string)
	stloc.1
	ldc.i4.0
	stloc.2
// WHILE
L1: // GT (IntType, IntType) -> BoolType
	ldloc.0
	ldc.i4.0
	ble.un	L0
// IF
// Equals (IntType, IntType) -> BoolType
// Mod (IntType, IntType) -> IntType
	ldloc.0
	ldc.i4.2
	rem
	ldc.i4.1
	bne.un	L3
// Add (IntType, IntType) -> IntType
	ldloc.2
	ldloc.1
	add
	stloc.2
	br	L2
L3: L2: // Mul (IntType, IntType) -> IntType
	ldc.i4.2
	ldloc.1
	mul
	stloc.1
// Div (IntType, IntType) -> IntType
	ldloc.0
	ldc.i4.2
	div
	stloc.0
	br	L1
L0: // Call WriteInt
	ldstr ""{0}""
	ldloc.0
	box int32
	call void [mscorlib]System.Console::Write(string, object)
// Call WriteInt
	ldstr ""{0}""
	ldloc.1
	box int32
	call void [mscorlib]System.Console::Write(string, object)
// Call WriteInt
	ldstr ""{0}""
	ldloc.2
	box int32
	call void [mscorlib]System.Console::Write(string, object)
// Call WriteLn
	call void [mscorlib]System.Console::WriteLine()
	ret
}
.method private static void Divide()
{
	.locals (int32 x, int32 y, int32 r, int32 q, int32 w)
// Call ReadInt
	call string [mscorlib]System.Console::ReadLine()
	call int32 [mscorlib]System.Int32::Parse(string)
	stloc.0
// Call ReadInt
	call string [mscorlib]System.Console::ReadLine()
	call int32 [mscorlib]System.Int32::Parse(string)
	stloc.1
	ldloc.0
	stloc.2
	ldc.i4.0
	stloc.3
	ldloc.1
	stloc.s 4
// WHILE
L5: // LE (IntType, IntType) -> BoolType
	ldloc.s 4
	ldloc.2
	bgt.un	L4
// Mul (IntType, IntType) -> IntType
	ldc.i4.2
	ldloc.s 4
	mul
	stloc.s 4
	br	L5
L4: // WHILE
L7: // GT (IntType, IntType) -> BoolType
	ldloc.s 4
	ldloc.1
	ble.un	L6
// Mul (IntType, IntType) -> IntType
	ldc.i4.2
	ldloc.3
	mul
	stloc.3
// Div (IntType, IntType) -> IntType
	ldloc.s 4
	ldc.i4.2
	div
	stloc.s 4
// IF
// LE (IntType, IntType) -> BoolType
	ldloc.s 4
	ldloc.2
	bgt.un	L9
// Sub (IntType, IntType) -> IntType
	ldloc.2
	ldloc.s 4
	sub
	stloc.2
// Add (IntType, IntType) -> IntType
	ldloc.3
	ldc.i4.1
	add
	stloc.3
	br	L8
L9: L8: 	br	L7
L6: // Call WriteInt
	ldstr ""{0}""
	ldloc.0
	box int32
	call void [mscorlib]System.Console::Write(string, object)
// Call WriteInt
	ldstr ""{0}""
	ldloc.1
	box int32
	call void [mscorlib]System.Console::Write(string, object)
// Call WriteInt
	ldstr ""{0}""
	ldloc.3
	box int32
	call void [mscorlib]System.Console::Write(string, object)
// Call WriteInt
	ldstr ""{0}""
	ldloc.2
	box int32
	call void [mscorlib]System.Console::Write(string, object)
// Call WriteLn
	call void [mscorlib]System.Console::WriteLine()
	ret
}
.method private static void BinSearch()
{
	.locals (int32 i, int32 j, int32 k, int32 n, int32 x, int32[] a)
	ldc.i4.s 32
	newarr	int32
	stloc.s 5
// Call WriteString
	ldstr ""'enter x: '""
	call void [mscorlib]System.Console::Write(string)
// Call ReadInt
	call string [mscorlib]System.Console::ReadLine()
	call int32 [mscorlib]System.Int32::Parse(string)
	stloc.s 4
	ldc.i4.0
	stloc.2
// WHILE
L11: // Not (BoolType) -> BoolType
// internal BOOLEAN eot()
	call	bool	isEof()
	brfalse	L10
// Call ReadInt
	ldloc.s 5
	ldloc.2
	call string [mscorlib]System.Console::ReadLine()
	call int32 [mscorlib]System.Int32::Parse(string)
	stelem.i4
// Add (IntType, IntType) -> IntType
	ldloc.2
	ldc.i4.1
	add
	stloc.2
	br	L11
L10: 	ldc.i4.0
	stloc.0
	ldloc.3
	stloc.1
// WHILE
L13: // LT (IntType, IntType) -> BoolType
	ldloc.0
	ldloc.1
	bge.un	L12
// Div (IntType, IntType) -> IntType
// Add (IntType, IntType) -> IntType
	ldloc.0
	ldloc.1
	add
	ldc.i4.2
	div
	stloc.2
// IF
// LT (IntType, IntType) -> BoolType
	ldloc.s 4
	ldloc.s 5
	ldloc.2
	ldelem int32
	bge.un	L15
	ldloc.2
	stloc.1
	br	L14
L15: // Add (IntType, IntType) -> IntType
	ldloc.2
	ldc.i4.1
	add
	stloc.0
L14: 	br	L13
L12: // Call WriteInt
	ldstr ""{0}""
	ldloc.0
	box int32
	call void [mscorlib]System.Console::Write(string, object)
// Call WriteInt
	ldstr ""{0}""
	ldloc.1
	box int32
	call void [mscorlib]System.Console::Write(string, object)
// Call WriteInt
	ldstr ""{0}""
	ldloc.s 5
	ldloc.1
	ldelem int32
	box int32
	call void [mscorlib]System.Console::Write(string, object)
// Call WriteLn
	call void [mscorlib]System.Console::WriteLine()
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
// Call ReadInt
	call string [mscorlib]System.Console::ReadLine()
	call int32 [mscorlib]System.Int32::Parse(string)
	stsfld int32 n
// IF
// Equals (IntType, IntType) -> BoolType
	ldsfld int32 n
	ldc.i4.0
	bne.un	L17
// Call Multiply
	call	void	Multiply	()
	br	L16
L17: // Equals (IntType, IntType) -> BoolType
	ldsfld int32 n
	ldc.i4.1
	bne.un	L18
// Call Divide
	call	void	Divide	()
	br	L16
L18: // Call BinSearch
	call	void	BinSearch	()
L16: 	ret
}
".NlFix();

            string proc = @"
MODULE Samples;
VAR 
  n: INTEGER;

PROCEDURE Multiply;
VAR 
  x, y, z: INTEGER;
BEGIN 
    ReadInt(x); 
    ReadInt(y); 
    z := 0;
    WHILE x > 0 DO
        IF x MOD 2 = 1 THEN 
            z := z + y 
        END ;
        y := 2*y; 
        x := x DIV 2
    END ;
    WriteInt(x); 
    WriteInt(y); 
    WriteInt(z); 
    WriteLn
END Multiply;

PROCEDURE Divide;
    VAR x, y, r, q, w: INTEGER;

BEGIN 
    ReadInt(x); 
    ReadInt(y); 
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
    WriteInt(x); 
    WriteInt(y); 
    WriteInt(q); 
    WriteInt(r); 
    WriteLn
END Divide;

PROCEDURE BinSearch;
VAR 
    i, j, k, n, x: INTEGER;
    a: ARRAY 32 OF INTEGER;
BEGIN 
    WriteString('enter x: ');
    ReadInt(x); k := 0;
    WHILE ~eot() DO 
        ReadInt(a[k]); 
        k := k + 1 
    END ;
    i := 0; 
    j := n;
    WHILE i < j DO
        k := (i+j) DIV 2;
        IF x < a[k] THEN 
            j := k 
        ELSE 
            i := k+1 
        END
    END ;
    WriteInt(i); 
    WriteInt(j); 
    WriteInt(a[j]); 
    WriteLn
END BinSearch;

BEGIN 
    ReadInt(n);
    IF n = 0 THEN 
        Multiply 
    ELSIF n = 1 THEN 
        Divide 
    ELSE 
        BinSearch 
    END
END Samples.
".NlFix();
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(proc);
            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
#if DUMPCODE
            using (var f = File.CreateText(Path.GetTempFileName()))
            {
                cg.DumpCode(f);
                f.Flush();
            }
#endif
            StringBuilder sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb))
            {
                cg.DumpCode(w);
            }
            Assert.AreEqual(expected, sb.ToString().NlFix());
        }
    }
}

