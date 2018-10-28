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

        public override void EnterTermConstant(OberonGrammarParser.TermConstantContext context)
        {
            context.expReturn = ConstantExpression.Create(context.c.Text);
        }

        public override void ExitArraySelector(OberonGrammarParser.ArraySelectorContext context)
        {
            context.selRet = new IndexSelector(context.e.expReturn, context.start);
        }

        public override void ExitArrayType(OberonGrammarParser.ArrayTypeContext context)
        {
            ConstantIntExpression constexp =
                ConstantSolver.Solve(context.e.expReturn, context.bParam) as ConstantIntExpression;
            if (constexp == null)
            {
                this.parser.NotifyErrorListeners(
                    context.Start,
                    "The array size must return a constant integer expression",
                    null);
                constexp = (ConstantIntExpression)ConstantExpression.Create(0);
            }

            context.returnType = new ArrayTypeDefinition(constexp.ToInt32(), context.t.returnType);
        }

        public override void ExitAssign_statement(OberonGrammarParser.Assign_statementContext context)
        {
            var v = context.bParam.LookupVar(context.id.Text);
            if (v == null)
            {
                this.parser.NotifyErrorListeners(context.id, $"Variable {context.id.Text} not known", null);
                return;
            }

            var targetType = v.Type;
            if (context.s.vsRet != null)
            {
                targetType = context.s.vsRet.SelectorResultType;
            }

            Expression e = ConstantSolver.Solve(context.r.expReturn, context.bParam);
            if ((targetType.BaseType == BaseType.ComplexType && e.TargetType.BaseType != BaseType.ComplexType)
                || (targetType.BaseType == BaseType.ComplexType && e.TargetType.BaseType == BaseType.ComplexType
                                                                && targetType != e.TargetType)
                || (targetType.BaseType != e.TargetType.BaseType))
            {
                this.parser.NotifyErrorListeners(context.id, $"Left & right side do not match types", null);
                return;
            }

            context.container.Statements.Add(
                new AssignmentStatement { Variable = v, Selector = context.s.vsRet, Expression = e });
        }

        public override void ExitConstDeclarationElement(OberonGrammarParser.ConstDeclarationElementContext context)
        {
            if (context.bParam.LookupVar(context.c.Text, false) != null)
            {
                this.parser.NotifyErrorListeners(
                    context.c,
                    "A variable/constant with this name has been defined already",
                    null);
                return;
            }

            ConstantExpression constexp =
                ConstantSolver.Solve(context.e.expReturn, context.bParam) as ConstantExpression;
            if (constexp == null)
            {
                this.parser.NotifyErrorListeners(context.e.start, "A constant must resolve during compile time", null);
                return;
            }

            context.bParam.Declarations.Add(
                new ConstDeclaration(context.c.Text, constexp.TargetType, constexp, context.bParam));
        }

        public override void ExitFactorExpression(OberonGrammarParser.FactorExpressionContext context)
        {
            context.expReturn = context._s[0].expReturn;
            for (int i = 1; i < context._s.Count; i++)
            {
                context.expReturn = BinaryExpression.Create(
                    context._op[i - 1].Type,
                    context.expReturn,
                    context._s[i].expReturn,
                    context.bParam);
            }
        }

        public override void ExitIf_statement(OberonGrammarParser.If_statementContext context)
        {
            foreach (OberonGrammarParser.RelationalExpressionContext expressionContext in context._c)
            {
                if (expressionContext.expReturn.TargetType.BaseType != BaseType.BoolType)
                {
                    this.parser.NotifyErrorListeners(
                        expressionContext.start,
                        "The condition needs to return a logical condition",
                        null);
                    return;
                }

                context.ifs.Conditions.Add(expressionContext.expReturn);
            }

            context.container.Statements.Add(context.ifs);
        }

        public override void ExitNonUnaryExpressionPrefix(OberonGrammarParser.NonUnaryExpressionPrefixContext context)
        {
            context.expReturn = context.t.expReturn;
        }

        public override void ExitProcCall_statement(OberonGrammarParser.ProcCall_statementContext context)
        {
            FunctionDeclaration fp = context.bParam.LookupFunction(context.id.Text);
            if (fp == null)
            {
                this.parser.NotifyErrorListeners(context.id, "Function not known", null);
                return;
            }

            if (fp.ReturnType.BaseType != BaseType.VoidType)
            {
                this.parser.NotifyErrorListeners(
                    context.id,
                    $"Procedure {fp.Name} not known or a function with this name is called as a procedure",
                    null);
                return;
            }

            ProcedureParameter[] procParameters = fp.Block.Declarations.OfType<ProcedureParameter>().ToArray();
            Expression[] parameters = context.cp?._p?.Select(x => x.expReturn).ToArray() ?? new Expression[0];
            if (parameters.Length != procParameters.Length)
            {
                this.parser.NotifyErrorListeners(
                    context.id,
                    $"Number of parameters expected: {procParameters.Length}",
                    null);
                return;
            }

            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = ConstantSolver.Solve(parameters[i], context.bParam);
                if (procParameters[i].Type.BaseType == BaseType.AnyType)
                {
                    continue;
                }

                if (procParameters[i].Type.BaseType != parameters[i].TargetType.BaseType)
                {
                    this.parser.NotifyErrorListeners(
                        context.id,
                        $"Parameter {procParameters[i].Name} mismatch. Expected {procParameters[i].Type}, found {parameters[i].TargetType}",
                        null);
                    return;
                }

                if (procParameters[i].IsVar && !(parameters[i] is VariableReferenceExpression))
                {
                    this.parser.NotifyErrorListeners(
                        context.id,
                        $"Parameter {procParameters[i].Name} requires a variable reference, not an expression",
                        null);
                    return;
                }
            }

            context.container.Statements.Add(new ProcedureCallStatement(fp, context.bParam, parameters.ToList()));
        }

        public override void ExitProcedureDeclaration(OberonGrammarParser.ProcedureDeclarationContext context)
        {
            context.bParam.Procedures.Add(context.p.proc);
        }

        public override void ExitProcedureHeader(OberonGrammarParser.ProcedureHeaderContext context)
        {
            context.proc = new FunctionDeclaration(context.name.Text, context.bParam, context.r?.@params);
        }

        public override void ExitProcedureParameter(OberonGrammarParser.ProcedureParameterContext context)
        {
            context.param = new ProcedureParameter(
                context.name.Text,
                context.bParam,
                context.t.returnType,
                context.isVar);
        }

        public override void ExitProcedureParameters(OberonGrammarParser.ProcedureParametersContext context)
        {
            List<ProcedureParameter> resultSet = new List<ProcedureParameter>();

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

        public override void ExitRelationalExpression(OberonGrammarParser.RelationalExpressionContext context)
        {
            context.expReturn = context._s[0].expReturn;
            for (int i = 1; i < context._s.Count; i++)
            {
                context.expReturn = BinaryExpression.Create(
                    context._op[i - 1].Type,
                    context.expReturn,
                    context._s[i].expReturn,
                    context.bParam);
            }
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
            context.container.Statements.Add(context.rs);
        }

        public override void ExitSelector(OberonGrammarParser.SelectorContext context)
        {
            VariableSelector vs = new VariableSelector();
            vs.AddRange(context._i.Select(selElement => selElement.selRet));
            if (!vs.Any()) return;

            if (context.type.Type.BaseType != BaseType.ComplexType)
            {
                this.parser.NotifyErrorListeners(context.start, "Simple variables do not allow any selector", null);
                return;
            }

            TypeDefinition type = context.type.Type;
            foreach (var v in vs)
            {
                if (type == null)
                {
                    // there has been an error before (wrong index, ...). Therefore we just pass out without setting vsRet
                    return;
                }

                if (v is IdentifierSelector selector)
                {
                    selector.Type = type;
                    type = this.CheckRecordSelector(selector, type);
                }
                else if (v is IndexSelector indexSelector)
                {
                    type = this.CheckArrayIndexSelector(indexSelector, type, context);
                }
            }

            vs.SelectorResultType = type;
            context.vsRet = vs;
        }

        public override void ExitSimpleExpression(OberonGrammarParser.SimpleExpressionContext context)
        {
            context.expReturn = context._f[0].expReturn;
            for (int i = 1; i < context._f.Count; i++)
            {
                context.expReturn = BinaryExpression.Create(
                    context._op[i - 1].Type,
                    context.expReturn,
                    context._f[i].expReturn,
                    context.bParam);
            }
        }

        public override void ExitSimpleTypeName(OberonGrammarParser.SimpleTypeNameContext context)
        {
            var type = context.bParam.LookupType(context.ID().GetText());
            if (type == null)
            {
                this.parser.NotifyErrorListeners(context.ID().Symbol, "Type not known", null);
                context.returnType = context.bParam.LookupType(TypeDefinition.VoidTypeName);
            }
            else
            {
                context.returnType = type;
            }
        }

        public override void ExitSingleTypeDeclaration(OberonGrammarParser.SingleTypeDeclarationContext context)
        {
            string name = context.id.Text;
            TypeDefinition t = context.bParam.LookupType(name);
            if (t != null)
            {
                this.parser.NotifyErrorListeners(context.id, $"Type {name} declared twice", null);
                return;
            }

            // create the final type
            t = context.t.returnType.Clone(name);
            context.bParam.Types.Add(t);
        }

        public override void ExitSingleVariableDeclaration(OberonGrammarParser.SingleVariableDeclarationContext context)
        {
            foreach (var token in context._v)
            {
                if (context.bParam.LookupVar(token.Text, false) != null)
                {
                    this.parser.NotifyErrorListeners(token, "Variable declared twice", null);
                }
                else
                {
                    context.bParam.Declarations.Add(new Declaration(token.Text, context.t.returnType, context.bParam));
                }
            }
        }

        public override void ExitTermConstant(OberonGrammarParser.TermConstantContext context)
        {
            context.expReturn = ConstantExpression.Create(context.c.Text);
        }

        public override void ExitTermEmbeddedExpression(OberonGrammarParser.TermEmbeddedExpressionContext context)
        {
            context.expReturn = context.e.expReturn;
        }

        public override void ExitTermFuncCall(OberonGrammarParser.TermFuncCallContext context)
        {
            var fp = context.bParam.LookupFunction(
                context.id.Text,
                context.cp?._p?.Select(x => x.expReturn).ToList() ?? new List<Expression>());
            if (fp == null)
            {
                this.parser.NotifyErrorListeners(context.id, "Function not known", null);
                return;
            }

            context.expReturn = new FunctionCallExpression(
                fp,
                context.bParam,
                context.cp?._p?.Select(x => x.expReturn).ToArray() ?? new Expression[0]);
        }

        public override void ExitTermNotExpression(OberonGrammarParser.TermNotExpressionContext context)
        {
            context.expReturn = context.e.expReturn;
        }

        public override void ExitTermSingleId(OberonGrammarParser.TermSingleIdContext context)
        {
            context.expReturn = VariableReferenceExpression.Create(context.bParam, context.id.Text, context.s.vsRet);
        }

        public override void ExitTermStringLiteral(OberonGrammarParser.TermStringLiteralContext context)
        {
            context.expReturn = new StringExpression(context.s.Text);
        }

        public override void ExitUnaryExpressionPrefix(OberonGrammarParser.UnaryExpressionPrefixContext context)
        {
            context.expReturn = BinaryExpression.Create(
                OberonGrammarLexer.MINUS,
                context.t.expReturn,
                null,
                context.bParam);
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
            context.container.Statements.Add(context.ws);
        }

        private TypeDefinition CheckArrayIndexSelector(
            IndexSelector indexSelector,
            TypeDefinition type,
            OberonGrammarParser.SelectorContext context)
        {
            var arrayType = type as ArrayTypeDefinition;
            if (arrayType == null)
            {
                this.parser.NotifyErrorListeners(indexSelector.Token, "Array reference expected", null);
                return null;
            }

            indexSelector.IndexDefinition = ConstantSolver.Solve(indexSelector.IndexDefinition, context.bParam);
            if (indexSelector.IndexDefinition.TargetType.BaseType != BaseType.IntType)
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