#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Antlr4.Runtime;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Solver;
using Oberon0.Compiler.Statements;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler
{
    internal partial class Oberon0CompilerListener(OberonGrammarParser parser) : OberonGrammarBaseListener
    {
        public override void ExitArraySelector(OberonGrammarParser.ArraySelectorContext context)
        {
            context.selRet = new IndexSelector(context.e.expReturn, context.Start);
        }

        public override void ExitArrayType(OberonGrammarParser.ArrayTypeContext context)
        {
            var constExpression =
                ConstantSolver.Solve(context.e.expReturn, parser.currentBlock);
            if (constExpression is ConstantIntExpression cie)
            {
                context.returnType = new ArrayTypeDefinition(cie.ToInt32(), context.t.returnType);
            } else
            {
                parser.NotifyErrorListeners(
                    context.Start,
                    "The array size must return a constant integer expression",
                    null);
                context.returnType = new ArrayTypeDefinition(0, context.t.returnType);
            }
        }

        public override void ExitAssign_statement(OberonGrammarParser.Assign_statementContext context)
        {
            var v = parser.currentBlock.LookupVar(context.id.Text);
            if (v == null)
            {
                parser.NotifyErrorListeners(context.id, $"Variable {context.id.Text} not known", null);
                return;
            }

            var targetType = v.Type;
            if (context.s.vsRet != null)
            {
                targetType = context.s.vsRet.SelectorResultType;
            }

            if (context.r?.expReturn == null)
            {
                parser.NotifyErrorListeners(context.id, "Cannot parse right side of assignment", null);
                return;
            }

            var e = ConstantSolver.Solve(context.r.expReturn, parser.currentBlock);
            if (!targetType.IsAssignable(e.TargetType))
            {
                parser.NotifyErrorListeners(context.id, "Left & right side do not match types", null);
                return;
            }

            parser.currentBlock.Statements.Add(
                new AssignmentStatement {Variable = v, Selector = context.s.vsRet, Expression = e});
        }

        public override void ExitConstDeclarationElement(OberonGrammarParser.ConstDeclarationElementContext context)
        {
            if (parser.currentBlock.LookupVar(context.c.Text, false) != null)
            {
                parser.NotifyErrorListeners(
                    context.c,
                    "A variable/constant with this name has been defined already",
                    null);
                return;
            }

            if (ConstantSolver.Solve(context.e.expReturn, parser.currentBlock) is not ConstantExpression
                constantExpression)
            {
                parser.NotifyErrorListeners(context.e.Start, "A constant must resolve during compile time", null);
                return;
            }

            var constDeclaration = new ConstDeclaration(
                context.c.Text,
                constantExpression.TargetType,
                constantExpression,
                parser.currentBlock) {Exportable = context.export != null};

            CheckExportable(context.export, constDeclaration.Exportable);

            parser.currentBlock.Declarations.Add(constDeclaration);
        }

        /* expressions */

        public override void ExitExprIntegerNumber(OberonGrammarParser.ExprIntegerNumberContext context)
        {
            context.expReturn = ConstantExpression.Create(context.int_n.Text, true);
        }

        public override void ExitExprFloatingNumber(OberonGrammarParser.ExprFloatingNumberContext context)
        {
            context.expReturn = ConstantExpression.Create(context.int_r.Text);
        }

        public override void ExitExprNotExpression(OberonGrammarParser.ExprNotExpressionContext context)
        {
            context.expReturn = context.op.Type switch
            {
                OberonGrammarLexer.MINUS => BinaryExpression.Create(OberonGrammarLexer.MINUS, context.e.expReturn, null,
                    parser.currentBlock),
                OberonGrammarLexer.NOT => BinaryExpression.Create(OberonGrammarLexer.NOT, context.e.expReturn, null,
                    parser.currentBlock),
                _ => null
            };
            if (context.expReturn == null)
            {
                parser.NotifyErrorListeners(context.op,
                    $"Expression is not compatible with {context.op.Text}", null);
            }
        }

        public override void ExitExprEmbeddedExpression(OberonGrammarParser.ExprEmbeddedExpressionContext context)
        {
            context.expReturn = context.e.expReturn;
        }

        public override void ExitExprSingleId(OberonGrammarParser.ExprSingleIdContext context)
        {
            var decl = parser.currentBlock.LookupVar(context.id.Text);
            if (decl == null)
            {
                parser.NotifyErrorListeners(context.id, "Unknown identifier: " + context.id.Text, null);
                context.expReturn = ConstantIntExpression.Zero;
                return;
            }

            context.expReturn = VariableReferenceExpression.Create(decl, context.s.vsRet);
        }

        [GeneratedRegex("^'((?:[^']+|'')*)'$")]
        internal static partial Regex StringLiteralRegex();

        public override void ExitExprStringLiteral(OberonGrammarParser.ExprStringLiteralContext context)
        {
            var match = StringLiteralRegex().Match(context.s.Text);
            if (match.Success)
            {
                string data = match.Groups[1].Value.Replace("''", "'");
                context.expReturn = new StringExpression(data);
            }
        }

        public override void ExitExprFactPrecedence(OberonGrammarParser.ExprFactPrecedenceContext context)
        {
            context.expReturn = BinaryExpression.Create(
                context.op.Type,
                context.l.expReturn,
                context.r.expReturn,
                parser.currentBlock);
            if (context.expReturn == null)
            {
                parser.NotifyErrorListeners(context.op,
                    $"Left and right expression are not compatible with {context.op.Text}", null);
            }
        }

        public override void ExitExprMultPrecedence(OberonGrammarParser.ExprMultPrecedenceContext context)
        {
            context.expReturn = BinaryExpression.Create(
                context.op.Type,
                context.l.expReturn,
                context.r.expReturn,
                parser.currentBlock);
            if (context.expReturn == null)
            {
                parser.NotifyErrorListeners(context.op,
                    $"Left and right expression are not compatible with {context.op.Text}", null);
            } 
        }

        public override void ExitExprRelPrecedence(OberonGrammarParser.ExprRelPrecedenceContext context)
        {
            context.expReturn = BinaryExpression.Create(
                context.op.Type,
                context.l.expReturn,
                context.r.expReturn,
                parser.currentBlock);
            if (context.expReturn == null)
            {
                parser.NotifyErrorListeners(context.op,
                    $"Left and right expression are not compatible with {context.op.Text}", null);
            } 
        }

        public override void ExitExprFuncCall(OberonGrammarParser.ExprFuncCallContext context)
        {
            var parameters = context.cp?._p.Select(x => x.expReturn).ToArray() ?? [];
            var fp = parser.currentBlock.LookupFunction(
                context.id.Text,
                context.Start,
                parameters);

            if (fp == null)
            {
                // error has been reported by LookupFunction
                context.expReturn = new ConstantIntExpression(0);
                return;
            }

            context.expReturn = new FunctionCallExpression(
                fp,
                parser.currentBlock,
                parameters);
        }

        public override void ExitIf_statement(OberonGrammarParser.If_statementContext context)
        {
            foreach (var expressionContext in context._c)
            {
                if (expressionContext.expReturn.TargetType.Type != BaseTypes.Bool)
                {
                    parser.NotifyErrorListeners(
                        expressionContext.Start,
                        "The condition needs to return a logical condition",
                        null);
                    return;
                }

                context.ifs.Conditions.Add(expressionContext.expReturn);
            }

            parser.currentBlock.Statements.Add(context.ifs);
        }

        public override void ExitProcCall_statement(OberonGrammarParser.ProcCall_statementContext context)
        {
            var parameters = context.cp?._p.Select(x => x.expReturn).ToArray() ?? [];
            var fp = parser.currentBlock.LookupFunction(context.id.Text, context.Start, parameters);

            if (fp == null)
                // error has been reported already
            {
                return;
            }

            parser.currentBlock.Statements.Add(new ProcedureCallStatement(fp, [.. parameters]));
        }

        public override void ExitProcedureDeclaration(OberonGrammarParser.ProcedureDeclarationContext context)
        {
            if (context.endname._ID.Text != context.p.proc.Name)
            {
                parser.NotifyErrorListeners(
                    context.endname._ID,
                    "The name of the procedure does not match the name after END",
                    null);
            }

            parser.PopBlock();
            parser.currentBlock.Procedures.Add(context.p.proc);
        }

        public override void ExitProcedureHeader(OberonGrammarParser.ProcedureHeaderContext context)
        {
            context.proc = new FunctionDeclaration(
                context.name.Text,
                context.procBlock,
                SimpleTypeDefinition.VoidType,
                context.pps?.@params)
            {
                Exportable = context.export != null
            };
            CheckExportable(context.export, context.proc.Exportable, true);
        }

        public override void ExitProcedureParameter(OberonGrammarParser.ProcedureParameterContext context)
        {
            context.param = new ProcedureParameterDeclaration(
                context.name.Text,
                parser.currentBlock,
                context.t.returnType,
                context.isVar);
        }

        public override void ExitProcedureParameters(OberonGrammarParser.ProcedureParametersContext context)
        {
            var resultSet = new List<ProcedureParameterDeclaration>();

            // check for double parameter names
            foreach (var parameterContext in context._p)
            {
                if (context._p.Any(x => x.name.Text == parameterContext.name.Text && x != parameterContext))
                {
                    parser.NotifyErrorListeners(parameterContext.name, "Duplicate parameter", null);
                } else
                {
                    resultSet.Add(parameterContext.param);
                }
            }

            // build result set
            context.@params = [..resultSet];
        }

        public override void ExitRecordElement(OberonGrammarParser.RecordElementContext context)
        {
            foreach (var token in context._ids)
            {
                string name = token.Text;
                if (context.record.Elements.Exists(x => x.Name == name))
                {
                    parser.NotifyErrorListeners(token, $"Element {name} defined more than once", null);
                    continue; // ignore this element
                }

                context.record.Elements.Add(new Declaration(name, context.t.returnType));
            }
        }

        public override void ExitRecordSelector(OberonGrammarParser.RecordSelectorContext context)
        {
            context.selRet = new IdentifierSelector(context.ID().GetText(), context.ID().Symbol);
        }

        public override void ExitRecordTypeName(OberonGrammarParser.RecordTypeNameContext context)
        {
            context.returnType = context.r.returnType;
        }

        public override void ExitRecordTypeNameElements(OberonGrammarParser.RecordTypeNameElementsContext context)
        {
            context.returnType = context.record;
        }

        public override void ExitRepeat_statement(OberonGrammarParser.Repeat_statementContext context)
        {
            var r = context.r.expReturn;
            if (r.TargetType != SimpleTypeDefinition.BoolType)
            {
                parser.NotifyErrorListeners(
                    context.r.Start,
                    "The condition needs to return a logical condition",
                    null);
                return;
            }

            context.rs.Condition = r;
            parser.currentBlock.Statements.Add(context.rs);
        }

        public override void ExitSelector(OberonGrammarParser.SelectorContext context)
        {
            if (context.referenceId == null)
                // variable not found
            {
                return;
            }

            var vs = new VariableSelector();
            vs.AddRange(context._i.Select(selElement => selElement.selRet));
            if (!vs.Any())
            {
                return;
            }

            if (context.referenceId.Type.Type.HasFlag(BaseTypes.Simple))
            {
                parser.NotifyErrorListeners(context.Start, "Simple variables or constants do not allow any selector",
                    null);
                return;
            }

            var type = context.referenceId.Type;
            foreach (var v in vs)
            {
                switch (v)
                {
                    case IdentifierSelector selector:
                        selector.TypeDefinition = type = CheckRecordSelector(selector, type);
                        break;
                    case IndexSelector indexSelector:
                        indexSelector.TypeDefinition = type = CheckArrayIndexSelector(indexSelector, type);
                        break;
                }
            }

            vs.SelectorResultType = type;
            context.vsRet = vs;
        }

        public override void ExitSimpleTypeName(OberonGrammarParser.SimpleTypeNameContext context)
        {
            var type = parser.currentBlock.LookupType(context.ID().GetText());
            if (type == null)
            {
                parser.NotifyErrorListeners(context.ID().Symbol, "Type not known", null);
                context.returnType = parser.currentBlock.LookupType(TypeDefinition.VoidTypeName);
            } else
            {
                context.returnType = type;
            }
        }

        public override void ExitSingleTypeDeclaration(OberonGrammarParser.SingleTypeDeclarationContext context)
        {
            string name = context.id.Text;
            var t = parser.currentBlock.LookupType(name);
            if (t != null)
            {
                parser.NotifyErrorListeners(context.id, $"Type {name} declared twice", null);
                return;
            }

            // create the final type
            t = context.t.returnType.Clone(name);
            t.Exportable = context.export != null;
            CheckExportable(context.export, t.Exportable);

            parser.currentBlock.Types.Add(t);
        }

        public override void ExitSingleVariableDeclaration(OberonGrammarParser.SingleVariableDeclarationContext context)
        {
            foreach (var token in context._v)
            {
                if (parser.currentBlock.LookupVar(token.ID().GetText(), false) != null)
                {
                    parser.NotifyErrorListeners(token.Start, "Variable declared twice", null);
                } else
                {
                    var declaration = new Declaration(
                        token.ID().GetText(),
                        context.t.returnType,
                        parser.currentBlock) {Exportable = token.export != null};

                    CheckExportable(token.export, declaration.Exportable);

                    if (declaration.Exportable && !(context.t.returnType.Exportable || context.t.returnType.IsInternal))
                    {
                        parser.NotifyErrorListeners(token.export,
                            $"Non-basic type ({context.t.returnType}) need to be exportable if used on exportable elements.",
                            null);
                    }

                    parser.currentBlock.Declarations.Add(declaration);
                }
            }
        }

        public override void ExitWhile_statement(OberonGrammarParser.While_statementContext context)
        {
            var r = context.r.expReturn;
            if (r.TargetType != SimpleTypeDefinition.BoolType)
            {
                parser.NotifyErrorListeners(
                    context.r.Start,
                    "The condition needs to return a logical condition",
                    null);
                return;
            }

            context.ws.Condition = r;
            parser.currentBlock.Statements.Add(context.ws);
        }

        public override void ExitImportDefinition(OberonGrammarParser.ImportDefinitionContext context)
        {
            string moduleName = context.id.Text;
            if (parser.module.ExternalReferences.Exists(x => x.GetName().Name == moduleName))
            {
                parser.NotifyErrorListeners(context.id, $"Module {moduleName} has already been imported", null);
            }

            //TODO: Load Module
        }

        public override void ExitModuleDefinition(OberonGrammarParser.ModuleDefinitionContext context)
        {
            if (parser.module.Name != null && parser.module.Name != context.rId.ret?.Text)
            {
                parser.NotifyErrorListeners(context.rId.Start, "The name of the module does not match the end node",
                    null);
            }

            parser.module.HasExports = parser.module.Block.Declarations.Exists(x => x.Exportable)
             || parser.module.Block.Procedures.Exists(x => x.Exportable)
             || parser.module.Block.Types.Exists(x => x.Exportable);
        }

        private TypeDefinition CheckArrayIndexSelector(
            IndexSelector indexSelector,
            TypeDefinition type)
        {
            if (type is not ArrayTypeDefinition arrayType)
            {
                parser.NotifyErrorListeners(indexSelector.Token, "Array reference expected", null);
                return SimpleTypeDefinition.VoidType;
            }

            indexSelector.IndexDefinition = ConstantSolver.Solve(indexSelector.IndexDefinition, parser.currentBlock);
            if (indexSelector.IndexDefinition.TargetType.Type != BaseTypes.Int)
            {
                parser.NotifyErrorListeners(indexSelector.Token, "Array reference must be INTEGER", null);
                return SimpleTypeDefinition.VoidType;
            }

            if (indexSelector.IndexDefinition.IsConst)
            {
                var ce = (ConstantExpression) indexSelector.IndexDefinition;
                int index = ce.ToInt32();
                if (index < 1 || index > arrayType.Size)
                {
                    parser.NotifyErrorListeners(indexSelector.Token, "Array index out of bounds", null);
                    return SimpleTypeDefinition.VoidType;
                }
            }

            return arrayType.ArrayType; // types match
        }

        private TypeDefinition CheckRecordSelector(IdentifierSelector identifierSelector, TypeDefinition type)
        {
            if (type is not RecordTypeDefinition recordType)
            {
                parser.NotifyErrorListeners(identifierSelector.Token, "Record reference expected", null);
                return SimpleTypeDefinition.IntType;
            }

            var declaration = recordType.Elements.Find(declaration => declaration.Name == identifierSelector.Name);
            if (declaration != null)
            {
                // found
                identifierSelector.Element = declaration;
                return declaration.Type;
            }

            parser.NotifyErrorListeners(identifierSelector.Token, "Element not found in underlying type", null);
            return SimpleTypeDefinition.VoidType;
        }

        private void CheckExportable(IToken? exportElement, bool isExportable, bool checkParent = false)
        {
            if (isExportable && (parser.currentBlock.Parent != null && !checkParent ||
                checkParent && parser.currentBlock.Parent?.Parent != null))
            {
                parser.NotifyErrorListeners(exportElement, "Exportable elements can only be defined as global", null);
            }
        }
    }
}
