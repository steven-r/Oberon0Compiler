#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeGenerator.Expressions.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil/CodeGenerator.Expressions.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Generator.Msil
{
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
    using Oberon0.Compiler.Expressions.Constant;
    using Oberon0.Compiler.Types;

    /// <summary>
    /// The code generator.
    /// </summary>
    public partial class CodeGenerator
    {
        private static readonly Dictionary<int, Func<int, CodeGenerator, Block, BinaryExpression, Expression>> OperatorMapping = new Dictionary<int, Func<int, CodeGenerator, Block, BinaryExpression, Expression>>
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
                    { OberonGrammarLexer.AND, HandleRelOperation },
                    { OberonGrammarLexer.OR, HandleRelOperation },
                };

        private static readonly Dictionary<int, string> SimpleInstructionMapping = new Dictionary<int, string>
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
                this.DumpCode(w);
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
        internal Expression ExpressionCompiler(
            [NotNull] Block block,
            [NotNull] Expression expression,
            TypeDefinition targetType = null)
        {
            Expression result;

            switch (expression)
            {
                case VariableReferenceExpression v:
                    result = this.HandleVariableReferenceExpression(block, v);
                    break;

                case StringExpression s:
                    string str = s.Value.Remove(0, 1);
                    str = str.Remove(str.Length - 1);
                    str = str.Replace("''", "'");
                    this.Code.Emit("ldstr", $"\"{str}\"");
                    result = s;
                    break;

                case BinaryExpression bin:
                    this.Code.EmitComment(bin.ToString());
                    result = OperatorMapping[bin.Operator](bin.Operator, this, block, bin);
                    break;

                case ConstantExpression cons:
                    this.LoadConstantExpression(cons, null);
                    result = cons;
                    break;

                case FunctionCallExpression fc:
                    {
                        this.Code.EmitComment(fc.FunctionDeclaration.ToString());
                        int i = 0;
                        foreach (ProcedureParameter parameter in fc.FunctionDeclaration.Block.Declarations
                            .OfType<ProcedureParameter>())
                        {
                            if (parameter.IsVar)
                            {
                                VariableReferenceExpression reference = (VariableReferenceExpression)fc.Parameters[i];
                                this.Load(block, reference.Declaration, reference.Selector);
                            }
                            else
                            {
                                this.ExpressionCompiler(block, fc.Parameters[i]);
                            }

                            i++;
                        }

                        this.Code.Call(fc.FunctionDeclaration);
                        result = fc;
                    }

                    break;

                default:
                    throw new NotImplementedException();
            }

            if (targetType != null && targetType.Type.HasFlag(BaseTypes.Simple)
                                   && targetType.Type != expression.TargetType.Type)
            {
                this.Code.Emit(
                    "call",
                    this.Code.GetTypeName(targetType),
                    this.BuildConvertString(targetType.Type, expression.TargetType.Type));
            }

            return result;
        }

        /// <summary>
        /// Load a variable
        /// </summary>
        /// <param name="block">The block this load is executed in</param>
        /// <param name="varDeclaration">The variable</param>
        /// <param name="selector">The selector provided</param>
        /// <param name="isStore"><c>true</c>, of this operation is part of a store operation (i.e. assignment)</param>
        /// <param name="isVarParam"><c>true</c>, if function call param is "VAR" parameter</param>
        internal void Load(
            Block block,
            Declaration varDeclaration,
            VariableSelector selector,
            bool isStore = false,
            bool isVarParam = false)
        {
            if (varDeclaration.Block.Parent == null && !(varDeclaration is ProcedureParameter))
            {
                this.Code.Emit(
                    "ldsfld" + (isVarParam ? "a" : string.Empty),
                    this.Code.GetTypeName(varDeclaration.Type),
                    $"{this.Code.ClassName}::{varDeclaration.Name}");
            }
            else
            {
                DeclarationGeneratorInfo dgi = (DeclarationGeneratorInfo)varDeclaration.GeneratorInfo;
                if (varDeclaration is ProcedureParameter pp)
                {
                    this.Code.Emit("ldarg." + dgi.Offset.ToString(CultureInfo.InvariantCulture));
                    if (pp.IsVar && !isStore)
                    {
                        this.Code.Emit("ldind" + GetIndirectSuffix(pp));
                    }
                }
                else
                {
                    this.Code.Emit("ldloc" + Code.DotNumOrArg(dgi.Offset, 0, 3));
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
                    this.Code.EmitStelem(indexSelector);
                }

                if (last is IdentifierSelector identSelector)
                {
                    this.Code.EmitStfld(identSelector);
                }
            }
            else
            {
                this.StoreSingletonVar(assignmentVariable, selector);
            }
        }

        private static string GetIndirectSuffix(ProcedureParameter pp)
        {
            string suffix;
            switch (pp.Type.Type)
            {
                case BaseTypes.Int:
                case BaseTypes.Bool:
                    suffix = ".i4";
                    break;
                case BaseTypes.Real:
                    suffix = ".r8";
                    break;
                default:
                    throw new NotImplementedException();
            }

            return suffix;
        }

        // "a <rel> b" or "<rel> a" (aka ~)
        private static BinaryExpression HandleRelOperation(
            int operation,
            CodeGenerator generator,
            Block block, 
            BinaryExpression bin)
        {
            Expression left = bin.LeftHandSide;
            Expression right = bin.RightHandSide;

            if (operation == OberonGrammarLexer.GE || operation == OberonGrammarLexer.LE)
            {
                left = bin.RightHandSide;
                right = bin.LeftHandSide;
            }

            generator.ExpressionCompiler(block, left);
            if (!bin.IsUnary)
                generator.ExpressionCompiler(block, right);

            switch (bin.Operator)
            {
                case OberonGrammarLexer.GT:
                    generator.Code.Emit("cgt");
                    break;
                case OberonGrammarLexer.LT:
                    generator.Code.Emit("clt");
                    break;
                case OberonGrammarLexer.EQUAL:
                case OberonGrammarLexer.NOTEQUAL:
                    generator.Code.Emit("ceq");
                    break;
                case OberonGrammarLexer.NOT:
                    generator.Code.Emit("not");
                    break;
                case OberonGrammarLexer.AND:
                    generator.Code.Emit("and");
                    break;
                case OberonGrammarLexer.OR:
                    generator.Code.Emit("or");
                    break;
                case OberonGrammarLexer.GE:
                    generator.Code.Emit("cgt");
                    generator.Code.PushConst(0);
                    generator.Code.Emit("ceq");
                    break;
                case OberonGrammarLexer.LE:
                    generator.Code.Emit("clt");
                    generator.Code.PushConst(0);
                    generator.Code.Emit("ceq");
                    break;
                default:
                    throw new NotImplementedException("Bin case not handled");
            }

            return bin;
        }

        // "a <op> b" or "<op> a"
        private static BinaryExpression HandleSimpleOperation(
            int operation,
            CodeGenerator generator,
            Block block,
            BinaryExpression bin)
        {
            if (bin.IsUnary && bin.LeftHandSide.TargetType.Type == BaseTypes.Int)
            {
                generator.LoadConstantExpression(ConstantIntExpression.Zero, null);
            }

            if (bin.IsUnary && bin.LeftHandSide.TargetType.Type == BaseTypes.Real)
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

        private string BuildConvertString(BaseTypes target, BaseTypes source)
        {
            switch (target)
            {
                case BaseTypes.Real:
                    return $"[mscorlib]System.Convert::ToDouble({Code.GetTypeName(source)})";
                case BaseTypes.Bool:
                    return $"[mscorlib]System.Convert::ToBoolean({Code.GetTypeName(source)})";
                default:
                    throw new ArgumentOutOfRangeException(nameof(target), target.ToString("F"), null);
            }
        }

        private Expression HandleVariableReferenceExpression(Block block, VariableReferenceExpression v)
        {
            if (v.IsConst)
            {
                this.LoadConstantExpression(((ConstDeclaration)v.Declaration).Value, (ConstDeclaration)v.Declaration);
            }
            else
            {
                this.Load(block, v.Declaration, v.Selector);
            }

            return v;
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
                    this.Code.Emit(
                        "ldfld",
                        this.Code.GetTypeName(ie.Element.Type),
                        $"{this.Code.GetTypeName(ie.Type)}::{ie.Name}");
            }
        }

        private void LoadConstantExpression(ConstantExpression cons, ConstDeclaration declaration)
        {
            if (declaration != null) this.Code.LoadConstRef(declaration);
            else this.Code.PushConst(cons.Value);
        }

        private void StoreSingletonVar(Declaration assignmentVariable, VariableSelector selector)
        {
            if (assignmentVariable.Block.Parent == null && !(assignmentVariable is ProcedureParameter))
            {
                this.Code.Emit(
                    "stsfld",
                    this.Code.GetTypeName(assignmentVariable.Type),
                    $"{this.Code.ClassName}::{assignmentVariable.Name}");
            }
            else if (assignmentVariable.Type is RecordTypeDefinition && selector == null)
            {
                this.Code.Emit("stobj", "valuetype", $"{this.Code.ClassName}::{assignmentVariable.Name}");
            }
            else
            {
                var dgi = (DeclarationGeneratorInfo)assignmentVariable.GeneratorInfo;

                // pp != null --> parameter otherwise local var
                if (assignmentVariable is ProcedureParameter pp)
                {
                    if (pp.IsVar)
                    {
                        var suffix = GetIndirectSuffix(pp);
                        this.Code.Emit("stind" + suffix);
                    }
                    else
                    {
                        this.Code.Emit("starg", dgi.Offset.ToString(CultureInfo.InvariantCulture));
                    }
                }
                else
                {
                    this.Code.Emit("stloc" + Code.DotNumOrArg(dgi.Offset, 0, 3));
                }
            }
        }
    }
}