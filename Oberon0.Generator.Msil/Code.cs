using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Oberon0.Generator.Msil
{
    public class Code : StringWriter
    {
        private int _labelId;

        public Code(StringBuilder sb) : base(sb)
        {

        }

        private static string DumpConstValue(ConstantExpression constantExpression, bool isLoad = false, bool isData = false)
        {
            switch (constantExpression.TargetType)
            {
                case BaseType.IntType:
                    return constantExpression.ToInt32().ToString();
                //case BaseType.StringType:
                //    return constantExpression.ToStringValue();
                case BaseType.DecimalType:
                    return constantExpression.ToDouble().ToString("G");
                case BaseType.BoolType:
                    if (isLoad || isData)
                        return constantExpression.ToBool() ? "1" : "0";
                    return constantExpression.ToBool().ToString().ToLower(CultureInfo.InvariantCulture);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static string GetDataTypeName(TypeDefinition type)
        {
            switch (type.BaseType)
            {
                case BaseType.BoolType:
                case BaseType.IntType:
                    return "int32";
                case BaseType.StringType:
                    return "string";
                case BaseType.DecimalType:
                    return "double";
                case BaseType.VoidType:
                    return "void";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        internal static string DotNumOrArg(int value, int min, int max, bool isSimpleShortForm = true)
        {
            if (value < min || value > max)
            {
                if (value <= 255 && isSimpleShortForm)
                    return $".s {value}";
                return $" {value}";
            }
            if (value < 0)
                return $".m{-value}";
            return $".{value}";
        }

        internal void Emit(string opCode, params object[] parameters)
        {
            EmitNoNl(opCode, parameters);
            WriteLine();
        }

        /*
                internal string GenerateBytes(byte[] data)
                {
                    return string.Join(" ", Array.ConvertAll(data, x => x.ToString("X2")));
                }
        */

        internal void EmitComment(string comment)
        {
            WriteLine("// " + comment);
        }

        internal void EmitNoNl(string opCode, params object[] parameters)
        {
            Write("\t" + opCode);
            foreach (string parameter in parameters)
            {
                Write("\t" + parameter);
            }
        }

        internal void EmitStfld(IdentifierSelector identSelector)
        {
            //TODO: Get type information from identSelector
            Emit("stelem");
        }

        internal static string GetTypeName(BaseType type)
        {
            switch (type)
            {
                case BaseType.IntType:
                    return "int32";
                case BaseType.StringType:
                    return "string";
                case BaseType.DecimalType:
                    return "double";
                case BaseType.BoolType:
                    return "bool";
                case BaseType.VoidType:
                    return "void";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal static string GetTypeName(TypeDefinition type)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (type.BaseType)
            {
                case BaseType.ComplexType:
                    var arrayTypeDefinition = type as ArrayTypeDefinition;
                    if (arrayTypeDefinition != null)
                    {
                        return $"{GetTypeName(arrayTypeDefinition.ArrayType)}[]";
                    }
                    return type.Name;
                default:
                    return GetTypeName(type.BaseType);
            }
        }

        internal void PushConst(object data, string label = null)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), $"{nameof(data)} is null.");
            if (!string.IsNullOrWhiteSpace(label))
                Write(label + ": ");
            switch (Type.GetTypeCode(data.GetType()))
            {
                case TypeCode.Boolean:
                    Emit("ldc.i4." + ((bool)data ? "1" : "0"));
                    break;
                case TypeCode.Char:
                    Emit("ldc.i4.s ", Convert.ToByte((char)data).ToString("x2"));
                    break;
                case TypeCode.Int16:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                    Emit("ldc.i2", data);
                    break;
                case TypeCode.Byte:
                    Emit("ldc.i1.s", data);
                    break;
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    Emit("ldc.i4" + DotNumOrArg((int)data, -1, 8));
                    break;
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    Emit("ldc.i8 ", data);
                    break;
                case TypeCode.Single:
                    Emit("ldc.r4", data);
                    break;
                case TypeCode.Double:
                    Emit("ldc.r8", data);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Branch(string brType, string label)
        {
            Emit(brType, label);
        }

        public void Call(FunctionDeclaration func)
        {
            if (func.IsInternal)
            {
                if (func.Name == "eot")
                    Emit("call", "bool", "isEof()");
                else
                    throw new NotImplementedException();
            }
            else
            {
                EmitNoNl("call", GetTypeName(func.ReturnType), func.Name, "(");
                List<string> typeNames = new List<string>(func.Parameters.Count);
                typeNames.AddRange(func.Parameters.Select(parameter => parameter.Name));
                Write(string.Join(", ", typeNames));
                WriteLine(")");
            }
        }


        public void ConstField(ConstDeclaration constDeclaration)
        {
            WriteLine($".data {constDeclaration.Name} = {GetDataTypeName(constDeclaration.Type)} ({DumpConstValue(constDeclaration.Value, false, true)})");
        }


        public void DataField(Declaration declaration, bool isStatic)
        {
            WriteLine($".field {(isStatic ? "static " : string.Empty)}{GetTypeName(declaration.Type)} {declaration.Name}");
        }


        public string EmitLabel()
        {
            return EmitLabel(null);
        }

        public string EmitLabel(string label)
        {
            label = label ?? GetLabel();
            Write(label + ": ");
            return label;
        }

        /// <summary>
        /// Emits the ldelem.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="arrayTypeDefinition">The array type definition.</param>
        public void EmitLdelem(IndexSelector index, ArrayTypeDefinition arrayTypeDefinition)
        {
            Write("\tldelem");
            switch (arrayTypeDefinition.BaseType)
            {
                case BaseType.IntType:
                case BaseType.BoolType:
                    WriteLine(".i4");
                    break;
                case BaseType.DecimalType:
                    WriteLine(".r8");
                    break;
                default:
                    WriteLine(" " + GetTypeName(arrayTypeDefinition.ArrayType));
                    break;
            }
        }

        public void EmitStelem(IndexSelector indexSelector)
        {
            Write("\tstelem");
            switch (indexSelector.IndexDefinition.TargetType)
            {
                case BaseType.IntType:
                case BaseType.BoolType:
                    WriteLine(".i4");
                    break;
                case BaseType.DecimalType:
                    WriteLine(".r8");
                    break;
                default:
                    WriteLine(".ref\t" + GetTypeName(indexSelector.IndexDefinition.TargetType));
                    break;
            }
        }

        public void EndMethod()
        {
            Emit("ret");
            WriteLine("}");
        }

        public string GetLabel()
        {
            return $"L{_labelId++}";
        }

        public void LoadConstRef(ConstDeclaration constDeclaration)
        {
            Emit("ldvar", constDeclaration.Name);
        }

        public void LocalVarDef(Declaration declaration, bool isPointer)
        {
            Write($"{(isPointer ? "&" : string.Empty)}{GetTypeName(declaration.Type)} {declaration.Name}");
        }

        public void Reference(string assemblyName)
        {
            WriteLine($".assembly extern {assemblyName} {{ }}");
        }

        public void StartAssembly(string moduleName)
        {
            WriteLine($".assembly {moduleName} {{ }}");
        }

        public void StartMainMethod()
        {
            Write(@"
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
");
        }

        public void StartMethod(FunctionDeclaration functionDeclaration)
        {
            Write($".method private static {GetTypeName(functionDeclaration.ReturnType)} {functionDeclaration.Name}(");
            List<string> paramList = new List<string>(functionDeclaration.Parameters.Count);
            int id = 0;
            foreach (ProcedureParameter parameter in functionDeclaration.Parameters)
            {
                parameter.GeneratorInfo = new DeclarationGeneratorInfo(id++);
                paramList.Add($"{GetTypeName(parameter.Type)} {parameter.Name}");
            }
            WriteLine(string.Join(", ", paramList) + @")
{");
        }

        public void StartModule(string moduleName)
        {
            WriteLine($".module {moduleName}.exe");
        }

        public void StartClass(string className)
        {
            WriteLine($".class public {className} {{");
        }

        public void EndClass()
        {
            WriteLine("}");
        }
    }
}
