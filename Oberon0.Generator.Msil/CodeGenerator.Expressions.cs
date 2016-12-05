using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Oberon0.Compiler;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Generator.Msil
{
    public partial class CodeGenerator
    {
        private static readonly Dictionary<TokenType, Func<CodeGenerator, Block, BinaryExpression, BinaryExpression>> OperatorMapping = 
            new Dictionary<TokenType, Func<CodeGenerator, Block, BinaryExpression, BinaryExpression>>
            {
                {TokenType.Add, HandleSimpleOperation},
                {TokenType.Sub, HandleSimpleOperation},
                {TokenType.Mul, HandleSimpleOperation},
                {TokenType.Div, HandleSimpleOperation},
                {TokenType.Mod, HandleSimpleOperation},
                {TokenType.Unary, HandleSimpleOperation},
                {TokenType.Equals, HandleRelOperation},
                {TokenType.NotEquals, HandleRelOperation},
                {TokenType.GT, HandleRelOperation},
                {TokenType.GE, HandleRelOperation},
                {TokenType.LT, HandleRelOperation},
                {TokenType.LE, HandleRelOperation},
                {TokenType.Not, HandleRelOperation},
            };

        private static readonly Dictionary<TokenType, string> SimpleInstructionMapping = 
            new Dictionary<TokenType, string>
            {
                {TokenType.Add, "add" },
                {TokenType.Div, "div" },
                {TokenType.Mul, "mul" },
                {TokenType.Sub, "sub" },
                {TokenType.Mod, "rem" },
            };

        // "a <op> b" or "<op> a"
        private static BinaryExpression HandleSimpleOperation(CodeGenerator generator, Block block, BinaryExpression bin)
        {
            generator.ExpressionCompiler(block, bin.LeftHandSide);
            if (!bin.IsUnary)
                generator.ExpressionCompiler(block, bin.RightHandSide);
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
                foreach (ProcedureParameter parameter in fc.FunctionDeclaration.Parameters)
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
            if (varDeclaration.Block.Parent == null)
            {
                Code.Emit("ldsfld", Code.GetTypeName(varDeclaration.Type), "Oberon0." + _module.Name + "::" + varDeclaration.Name);
            }
            else
            {
                DeclarationGeneratorInfo dgi = (DeclarationGeneratorInfo)varDeclaration.GeneratorInfo;
                ProcedureParameter pp = varDeclaration as ProcedureParameter;
                if (pp != null)
                {
                    if (pp.IsVar)
                        Code.Emit("ldarg", Code.DotNumOrArg(dgi.Offset, 0, 3, false));
                    else
                        Code.Emit("ldarga", dgi.Offset);
                }
                else
                {
                    Code.Emit("ldloc", Code.DotNumOrArg(dgi.Offset, 0, 3));
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
                    }
                    else
                        throw new NotImplementedException();
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
                if (assignmentVariable.Block.Parent == null)
                {
                    Code.Emit("stsfld", Code.GetTypeName(assignmentVariable.Type), "Oberon0." + _module.Name + "::" + assignmentVariable.Name);
                }
                else
                {
                    var pp = assignmentVariable as ProcedureParameter;
                    var dgi = (DeclarationGeneratorInfo) assignmentVariable.GeneratorInfo;
                    // pp != null --> parameter otherwise local var
                    Code.Emit(pp != null ? "starg" : "stloc", Code.DotNumOrArg(dgi.Offset, 0, 3));
                }
            }
        }

        class x
        {
            internal int a;
        }

        internal void StartStoreVar(Block block, Declaration assignmentVariable, VariableSelector selector)
        {
            int n = 3;
            int[] a = new int[5];
            x[]b = new x[5];
            x c = new x();
            c.a = 42;
            b[4] = c;
            b[4].a = 1;
            a[3] = n;
            if (selector != null && selector.Any())
                Load(block, assignmentVariable, selector, true);
        }
    }
}
