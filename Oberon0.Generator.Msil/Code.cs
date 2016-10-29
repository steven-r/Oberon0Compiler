using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Generator.Msil
{
    public class Code: StringWriter
    {
        private static int _labelId;

        public string ModuleName { get; set; }

        public Code(StringBuilder sb): base(sb)
        {
            
        }

        internal void PushConst(object data, string label = null)
        {
            if (!string.IsNullOrWhiteSpace(label))
                Write(label + ": ");
            else 
                Write('\t');
            switch (Type.GetTypeCode(data.GetType()))
            {
                case TypeCode.Boolean:
                    WriteLine("ldc.i4." + (((bool)data)? "1": "0"));
                    break;
                case TypeCode.Char:
                    WriteLine("ldc.i4.s " + Convert.ToByte((char)data).ToString("x2"));
                    break;
                case TypeCode.Int16:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                    WriteLine("ldc.i2 " + data);
                    break;
                case TypeCode.Byte:
                    WriteLine("ldc.i1.s " + data);
                    break;
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    WriteLine("ldc.i4" + DotNumOrArg((int)data, -1, 8));
                    break;
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    WriteLine("ldc.i8 " + data);
                    break;
                case TypeCode.Single:
                    WriteLine("ldc.r4 " + data);
                    break;
                case TypeCode.Double:
                    WriteLine("ldc.r8 " + data);
                    break;
                case TypeCode.String:
                    WriteLine("ldstr " + generateString((string)data));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }




        internal string generateString(string str)
        {
            StringBuilder sb = new StringBuilder(str.Length*2);
            sb.Append('"');
            foreach (char c in str)
            {
                if (char.IsControl(c) || c == '"')
                {
                    switch (c)
                    {
                        case '\r':
                            sb.Append("\\r");
                            break;
                        case '\t':
                            sb.Append("\\t");
                            break;
                        case '\n':
                            sb.Append("\\n");
                            break;
                        case '"':
                            sb.Append("\\");
                            break;
                        default:
                            sb.Append('\\' + Convert.ToByte(c).ToString("x2"));
                            break;
                    }
                }
                else
                {
                    sb.Append(c);
                }
            }
            sb.Append('"');
            return sb.ToString();
        }

        internal string GenerateBytes(byte[] data)
        {
            return string.Join(" ", Array.ConvertAll(data, x => x.ToString("X2")));
        }

        internal void EmitComment(string comment)
        {
            WriteLine("// " + comment);
        }

        public void Reference(string assemblyName, bool auto)
        {
            WriteLine($".assembly extern {assemblyName} {{ }}");
        }

        public void StartAssembly(string moduleName)
        {
            ModuleName = moduleName;
            WriteLine($".assembly {moduleName} {{ }}");
        }


        public void ConstField(ConstDeclaration constDeclaration)
        {
            WriteLine($".data {constDeclaration.Name} = {GetDataTypeName(constDeclaration.Type)} ({DumpConstValue(constDeclaration.Value, false, true)})");
        }

        private string DumpConstValue(ConstantExpression constantExpression, bool isLoad = false, bool isData = false)
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
                    return constantExpression.ToBool().ToString().ToLower();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string GetTypeName(TypeDefinition type)
        {
            switch (type.BaseType)
            {
                case BaseType.IntType:
                    return "int32";
                case BaseType.StringType:
                    return "string";
                case BaseType.DecimalType:
                    return "double";
                case BaseType.ComplexType:
                    var arrayTypeDefinition = type as ArrayTypeDefinition;
                    if (arrayTypeDefinition != null)
                    {
                        return $"{GetTypeName(arrayTypeDefinition.ArrayType)} [{arrayTypeDefinition.Size}]";
                    }
                    return type.Name;
                case BaseType.BoolType:
                    return "Bool";
                case BaseType.VoidType:
                    return "void";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string GetDataTypeName(TypeDefinition type)
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


        public void DataField(Declaration declaration, bool isStatic = false)
        {
            WriteLine($".field {(isStatic ? "static " : string.Empty)}{GetTypeName(declaration.Type)} {declaration.Name}");
        }


        public string EmitLabel(string label = null)
        {
            label = label ?? GetLabel();
            Write(label + ": ");
            return label;
        }

        public string GetLabel()
        {
            return $"L{_labelId++}";
        }

        public void LoadConstRef(ConstDeclaration constDeclaration)
        {
            WriteLine("\tldvar " + constDeclaration.Name);
        }

        public void StartModule(string moduleName)
        {
            WriteLine($".module {moduleName}.exe");
        }

        public void Load(VariableReferenceExpression variable)
        {
            if (variable.Declaration.Block.Parent == null)
            {
                // global
                WriteLine($"\tldsfld {GetTypeName(variable.Declaration.Type)} " + variable.Name);
            }
            else
            {
                DeclarationGeneratorInfo dgi = (DeclarationGeneratorInfo)variable.Declaration.GeneratorInfo;
                ProcedureParameter pp = variable.Declaration as ProcedureParameter;
                if (pp != null)
                {
                    if (pp.IsVar)
                        WriteLine("\tldarg" + DotNumOrArg(dgi.Offset, 0, 3, false));
                    else
                        WriteLine("\tldarga " + dgi.Offset);
                }
                else
                {
                    WriteLine("\tldloc" + DotNumOrArg(dgi.Offset, 0, 3));
                }
            }
        }

        private string DotNumOrArg(int value, int min, int max, bool isSimpleShortForm = true)
        {
            if (value < min || value > max)
            {
                if (value <= 255 && isSimpleShortForm)
                {
                    return $".s {value}";
                }
                return $" {value}";
            }
            if (value < 0)
            {
                return $".m{-value}";
            }
            return $".{value}";
        }

        public void LocalVarDef(Declaration declaration, bool isPointer)
        {
            Write($"{(isPointer ? "&" : string.Empty)}{GetTypeName(declaration.Type)} {declaration.Name}");
        }

        public void Branch(string brType, string label)
        {
            WriteLine($"\t{brType} {label}");
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
            WriteLine(string.Join(", ", paramList) + ")\n{");
        }

        public void EndMethod(FunctionDeclaration functionDeclaration)
        {
            WriteLine("\tret");
            WriteLine("}");
        }

        public void StoreVar(Declaration assignmentVariable)
        {
            if (assignmentVariable.Block.Parent == null)
            {
                WriteLine($"\tstsfld {GetTypeName(assignmentVariable.Type)} {assignmentVariable.Name}");
            }
            else
            {
                var pp = assignmentVariable as ProcedureParameter;
                var dgi = (DeclarationGeneratorInfo)assignmentVariable.GeneratorInfo;
                if (pp != null)
                {
                    WriteLine("\tstarg" + DotNumOrArg(dgi.Offset, 0, 3));
                }
                else
                {
                    WriteLine("\tstloc" + DotNumOrArg(dgi.Offset, 0, 3));
                }
            }
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

        public void Call(FunctionDeclaration func)
        {
            if (func.IsInternal)
            {
                if (func.Name == "eot")
                {
                    WriteLine("\tcall bool isEof()");
                }
                else throw new NotImplementedException();
            }
            else
            {
                Write($"\tcall {GetTypeName(func.ReturnType)} {func.Name} (");
                List<string> typeNames = new List<string>(func.Parameters.Count);
                typeNames.AddRange(func.Parameters.Select(parameter => parameter.Name));
                WriteLine($"{string.Join(", ", typeNames)})");
            }
        }

        public void Pop()
        {
            Write("\tpop");
        }
    }
}
