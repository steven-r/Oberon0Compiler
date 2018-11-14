#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Oberon0CompilerListener.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/Oberon0CompilerListener.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler
{
    using System.Collections.Generic;
    using System.Linq;

    using Antlr4.Runtime;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions;
    using Oberon0.Compiler.Expressions.Constant;
    using Oberon0.Compiler.Solver;
    using Oberon0.Compiler.Statements;
    using Oberon0.Compiler.Types;

    internal class Oberon0CompilerListener : OberonGrammarBaseListener
    {
        private readonly OberonGrammarParser parser;

        public Oberon0CompilerListener(OberonGrammarParser parser)
        {
            this.parser = parser;
        }

        public override void ExitArraySelector(OberonGrammarParser.ArraySelectorContext context)
        {
            context.selRet = new IndexSelector(context.e.expReturn, context.start);
        }

        public override void ExitArrayType(OberonGrammarParser.ArrayTypeContext context)
        {
            var constExpression =
                ConstantSolver.Solve(context.e.expReturn, parser.currentBlock);
            if (constExpression is ConstantIntExpression cie)
            {
                context.returnType = new ArrayTypeDefinition(cie.ToInt32(), context.t.returnType);
            }
            else
            {
                this.parser.NotifyErrorListeners(
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

            if (context.r.expReturn == null)
            {
                parser.NotifyErrorListeners(context.id, "Cannot parse right side of assignment", null);
                return;
            }

            Expression e = ConstantSolver.Solve(context.r.expReturn, this.parser.currentBlock);
            if (!targetType.IsAssignable(e.TargetType))
            {
                this.parser.NotifyErrorListeners(context.id, "Left & right side do not match types", null);
                return;
            }

            parser.currentBlock.Statements.Add(
                new AssignmentStatement { Variable = v, Selector = context.s.vsRet, Expression = e });
        }

        public override void ExitConstDeclarationElement(OberonGrammarParser.ConstDeclarationElementContext context)
        {
            if (this.parser.currentBlock.LookupVar(context.c.Text, false) != null)
            {
                this.parser.NotifyErrorListeners(
                    context.c,
                    "A variable/constant with this name has been defined already",
                    null);
                return;
            }

            ConstantExpression constantExpression =
                ConstantSolver.Solve(context.e.expReturn, this.parser.currentBlock) as ConstantExpression;
            if (constantExpression == null)
            {
                parser.NotifyErrorListeners(context.e.start, "A constant must resolve during compile time", null);
                return;
            }

            parser.currentBlock.Declarations.Add(
                new ConstDeclaration(context.c.Text, constantExpression.TargetType, constantExpression, parser.currentBlock));
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
                        this.parser.currentBlock, 
                        context.op);
                    break;
                case OberonGrammarLexer.NOT:
                    context.expReturn = BinaryExpression.Create(
                        OberonGrammarLexer.NOT,
                        context.e.expReturn,
                        null,
                        this.parser.currentBlock,
                        context.op);
                    break;
            }
        }

        public override void ExitExprEmbeddedExpression(OberonGrammarParser.ExprEmbeddedExpressionContext context)
        {
            context.expReturn = context.e.expReturn;
        }

        public override void ExitExprSingleId(OberonGrammarParser.ExprSingleIdContext context)
        {
            var decl = this.parser.currentBlock.LookupVar(context.id.Text);
            if (decl == null)
            {
                this.parser.NotifyErrorListeners(context.id, "Unknown identifier: " + context.id.Text, null);
                context.expReturn = ConstantIntExpression.Zero;
            }
            else if (decl is ConstDeclaration cd)
            {
                if (context.s._i.Any())
                {
                    this.parser.NotifyErrorListeners(context.s.Start, "Selectors are not allowed for constants", null);
                }

                context.expReturn = cd.Value;
            }
            else
            {
                context.expReturn = VariableReferenceExpression.Create(this.parser.currentBlock, context.id.Text, context.s.vsRet);
            }
        }

        public override void ExitExprStringLiteral(OberonGrammarParser.ExprStringLiteralContext context)
        {
            context.expReturn = new StringExpression(context.s.Text);
        }

        public override void ExitExprFactPrecedence(OberonGrammarParser.ExprFactPrecedenceContext context)
        {
            context.expReturn = BinaryExpression.Create(
                context.op.Type,
                context.l.expReturn,
                context.r.expReturn,
                this.parser.currentBlock,
                context.op);
        }

        public override void ExitExprMultPrecedence(OberonGrammarParser.ExprMultPrecedenceContext context)
        {
            context.expReturn = BinaryExpression.Create(
                context.op.Type,
                context.l.expReturn,
                context.r.expReturn,
                this.parser.currentBlock,
                context.op);
        }

        public override void ExitExprRelPrecedence(OberonGrammarParser.ExprRelPrecedenceContext context)
        {
            context.expReturn = BinaryExpression.Create(
                context.op.Type,
                context.l.expReturn,
                context.r.expReturn,
                this.parser.currentBlock,
                context.op);
        }

        public override void ExitExprFuncCall(OberonGrammarParser.ExprFuncCallContext context)
        {
            var parameters = context.cp?._p?.Select(x => x.expReturn).ToArray() ?? new Expression[0];
            var fp = parser.currentBlock.LookupFunction(
                context.id.Text,
                context.Start,
                parameters);

            if (fp == null)
            {
                context.expReturn = new ConstantIntExpression(0);
                return;
            }

            context.expReturn = new FunctionCallExpression(
                fp,
                parser.currentBlock,
                context.Start,
                parameters);
        }

        public override void ExitIf_statement(OberonGrammarParser.If_statementContext context)
        {
            foreach (OberonGrammarParser.ExpressionContext expressionContext in context._c)
            {
                if (expressionContext.expReturn.TargetType.Type != BaseTypes.Bool)
                {
                    this.parser.NotifyErrorListeners(
                        expressionContext.start,
                        "The condition needs to return a logical condition",
                        null);
                    return;
                }

                context.ifs.Conditions.Add(expressionContext.expReturn);
            }

            this.parser.currentBlock.Statements.Add(context.ifs);
        }

        public override void ExitProcCall_statement(OberonGrammarParser.ProcCall_statementContext context)
        {
            var parameters = context.cp?._p?.Select(x => x.expReturn).ToArray() ?? new Expression[0];
            FunctionDeclaration fp = this.parser.currentBlock.LookupFunction(context.id.Text, context.Start, parameters);

            if (fp == null)
            {
                // error has been reported already
                return;
            }

            parser.currentBlock.Statements.Add(new ProcedureCallStatement(fp, parameters.ToList()));
        }

        public override void ExitProcedureDeclaration(OberonGrammarParser.ProcedureDeclarationContext context)
        {
            if (context.endname._ID.Text != context.p.proc.Name)
            {
                this.parser.NotifyErrorListeners(
                    context.endname._ID,
                    "The name of the procedure does not match the name after END",
                    null);
            }

            this.parser.PopBlock();
            this.parser.currentBlock.Procedures.Add(context.p.proc);
        }

        public override void ExitProcedureHeader(OberonGrammarParser.ProcedureHeaderContext context)
        {
            context.proc = new FunctionDeclaration(context.name.Text, context.procBlock, SimpleTypeDefinition.VoidType, context.pps?.@params);
        }

        public override void ExitProcedureParameter(OberonGrammarParser.ProcedureParameterContext context)
        {
            context.param = new ProcedureParameterDeclaration(
                context.name.Text,
                this.parser.currentBlock,
                context.t.returnType,
                context.isVar);
        }

        public override void ExitProcedureParameters(OberonGrammarParser.ProcedureParametersContext context)
        {
            List<ProcedureParameterDeclaration> resultSet = new List<ProcedureParameterDeclaration>();

            // check for double parameter names
            foreach (OberonGrammarParser.ProcedureParameterContext parameterContext in context._p)
            {
                if (context._p.Any(x => x.name.Text == parameterContext.name.Text && x != parameterContext))
                {
                    this.parser.NotifyErrorListeners(parameterContext.name, "Duplicate parameter", null);
                }
                else
                {
                    resultSet.Add(parameterContext.param);
                }
            }

            // build result set
            context.@params = resultSet.ToArray();
        }

        public override void ExitRecordElement(OberonGrammarParser.RecordElementContext context)
        {
            foreach (IToken token in context._ids)
            {
                string name = token.Text;
                if (context.record.Elements.Any(x => x.Name == name))
                {
                    this.parser.NotifyErrorListeners(token, $"Element {name} defined more than once", null);
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
                this.parser.NotifyErrorListeners(
                    context.r.start,
                    $"The condition needs to return a logical condition",
                    null);
                return;
            }

            context.rs.Condition = r;
            this.parser.currentBlock.Statements.Add(context.rs);
        }

        public override void ExitSelector(OberonGrammarParser.SelectorContext context)
        {
            VariableSelector vs = new VariableSelector();
            vs.AddRange(context._i.Select(selElement => selElement.selRet));
            if (!vs.Any()) return;

            if (context.type.Type.Type.HasFlag(BaseTypes.Simple))
            {
                parser.NotifyErrorListeners(context.start, "Simple variables do not allow any selector", null);
                return;
            }

            TypeDefinition type = context.type.Type;
            TypeDefinition baseType = context.type.Type;
            foreach (var v in vs)
            {
                if (type == null)
                {
                    // there has been an error before (wrong index, ...). Therefore we just pass out without setting vsRet
                    return;
                }

                v.BasicTypeDefinition = baseType;
                if (v is IdentifierSelector selector)
                {
                    selector.TypeDefinition = type = this.CheckRecordSelector(selector, type);
                }
                else if (v is IndexSelector indexSelector)
                {
                    indexSelector.TypeDefinition = type = this.CheckArrayIndexSelector(indexSelector, type);
                }
            }

            vs.SelectorResultType = type;
            context.vsRet = vs;
        }

        public override void ExitSimpleTypeName(OberonGrammarParser.SimpleTypeNameContext context)
        {
            var type = this.parser.currentBlock.LookupType(context.ID().GetText());
            if (type == null)
            {
                this.parser.NotifyErrorListeners(context.ID().Symbol, "Type not known", null);
                context.returnType = this.parser.currentBlock.LookupType(TypeDefinition.VoidTypeName);
            }
            else
            {
                context.returnType = type;
            }
        }

        public override void ExitSingleTypeDeclaration(OberonGrammarParser.SingleTypeDeclarationContext context)
        {
            string name = context.id.Text;
            TypeDefinition t = this.parser.currentBlock.LookupType(name);
            if (t != null)
            {
                this.parser.NotifyErrorListeners(context.id, $"Type {name} declared twice", null);
                return;
            }

            // create the final type
            t = context.t.returnType.Clone(name);
            this.parser.currentBlock.Types.Add(t);
        }

        public override void ExitSingleVariableDeclaration(OberonGrammarParser.SingleVariableDeclarationContext context)
        {
            foreach (var token in context._v)
            {
                if (this.parser.currentBlock.LookupVar(token.Text, false) != null)
                {
                    this.parser.NotifyErrorListeners(token, "Variable declared twice", null);
                }
                else
                {
                    this.parser.currentBlock.Declarations.Add(new Declaration(token.Text, context.t.returnType, this.parser.currentBlock));
                }
            }
        }

        public override void ExitWhile_statement(OberonGrammarParser.While_statementContext context)
        {
            var r = context.r.expReturn;
            if (r.TargetType != SimpleTypeDefinition.BoolType)
            {
                this.parser.NotifyErrorListeners(
                    context.r.start,
                    $"The condition needs to return a logical condition",
                    null);
                return;
            }

            context.ws.Condition = r;
            this.parser.currentBlock.Statements.Add(context.ws);
        }

        private TypeDefinition CheckArrayIndexSelector(
            IndexSelector indexSelector,
            TypeDefinition type)
        {
            var arrayType = type as ArrayTypeDefinition;
            if (arrayType == null)
            {
                this.parser.NotifyErrorListeners(indexSelector.Token, "Array reference expected", null);
                return null;
            }

            indexSelector.IndexDefinition = ConstantSolver.Solve(indexSelector.IndexDefinition, this.parser.currentBlock);
            if (indexSelector.IndexDefinition.TargetType.Type != BaseTypes.Int)
            {
                this.parser.NotifyErrorListeners(indexSelector.Token, "Array reference must be INTEGER", null);
                return null;
            }

            if (indexSelector.IndexDefinition.IsConst)
            {
                ConstantExpression ce = (ConstantExpression)indexSelector.IndexDefinition;
                int index = ce.ToInt32();
                if (index < 0 || index >= arrayType.Size)
                {
                    this.parser.NotifyErrorListeners(indexSelector.Token, "Array index of out bounds", null);
                    return null;
                }
            }

            return arrayType.ArrayType; // types match
        }

        private TypeDefinition CheckRecordSelector(IdentifierSelector identifierSelector, TypeDefinition type)
        {
            var recordType = type as RecordTypeDefinition;
            if (recordType == null)
            {
                this.parser.NotifyErrorListeners(identifierSelector.Token, "Record reference expected", null);
                return null;
            }

            foreach (Declaration declaration in recordType.Elements)
            {
                if (declaration.Name == identifierSelector.Name)
                {
                    identifierSelector.Element = declaration;

                    // found
                    return declaration.Type;
                }
            }

            this.parser.NotifyErrorListeners(identifierSelector.Token, "Element not found in underlying type", null);
            return null;
        }
    }
}