#region copyright
// ***********************************************************************
// Assembly         : Oberon0.Generator.Msil
// Author           : Stephen Reindl
// Created          : 2017-01-28
// 
// ***********************************************************************
// <copyright file="CodeGenerator.Expressions.cs" company="Reindl IT">
//      Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************
#endregion

#region usings

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

#endregion

namespace Oberon0.Generator.Msil
{
    using Oberon0.Compiler.Expressions.Constant;

    /// <summary>
    /// The code generator.
    /// </summary>
    public partial class CodeGenerator
    {
        private static readonly Dictionary<int, Func<CodeGenerator, Block, BinaryExpression, Expression>>
            OperatorMapping = new Dictionary<int, Func<CodeGenerator, Block, BinaryExpression, Expression>>
                                  {
                                      { OberonGrammarLexer.PLUS, HandleSimpleOperation },
                                      { OberonGrammarLexer.MINUS, HandleSimpleOperation },
                                      { OberonGrammarLexer.MULT, HandleSimpleOperation },
                                      { OberonGrammarLexer.DIV, HandleSimpleOperation },
                                      { OberonGrammarLexer.MOD, HandleSimpleOperation },
                                      { OberonGrammarLexer.EQUAL, HandleRelOperation },
                                      { OberonGrammarLexer.NOTEQUAL, HandleRelOperation },
                                      { OberonGrammarLexer.GT, HandleRelOperation },
                                      { OberonGrammarLexer.GE, HandleRelOperation },
                                      { OberonGrammarLexer.LT, HandleRelOperation },
                                      { OberonGrammarLexer.LE, HandleRelOperation },
                                      { OberonGrammarLexer.NOT, HandleRelOperation },
                                  };

        private static readonly Dictionary<int, string> SimpleInstructionMapping =
            new Dictionary<int, string>
                {
                    { OberonGrammarLexer.PLUS, "add" },
                    { OberonGrammarLexer.DIV, "div" },
                    { OberonGrammarLexer.MULT, "mul" },
                    { OberonGrammarLexer.MINUS, "sub" },
                    { OberonGrammarLexer.MOD, "rem" },
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

        /// <summary>
        /// Compile the expression and return the register number where the result is stored
        /// </summary>
        /// <param name="block">The block.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="targetType">The target type to assign to (if needed)</param>
        /// <returns>The expression generated</returns>
        [NotNull]
        internal Expression ExpressionCompiler([NotNull] Block block, [NotNull] Expression expression, TypeDefinition targetType = null)
        {
            var expressionGeneratorInfo = new ExpressionGeneratorInfo();
            expression.GeneratorInfo = expressionGeneratorInfo;
            Expression result;

            switch (expression)
            {
                case VariableReferenceExpression v:
                    result = HandleVariableReferenceExpression(block, v);
                    break;

                case StringExpression s:
                    {
                        string str = s.Value.Remove(0, 1);
                        str = str.Remove(str.Length - 1);
                        str = str.Replace("''", "'");
                        Code.Emit("ldstr", $"\"{str}\"");
                        result = s;
                    }
                    break;

                case BinaryExpression bin:
                    Code.EmitComment(bin.ToString());
                    result = OperatorMapping[bin.Operator](this, block, bin);
                    break;

                case ConstantExpression cons:
                    LoadConstantExpression(cons, null);
                    result = cons;
                    break;

                case FunctionCallExpression fc:
                    {
                        this.Code.EmitComment(fc.FunctionDeclaration.ToString());
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
                        result = fc;
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            if (targetType != null && targetType.Type.HasFlag(BaseTypes.Simple) && targetType.Type != expression.TargetType.Type)
            {
                Code.Emit("call", Code.GetTypeName(targetType), BuildConvertString(targetType.Type, expression.TargetType.Type));
            }

            return result;
        }

        private string BuildConvertString(BaseTypes target, BaseTypes source)
        {
            switch (target)
            {
                case BaseTypes.Decimal:
                    return $"[mscorlib]System.Convert::ToDouble({Code.GetTypeName(source)})";
                case BaseTypes.Bool:
                    return $"[mscorlib]System.Convert::ToBoolean({Code.GetTypeName(source)})";
                default:
                    throw new ArgumentOutOfRangeException(nameof(target), target.ToString("F"), null);
            }
        }

        /// <summary>
        /// Load a variable
        /// </summary>
        /// <param name="block">The block this load is executed in</param>
        /// <param name="varDeclaration">The variable</param>
        /// <param name="selector">The selector provided</param>
        /// <param name="isStore"><c>true</c>, of this operation is part of a store operation (i.e. assignment)</param>
        internal void Load(Block block, Declaration varDeclaration, VariableSelector selector, bool isStore = false)
        {
            if (varDeclaration.Block.Parent == null && !(varDeclaration is ProcedureParameter))
            {
                Code.Emit("ldsfld", Code.GetTypeName(varDeclaration.Type), $"{Code.ClassName}::{varDeclaration.Name}");
            }
            else
            {
                DeclarationGeneratorInfo dgi = (DeclarationGeneratorInfo)varDeclaration.GeneratorInfo;
                if (varDeclaration is ProcedureParameter pp)
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

            if (varDeclaration.Type.Type.HasFlag(BaseTypes.Complex) && selector != null)
            {
                this.LoadComplexType(block, varDeclaration, selector, isStore);
            }
        }

        internal void StoreVar(Block block, Declaration assignmentVariable, VariableSelector selector)
        {
            BaseSelectorElement last = selector?.LastOrDefault();

            if (last != null)
            {
                if (last is IndexSelector indexSelector)
                {
                    Code.EmitStelem(indexSelector);
                }

                if (last is IdentifierSelector identSelector)
                {
                    Code.EmitStfld(identSelector);
                }
            }
            else
            {
                StoreSingletonVar(assignmentVariable, selector);
            }
        }

        internal void StartStoreVar(Block block, Declaration assignmentVariable, VariableSelector selector)
        {
            if (selector != null && selector.Any())
                Load(block, assignmentVariable, selector, true);
        }

        // "a <op> b" or "<op> a"
        private static BinaryExpression HandleSimpleOperation(CodeGenerator generator, Block block, BinaryExpression bin)
        {
            if (bin.IsUnary && bin.LeftHandSide.TargetType.Type == BaseTypes.Int)
            {
                generator.LoadConstantExpression(ConstantIntExpression.Zero, null);
            }

            if (bin.IsUnary && bin.LeftHandSide.TargetType.Type == BaseTypes.Decimal)
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

        private void StoreSingletonVar(Declaration assignmentVariable, VariableSelector selector)
        {
            if (assignmentVariable.Block.Parent == null && !(assignmentVariable is ProcedureParameter))
            {
                this.Code.Emit("stsfld", this.Code.GetTypeName(assignmentVariable.Type), $"{this.Code.ClassName}::{assignmentVariable.Name}");
            }
            else if (assignmentVariable.Type is RecordTypeDefinition && selector == null)
            {
                this.Code.Emit("stobj", "valuetype", $"{this.Code.ClassName}::{assignmentVariable.Name}");
            }
            else
            {
                var dgi = (DeclarationGeneratorInfo)assignmentVariable.GeneratorInfo;

                // pp != null --> parameter otherwise local var
                if (assignmentVariable is ProcedureParameter)
                {
                    this.Code.Emit("starg", dgi.Offset.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    this.Code.Emit("stloc" + Code.DotNumOrArg(dgi.Offset, 0, 3));
                }
            }
        }

        private void LoadComplexType(Block block, Declaration varDeclaration, VariableSelector selector, bool isStore)
        {
            foreach (BaseSelectorElement selectorElement in selector)
            {
                if (selectorElement is IndexSelector ae)
                {
                    var ad = (ArrayTypeDefinition)varDeclaration.Type;
                    this.ExpressionCompiler(block, ae.IndexDefinition);
                    if (!isStore)
                        this.Code.EmitLdelem(ae, ad);
                    continue;
                }

                if (selectorElement is IdentifierSelector ie && !isStore)
                    this.Code.Emit("ldfld", this.Code.GetTypeName(ie.Element.Type), $"{this.Code.GetTypeName(ie.Type)}::{ie.Name}");
            }
        }

        private Expression HandleVariableReferenceExpression(Block block, VariableReferenceExpression v)
        {
            if (v.IsConst)
            {
                LoadConstantExpression(((ConstDeclaration)v.Declaration).Value, (ConstDeclaration)v.Declaration);
            }
            else
            {
                Load(block, v.Declaration, v.Selector);
            }

            return v;
        }

        private void LoadConstantExpression(ConstantExpression cons, ConstDeclaration declaration)
        {
            if (declaration != null)
                Code.LoadConstRef(declaration);
            else
                Code.PushConst(cons.Value);
        }
    }
}
