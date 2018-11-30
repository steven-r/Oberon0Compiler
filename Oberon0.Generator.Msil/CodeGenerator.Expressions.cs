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
                    { OberonGrammarLexer.STAR, HandleSimpleOperation },
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
                { OberonGrammarLexer.STAR, "mul" },
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
                        var parameters = fc.FunctionDeclaration.Block.Declarations
                            .OfType<ProcedureParameterDeclaration>().ToList();
                        foreach (ProcedureParameterDeclaration parameter in parameters)
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

                        this.Code.Call(this, fc.FunctionDeclaration, fc.Parameters);
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
        /// <param name="isCall"><c>true</c>, if used as parameter for a function call</param>
        /// <param name="ignoreReplacement">if <c>true</c>, replacements are not resolved</param>
        internal void Load(
            Block block,
            Declaration varDeclaration,
            VariableSelector selector,
            bool isStore = false,
            bool isVarParam = false,
            bool isCall = false,
            bool ignoreReplacement = false)
        {
            DeclarationGeneratorInfo dgi = (DeclarationGeneratorInfo)varDeclaration.GeneratorInfo;
            if (!ignoreReplacement && dgi?.ReplacedBy != null)
            {
                varDeclaration = dgi.ReplacedBy;
                dgi = (DeclarationGeneratorInfo)varDeclaration.GeneratorInfo;
            }

            if (dgi == null)
            {
                throw new NotImplementedException("internal error - dgi is null");
            }

            this.DoLoad(varDeclaration, isStore, isVarParam, dgi);

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

        private void DoLoad(Declaration varDeclaration, bool isStore, bool isVarParam, DeclarationGeneratorInfo dgi)
        {
            if (varDeclaration.Block.Parent == null)
            {
                this.Code.Emit(
                    "ldsfld" + (isVarParam ? "a" : string.Empty),
                    this.Code.GetTypeName(varDeclaration.Type),
                    $"{this.Code.ClassName}::{Code.MakeName(varDeclaration.Name)}");
            }
            else
            {
                if (varDeclaration is ProcedureParameterDeclaration pp)
                {
                    this.Code.Emit("ldarg." + dgi.Offset.ToString(CultureInfo.InvariantCulture));
                    var isVar = pp.IsVar && pp.Type.Type.HasFlag(BaseTypes.Simple);
                    if (isVar && !isStore)
                    {
                        this.Code.Emit("ldind" + Code.GetIndirectSuffix(pp.Type));
                    }
                }
                else
                {
                    this.Code.Emit("ldloc" + Code.DotNumOrArg(dgi.Offset, 0, 3));
                }
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
                DeclarationGeneratorInfo dgi = v.Declaration.GeneratorInfo as DeclarationGeneratorInfo;
                Declaration d = v.Declaration;
                if (dgi?.ReplacedBy != null)
                {
                    d = dgi.ReplacedBy;
                }

                this.Load(block, d, v.Selector);
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
                    this.Code.PushConst(1);
                    this.Code.Emit("sub"); // rebase to 0
                    if (!isStore)
                        this.Code.EmitLdelem(ae, ad);
                    continue;
                }

                if (selectorElement is IdentifierSelector ie && !isStore)
                    this.Code.Emit(
                        "ldfld",
                        this.Code.GetTypeName(ie.Element.Type),
                        $"{this.Code.GetTypeName(ie.BasicTypeDefinition)}::{Code.MakeName(ie.Name)}");
            }
        }

        private void LoadConstantExpression(ConstantExpression cons, ConstDeclaration declaration)
        {
            if (declaration != null) this.Code.LoadConstRef(declaration);
            else this.Code.PushConst(cons.Value);
        }

        private void StoreSingletonVar(Declaration assignmentVariable, VariableSelector selector)
        {
            var dgi = (DeclarationGeneratorInfo)assignmentVariable.GeneratorInfo;
            if (dgi.ReplacedBy != null)
            {
                assignmentVariable = dgi.ReplacedBy;
                dgi = (DeclarationGeneratorInfo)assignmentVariable.GeneratorInfo;
            }

            if (dgi == null)
            {
                throw new NotImplementedException("Internal error - dgi is null");
            }

            if (assignmentVariable.Block.Parent == null && !(assignmentVariable is ProcedureParameterDeclaration))
            {
                this.Code.Emit(
                    "stsfld",
                    this.Code.GetTypeName(assignmentVariable.Type),
                    $"{this.Code.ClassName}::{Code.MakeName(assignmentVariable.Name)}");
            }
            else if (assignmentVariable.Type is RecordTypeDefinition && selector == null)
            {
                this.Code.Emit("stloc" + Code.DotNumOrArg(dgi.Offset, 0, 3));
            }
            else
            {
                // pp != null --> parameter otherwise local var
                if (assignmentVariable is ProcedureParameterDeclaration pp)
                {
                    if (pp.IsVar)
                    {
                        var suffix = Code.GetIndirectSuffix(pp.Type);
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