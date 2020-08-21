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
    internal class Oberon0CompilerListener : OberonGrammarBaseListener
    {
        private readonly OberonGrammarParser _parser;

        public Oberon0CompilerListener(OberonGrammarParser parser)
        {
            _parser = parser;
        }

        public override void ExitArraySelector(OberonGrammarParser.ArraySelectorContext context)
        {
            context.selRet = new IndexSelector(context.e.expReturn, context.start);
        }

        public override void ExitArrayType(OberonGrammarParser.ArrayTypeContext context)
        {
            var constExpression =
                ConstantSolver.Solve(context.e.expReturn, _parser.currentBlock);
            if (constExpression is ConstantIntExpression cie)
            {
                context.returnType = new ArrayTypeDefinition(cie.ToInt32(), context.t.returnType);
            } else
            {
                _parser.NotifyErrorListeners(
                    context.Start,
                    "The array size must return a constant integer expression",
                    null);
                context.returnType = new ArrayTypeDefinition(0, context.t.returnType);
            }
        }

        public override void ExitAssign_statement(OberonGrammarParser.Assign_statementContext context)
        {
            var v = _parser.currentBlock.LookupVar(context.id.Text);
            if (v == null)
            {
                _parser.NotifyErrorListeners(context.id, $"Variable {context.id.Text} not known", null);
                return;
            }

            var targetType = v.Type;
            if (context.s.vsRet != null)
            {
                targetType = context.s.vsRet.SelectorResultType;
            }

            if (context.r?.expReturn == null)
            {
                _parser.NotifyErrorListeners(context.id, "Cannot parse right side of assignment", null);
                return;
            }

            var e = ConstantSolver.Solve(context.r.expReturn, _parser.currentBlock);
            if (!targetType.IsAssignable(e.TargetType))
            {
                _parser.NotifyErrorListeners(context.id, "Left & right side do not match types", null);
                return;
            }

            _parser.currentBlock.Statements.Add(
                new AssignmentStatement {Variable = v, Selector = context.s.vsRet, Expression = e});
        }

        public override void ExitConstDeclarationElement(OberonGrammarParser.ConstDeclarationElementContext context)
        {
            if (_parser.currentBlock.LookupVar(context.c.Text, false) != null)
            {
                _parser.NotifyErrorListeners(
                    context.c,
                    "A variable/constant with this name has been defined already",
                    null);
                return;
            }

            if (!(ConstantSolver.Solve(context.e.expReturn, _parser.currentBlock) is ConstantExpression
                constantExpression))
            {
                _parser.NotifyErrorListeners(context.e.start, "A constant must resolve during compile time", null);
                return;
            }

            var constDeclaration = new ConstDeclaration(
                context.c.Text,
                constantExpression.TargetType,
                constantExpression,
                _parser.currentBlock) {Exportable = context.export != null};

            CheckExportable(context.export, constDeclaration.Exportable);

            _parser.currentBlock.Declarations.Add(constDeclaration);
        }

        /* expressions */

        public override void ExitExprConstant(OberonGrammarParser.ExprConstantContext context)
        {
            context.expReturn = ConstantExpression.Create(context.c.Text);
        }

        public override void ExitExprNotExpression(OberonGrammarParser.ExprNotExpressionContext context)
        {
            switch (context.op.Type)
            {
                case OberonGrammarLexer.MINUS:
                    context.expReturn = BinaryExpression.Create(
                        OberonGrammarLexer.MINUS,
                        context.e.expReturn,
                        null,
                        _parser.currentBlock);
                    break;
                case OberonGrammarLexer.NOT:
                    context.expReturn = BinaryExpression.Create(
                        OberonGrammarLexer.NOT,
                        context.e.expReturn,
                        null,
                        _parser.currentBlock);
                    break;
            }
        }

        public override void ExitExprEmbeddedExpression(OberonGrammarParser.ExprEmbeddedExpressionContext context)
        {
            context.expReturn = context.e.expReturn;
        }

        public override void ExitExprSingleId(OberonGrammarParser.ExprSingleIdContext context)
        {
            var decl = _parser.currentBlock.LookupVar(context.id.Text);
            if (decl == null)
            {
                _parser.NotifyErrorListeners(context.id, "Unknown identifier: " + context.id.Text, null);
                context.expReturn = ConstantIntExpression.Zero;
                return;
            }

            context.expReturn = VariableReferenceExpression.Create(decl, context.s.vsRet);
        }

        public override void ExitExprStringLiteral(OberonGrammarParser.ExprStringLiteralContext context)
        {
            var match = Regex.Match(context.s.Text, "^'((?:[^']+|'')*)'$");
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
                _parser.currentBlock);
        }

        public override void ExitExprMultPrecedence(OberonGrammarParser.ExprMultPrecedenceContext context)
        {
            context.expReturn = BinaryExpression.Create(
                context.op.Type,
                context.l.expReturn,
                context.r.expReturn,
                _parser.currentBlock);
        }

        public override void ExitExprRelPrecedence(OberonGrammarParser.ExprRelPrecedenceContext context)
        {
            context.expReturn = BinaryExpression.Create(
                context.op.Type,
                context.l.expReturn,
                context.r.expReturn,
                _parser.currentBlock);
        }

        public override void ExitExprFuncCall(OberonGrammarParser.ExprFuncCallContext context)
        {
            var parameters = context.cp?._p.Select(x => x.expReturn).ToArray() ?? new Expression[0];
            var fp = _parser.currentBlock.LookupFunction(
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
                _parser.currentBlock,
                parameters);
        }

        public override void ExitIf_statement(OberonGrammarParser.If_statementContext context)
        {
            foreach (var expressionContext in context._c)
            {
                if (expressionContext.expReturn.TargetType.Type != BaseTypes.Bool)
                {
                    _parser.NotifyErrorListeners(
                        expressionContext.start,
                        "The condition needs to return a logical condition",
                        null);
                    return;
                }

                context.ifs.Conditions.Add(expressionContext.expReturn);
            }

            _parser.currentBlock.Statements.Add(context.ifs);
        }

        public override void ExitProcCall_statement(OberonGrammarParser.ProcCall_statementContext context)
        {
            var parameters = context.cp?._p.Select(x => x.expReturn).ToArray() ?? new Expression[0];
            var fp = _parser.currentBlock.LookupFunction(context.id.Text, context.Start, parameters);

            if (fp == null)
                // error has been reported already
            {
                return;
            }

            _parser.currentBlock.Statements.Add(new ProcedureCallStatement(fp, parameters.ToList()));
        }

        public override void ExitProcedureDeclaration(OberonGrammarParser.ProcedureDeclarationContext context)
        {
            if (context.endname._ID.Text != context.p.proc.Name)
            {
                _parser.NotifyErrorListeners(
                    context.endname._ID,
                    "The name of the procedure does not match the name after END",
                    null);
            }

            _parser.PopBlock();
            _parser.currentBlock.Procedures.Add(context.p.proc);
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
                _parser.currentBlock,
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
                    _parser.NotifyErrorListeners(parameterContext.name, "Duplicate parameter", null);
                } else
                {
                    resultSet.Add(parameterContext.param);
                }
            }

            // build result set
            context.@params = resultSet.ToArray();
        }

        public override void ExitRecordElement(OberonGrammarParser.RecordElementContext context)
        {
            foreach (var token in context._ids)
            {
                string name = token.Text;
                if (context.record.Elements.Any(x => x.Name == name))
                {
                    _parser.NotifyErrorListeners(token, $"Element {name} defined more than once", null);
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
                _parser.NotifyErrorListeners(
                    context.r.start,
                    "The condition needs to return a logical condition",
                    null);
                return;
            }

            context.rs.Condition = r;
            _parser.currentBlock.Statements.Add(context.rs);
        }

        public override void ExitSelector(OberonGrammarParser.SelectorContext context)
        {
            if (context.referenceId == null)
                // variable not found
            {
                return;
            }

            var vs = new VariableSelector(null);
            vs.AddRange(context._i.Select(selElement => selElement.selRet));
            if (!vs.Any())
            {
                return;
            }

            if (context.referenceId.Type.Type.HasFlag(BaseTypes.Simple))
            {
                _parser.NotifyErrorListeners(context.start, "Simple variables or constants do not allow any selector",
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
            var type = _parser.currentBlock.LookupType(context.ID().GetText());
            if (type == null)
            {
                _parser.NotifyErrorListeners(context.ID().Symbol, "Type not known", null);
                context.returnType = _parser.currentBlock.LookupType(TypeDefinition.VoidTypeName);
            } else
            {
                context.returnType = type;
            }
        }

        public override void ExitSingleTypeDeclaration(OberonGrammarParser.SingleTypeDeclarationContext context)
        {
            string name = context.id.Text;
            var t = _parser.currentBlock.LookupType(name);
            if (t != null)
            {
                _parser.NotifyErrorListeners(context.id, $"Type {name} declared twice", null);
                return;
            }

            // create the final type
            t = context.t.returnType.Clone(name);
            t.Exportable = context.export != null;
            CheckExportable(context.export, t.Exportable);

            _parser.currentBlock.Types.Add(t);
        }

        public override void ExitSingleVariableDeclaration(OberonGrammarParser.SingleVariableDeclarationContext context)
        {
            foreach (var token in context._v)
            {
                if (_parser.currentBlock.LookupVar(token.ID().GetText(), false) != null)
                {
                    _parser.NotifyErrorListeners(token.Start, "Variable declared twice", null);
                } else
                {
                    var declaration = new Declaration(
                        token.ID().GetText(),
                        context.t.returnType,
                        _parser.currentBlock) {Exportable = token.export != null};

                    CheckExportable(token.export, declaration.Exportable);

                    if (declaration.Exportable && !(context.t.returnType.Exportable || context.t.returnType.IsInternal))
                    {
                        _parser.NotifyErrorListeners(token.export,
                            $"Non-basic type ({context.t.returnType}) need to be exportable if used on exportable elements.",
                            null);
                    }

                    _parser.currentBlock.Declarations.Add(declaration);
                }
            }
        }

        public override void ExitWhile_statement(OberonGrammarParser.While_statementContext context)
        {
            var r = context.r.expReturn;
            if (r.TargetType != SimpleTypeDefinition.BoolType)
            {
                _parser.NotifyErrorListeners(
                    context.r.start,
                    "The condition needs to return a logical condition",
                    null);
                return;
            }

            context.ws.Condition = r;
            _parser.currentBlock.Statements.Add(context.ws);
        }

        public override void ExitImportDefinition(OberonGrammarParser.ImportDefinitionContext context)
        {
            string moduleName = context.id.Text;
            if (_parser.module.ExternalReferences.Any(x => x.GetName().Name == moduleName))
            {
                _parser.NotifyErrorListeners(context.id, $"Module {moduleName} has already been imported", null);
            }

            //TODO: Load Module
        }

        public override void ExitModuleDefinition(OberonGrammarParser.ModuleDefinitionContext context)
        {
            if (_parser.module.Name != null && _parser.module.Name != context.rId.ret?.Text)
            {
                _parser.NotifyErrorListeners(context.rId.start, "The name of the module does not match the end node",
                    null);
            }

            _parser.module.HasExports = _parser.module.Block.Declarations.Any(x => x.Exportable)
             || _parser.module.Block.Procedures.Any(x => x.Exportable)
             || _parser.module.Block.Types.Any(x => x.Exportable);
        }

        private TypeDefinition CheckArrayIndexSelector(
            IndexSelector indexSelector,
            TypeDefinition type)
        {
            if (!(type is ArrayTypeDefinition arrayType))
            {
                _parser.NotifyErrorListeners(indexSelector.Token, "Array reference expected", null);
                return SimpleTypeDefinition.VoidType;
            }

            indexSelector.IndexDefinition = ConstantSolver.Solve(indexSelector.IndexDefinition, _parser.currentBlock);
            if (indexSelector.IndexDefinition.TargetType.Type != BaseTypes.Int)
            {
                _parser.NotifyErrorListeners(indexSelector.Token, "Array reference must be INTEGER", null);
                return SimpleTypeDefinition.VoidType;
            }

            if (indexSelector.IndexDefinition.IsConst)
            {
                var ce = (ConstantExpression) indexSelector.IndexDefinition;
                int index = ce.ToInt32();
                if (index < 1 || index > arrayType.Size)
                {
                    _parser.NotifyErrorListeners(indexSelector.Token, "Array index out of bounds", null);
                    return SimpleTypeDefinition.VoidType;
                }
            }

            return arrayType.ArrayType; // types match
        }

        private TypeDefinition CheckRecordSelector(IdentifierSelector identifierSelector, TypeDefinition type)
        {
            if (!(type is RecordTypeDefinition recordType))
            {
                _parser.NotifyErrorListeners(identifierSelector.Token, "Record reference expected", null);
                return SimpleTypeDefinition.IntType;
            }

            foreach (var declaration in recordType.Elements)
            {
                if (declaration.Name == identifierSelector.Name)
                {
                    identifierSelector.Element = declaration;

                    // found
                    return declaration.Type;
                }
            }

            _parser.NotifyErrorListeners(identifierSelector.Token, "Element not found in underlying type", null);
            return SimpleTypeDefinition.VoidType;
        }

        private void CheckExportable(IToken exportElement, bool isExportable, bool checkParent = false)
        {
            if (isExportable && (_parser.currentBlock.Parent != null && !checkParent ||
                checkParent && _parser.currentBlock.Parent?.Parent != null))
            {
                _parser.NotifyErrorListeners(exportElement, "Exportable elements can only be defined as global", null);
            }
        }
    }
}
