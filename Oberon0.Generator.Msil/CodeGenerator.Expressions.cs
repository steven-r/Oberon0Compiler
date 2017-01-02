using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Oberon0.Compiler;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Types;

namespace Oberon0.Generator.Msil
{
    public partial class CodeGenerator
    {
        private static readonly Dictionary<int, Func<CodeGenerator, Block, BinaryExpression, Expression>> OperatorMapping = 
            new Dictionary<int, Func<CodeGenerator, Block, BinaryExpression, Expression>>
            {
                {OberonGrammarLexer.PLUS, HandleSimpleOperation},
                {OberonGrammarLexer.MINUS, HandleSimpleOperation},
                {OberonGrammarLexer.MULT, HandleSimpleOperation},
                {OberonGrammarLexer.DIV, HandleSimpleOperation},
                {OberonGrammarLexer.MOD, HandleSimpleOperation},
                {OberonGrammarLexer.EQUAL, HandleRelOperation},
                {OberonGrammarLexer.NOTEQUAL, HandleRelOperation},
                {OberonGrammarLexer.GT, HandleRelOperation},
                {OberonGrammarLexer.GE, HandleRelOperation},
                {OberonGrammarLexer.LT, HandleRelOperation},
                {OberonGrammarLexer.LE, HandleRelOperation},
                {OberonGrammarLexer.NOT, HandleRelOperation},
            };

        private static readonly Dictionary<int, string> SimpleInstructionMapping = 
            new Dictionary<int, string>
            {
                {OberonGrammarLexer.PLUS, "add" },
                {OberonGrammarLexer.DIV, "div" },
                {OberonGrammarLexer.MULT, "mul" },
                {OberonGrammarLexer.MINUS, "sub" },
                {OberonGrammarLexer.MOD, "rem" },
            };

        public string DumpCode()
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb))
            {
                DumpCode(w);
            }
            return sb.ToString();
        }

        // "a <op> b" or "<op> a"
        private static BinaryExpression HandleSimpleOperation(CodeGenerator generator, Block block, BinaryExpression bin)
        {
            if (bin.IsUnary && bin.LeftHandSide.TargetType.BaseType == BaseType.IntType)
            {
                generator.LoadConstantExpression(ConstantIntExpression.Zero, null);
            }
            if (bin.IsUnary && bin.LeftHandSide.TargetType.BaseType == BaseType.DecimalType)
            {
                generator.LoadConstantExpression(ConstantDoubleExpression.Zero, null);
            }
            generator.ExpressionCompiler(block, bin.LeftHandSide);
            if (!bin.IsUnary)
            {
                generator.ExpressionCompiler(block, bin.RightHandSide);
            }
            generator.Code.Emit(SimpleInstructionMapping[bin.Operator]);
            return bin;
        }

        // "a <rel> b" or "<rel> a" (aka ~)
        private static BinaryExpression HandleRelOperation(CodeGenerator generator, Block block, BinaryExpression bin)
        {
            generator.ExpressionCompiler(block, bin.LeftHandSide);
            if (!bin.IsUnary)
                generator.ExpressionCompiler(block, bin.RightHandSide);
            return bin;
        }

        /// <summary>
        /// Compile the expression and return the register number where the result is stored
        /// </summary>
        /// <param name="block">The block.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>The register</returns>
        [NotNull]
        internal Expression ExpressionCompiler([NotNull] Block block, [NotNull] Expression expression)
        {
            var eInfo = new ExpressionGeneratorInfo();
            expression.GeneratorInfo = eInfo;
            var v = expression as VariableReferenceExpression;
            if (v != null)
                return HandleVariableReferenceExpression(block, v);
            var s = expression as StringExpression;
            if (s != null)
            {

                string str = s.Value.Remove(0,1);
                str = str.Remove(str.Length - 1);
                str = str.Replace("''", "'");
                Code.Emit("ldstr", $"\"{str}\"");
                return s;
            }
            var bin = expression as BinaryExpression;
            if (bin != null)
            {
                Code.EmitComment(bin.ToString());
                return OperatorMapping[bin.Operator](this, block, bin);
            }
            var cons = expression as ConstantExpression;
            if (cons != null)
            {
                LoadConstantExpression(cons, null);
                return cons;
            }
            var fc = expression as FunctionCallExpression;
            if (fc != null)
            {
                Code.EmitComment(fc.FunctionDeclaration.ToString());
                int i = 0;
                foreach (ProcedureParameter parameter in fc.FunctionDeclaration.Block.Declarations.OfType<ProcedureParameter>())
                {
                    if (parameter.IsVar)
                    {
                        VariableReferenceExpression reference = (VariableReferenceExpression)fc.Parameters[i];
                        Load(block, reference.Declaration, reference.Selector);
                    }
                    else
                    {
                        ExpressionCompiler(block, fc.Parameters[i]);
                    }
                    i++;
                }
                Code.Call(fc.FunctionDeclaration);
                return fc;
            }
            throw new NotImplementedException();
        }

        private void LoadConstantExpression(ConstantExpression cons, ConstDeclaration declaration)
        {
            if (declaration != null)
                Code.LoadConstRef(declaration);
            else
                Code.PushConst(cons.Value);
        }

        private Expression HandleVariableReferenceExpression(Block block, VariableReferenceExpression v)
        {
            if (v.IsConst)
            {
                LoadConstantExpression(((ConstDeclaration) v.Declaration).Value, (ConstDeclaration) v.Declaration);
            }
            else
            {
                Load(block, v.Declaration, v.Selector);
            }
            return v;
        }

        internal void Load(Block block, Declaration varDeclaration, VariableSelector selector, bool noLoad = false)
        {
            if (varDeclaration.Block.Parent == null && !(varDeclaration is ProcedureParameter))
            {
                Code.Emit("ldsfld", Code.GetTypeName(varDeclaration.Type), $"{Code.ClassName}::{varDeclaration.Name}");
            }
            else
            {
                DeclarationGeneratorInfo dgi = (DeclarationGeneratorInfo)varDeclaration.GeneratorInfo;
                ProcedureParameter pp = varDeclaration as ProcedureParameter;
                if (pp != null)
                {
                    if (pp.IsVar)
                        Code.Emit("ldarga" + Code.DotNumOrArg(dgi.Offset, 0, 3, false));
                    else
                        Code.Emit("ldarg", dgi.Offset.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    Code.Emit("ldloc" + Code.DotNumOrArg(dgi.Offset, 0, 3));
                }
            }
            if (varDeclaration.Type.BaseType == BaseType.ComplexType && selector != null)
            {
                foreach (BaseSelectorElement selectorElement in selector)
                {
                    var ae = selectorElement as IndexSelector;
                    if (ae != null)
                    {
                        var ad = (ArrayTypeDefinition)varDeclaration.Type;
                        ExpressionCompiler(block, ae.IndexDefinition);
                        if (!noLoad)
                            Code.EmitLdelem(ae, ad);
                        continue;
                    }
                    var ie = selectorElement as IdentifierSelector;
                    if (ie != null)
                    {
                        
                    }
                }
            }
        }

        internal void StoreVar(Block block, Declaration assignmentVariable, VariableSelector selector)
        {
            BaseSelectorElement last = selector?.LastOrDefault();

            if (last != null)
            {
                IndexSelector indexSelector = last as IndexSelector;
                if (indexSelector != null)
                    Code.EmitStelem(indexSelector);
                IdentifierSelector identSelector = last as IdentifierSelector;
                if (identSelector != null)
                {
                    Code.EmitStfld(identSelector);
                }
            }
            else
            {
                if (assignmentVariable.Block.Parent == null && !(assignmentVariable is ProcedureParameter))
                {
                    Code.Emit("stsfld", Code.GetTypeName(assignmentVariable.Type), $"{Code.ClassName}::{assignmentVariable.Name}");
                }
                else
                {
                    var pp = assignmentVariable as ProcedureParameter;
                    var dgi = (DeclarationGeneratorInfo) assignmentVariable.GeneratorInfo;
                    // pp != null --> parameter otherwise local var
                    if (pp != null)
                    {
                        Code.Emit("starg", dgi.Offset.ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        Code.Emit("stloc" + Code.DotNumOrArg(dgi.Offset, 0, 3));
                    }
                }
            }
        }

        internal void StartStoreVar(Block block, Declaration assignmentVariable, VariableSelector selector)
        {
            if (selector != null && selector.Any())
                Load(block, assignmentVariable, selector, true);
        }
    }
}
