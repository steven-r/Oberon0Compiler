#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeGenerator.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil/CodeGenerator.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Generator.Msil
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Oberon0.Compiler;
    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions;
    using Oberon0.Compiler.Statements;
    using Oberon0.Compiler.Types;
    using Oberon0.Generator.Msil.PredefinedFunctions;

    /// <summary>
    /// The code generator to generate RISC code
    /// </summary>
    public partial class CodeGenerator
    {
        /// <summary>
        /// The default namespace use for standard functions like <c>WriteInt</c>, ...
        /// </summary>
        public const string DefaultNamespace = "$DEFAULT";

        private static readonly Dictionary<int, string> RelRevJumpMapping = new Dictionary<int, string>
            {
                { OberonGrammarLexer.EQUAL, "brfalse" },
                { OberonGrammarLexer.AND, "brfalse" },
                { OberonGrammarLexer.OR, "brfalse" },
                { OberonGrammarLexer.LT, "brfalse" },
                { OberonGrammarLexer.LE, "brfalse" },
                { OberonGrammarLexer.GT, "brfalse" },
                { OberonGrammarLexer.GE, "brfalse" },
                { OberonGrammarLexer.NOTEQUAL, "brtrue" },
                { OberonGrammarLexer.NOT, "brtrue" }
            };

        private readonly Module module;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeGenerator"/> class.
        /// </summary>
        /// <param name="module">The parsed module.</param>
        public CodeGenerator(Module module)
        {
            this.OutputBuffer = new StringBuilder();
            this.module = module;
            this.Code = new Code(this.OutputBuffer);
        }

        /// <summary>
        /// Gets the <see cref="Code"/> reference
        /// </summary>
        /// <value>The code.</value>
        public Code Code { get; }

        private StringBuilder OutputBuffer { get; }

        public void DumpCode(TextWriter writer)
        {
            writer.Write(this.OutputBuffer.ToString());
        }

        public void Generate()
        {
            StandardFunctionRepository.Initialize(this.module);
            this.Code.Reference("mscorlib");
            this.Code.Reference("Oberon0.System");
            this.Code.StartAssembly(this.module.Name);
            this.Code.EmitComment("Code compiled for module " + this.module.Name);
            this.Code.StartModule(this.module.Name);
            this.Code.StartClass($"Oberon0.{this.module.Name}");
            this.ProcessMainBlock(this.module.Block);
            this.Code.EndMethod();
            this.Code.EndClass();
        }

        private void CallInternalFunction(ProcedureCallStatement call, Block block)
        {
            var function = StandardFunctionRepository.Get(call.FunctionDeclaration);
            function.Instance.Generate(function, this, call.FunctionDeclaration, call.Parameters, block);
        }

        private void CallProcedure(ProcedureCallStatement call, Block block)
        {
            this.Code.EmitComment("Call " + call.FunctionDeclaration.Name);
            int i = 0;
            if (call.FunctionDeclaration.IsInternal)
            {
                this.CallInternalFunction(call, block);
            }
            else
            {
                foreach (ProcedureParameter parameter in call.FunctionDeclaration.Block.Declarations
                    .OfType<ProcedureParameter>())
                {
                    if (parameter.IsVar)
                    {
                        VariableReferenceExpression reference = (VariableReferenceExpression)call.Parameters[i];
                        this.Load(block, reference.Declaration, reference.Selector, isVarParam: true);
                    }
                    else
                    {
                        this.ExpressionCompiler(block, call.Parameters[i]);
                    }

                    i++;
                }

                this.Code.Call(call.FunctionDeclaration);
            }
        }

        private void GenerateAssignmentStatement(Block block, AssignmentStatement assignment)
        {
            this.Code.EmitComment(assignment.Variable + " := " + assignment.Expression);
            var isVar = (assignment.Variable is ProcedureParameter pp) && pp.IsVar;
            if (isVar || (assignment.Selector != null && assignment.Selector.Any()))
            {
                this.Load(block, assignment.Variable, assignment.Selector, true);
            }

            this.ExpressionCompiler(
                block,
                assignment.Expression,
                this.GetTargetType(assignment.Variable.Type, assignment.Selector));
            this.StoreVar(block, assignment.Variable, assignment.Selector);
        }

        private void GenerateIfStatement(IfStatement stmt, Block block)
        {
            string endLabel = this.Code.GetLabel();
            this.Code.EmitComment("IF");
            for (int i = 0; i < stmt.Conditions.Count; i++)
            {
                string nextLabel = this.Code.GetLabel();
                var expression = this.ExpressionCompiler(block, stmt.Conditions[i]);
                if (expression is BinaryExpression bin)
                {
                    this.Code.Branch(RelRevJumpMapping[bin.Operator], nextLabel);
                }
                else if (expression is VariableReferenceExpression)
                {
                    this.Code.Branch(RelRevJumpMapping[OberonGrammarLexer.NOT], nextLabel);
                }
                else
                {
                    throw new NotImplementedException("Unknown IF condition " + expression.GetType().FullName);
                }

                this.ProcessStatements(stmt.ThenParts[i]);
                this.Code.Branch("br", endLabel);
                this.Code.EmitLabel(nextLabel);
            }

            if (stmt.ElsePart != null) this.ProcessStatements(stmt.ElsePart);
            this.Code.EmitLabel(endLabel);
        }

        private void GenerateRepeatStatement(RepeatStatement stmt, Block block)
        {
            this.Code.EmitComment("REPEAT");
            string label = this.Code.EmitLabel();
            this.ProcessStatements(stmt.Block);
            BinaryExpression bin = (BinaryExpression)this.ExpressionCompiler(block, stmt.Condition);
            this.Code.Branch(RelRevJumpMapping[bin.Operator], label);
        }

        private void GenerateWhileStatement(WhileStatement stmt, Block block)
        {
            string endLabel = this.Code.GetLabel();
            this.Code.EmitComment("WHILE");
            string label = this.Code.EmitLabel();
            BinaryExpression bin = (BinaryExpression)this.ExpressionCompiler(block, stmt.Condition);
            this.Code.Branch(RelRevJumpMapping[bin.Operator], endLabel);
            this.ProcessStatements(stmt.Block);
            this.Code.Branch("br", label);
            this.Code.EmitLabel(endLabel);
        }

        private TypeDefinition GetTargetType(TypeDefinition baseType, VariableSelector selector)
        {
            if (baseType.Type.HasFlag(BaseTypes.Simple))
            {
                return baseType;
            }

            return selector.SelectorResultType;
        }

        private void ProcessMainBlock(Block block)
        {
            this.ProcessDeclarations(block, true);
            foreach (var functionDeclaration in block.Procedures)
            {
                // ignore system and external libraries
                if (functionDeclaration.IsInternal || functionDeclaration.IsExternal)
                    continue;
                this.Code.StartMethod(functionDeclaration);
                this.ProcessDeclarations(functionDeclaration.Block);
                this.ProcessStatements(functionDeclaration.Block);
                this.Code.EndMethod();
            }

            this.Code.StartMainMethod();
            this.ProcessStatements(block);
        }

        private void ProcessStatements(Block block)
        {
            this.InitComplexData(block);
            foreach (var statement in block.Statements)
            {
                switch (statement)
                {
                    case AssignmentStatement assignment:
                        this.GenerateAssignmentStatement(block, assignment);
                        break;
                    case IfStatement ifStmt:
                        this.GenerateIfStatement(ifStmt, block);
                        break;
                    case WhileStatement whileStmt:
                        this.GenerateWhileStatement(whileStmt, block);
                        break;
                    case RepeatStatement repeatStmt:
                        this.GenerateRepeatStatement(repeatStmt, block);
                        break;
                    case ProcedureCallStatement callStmt:
                        this.CallProcedure(callStmt, block);
                        break;
                }
            }
        }
    }
}