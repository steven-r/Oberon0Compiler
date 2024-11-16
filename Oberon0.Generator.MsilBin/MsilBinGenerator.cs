#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Exceptions;
using Oberon0.Compiler.Types;
using Oberon0.Generator.MsilBin.PredefinedFunctions;
using Oberon0.Shared;

namespace Oberon0.Generator.MsilBin
{
    /// <summary>
    ///     Generator for MSIL output using Roslyn
    /// </summary>
    public partial class MsilBinGenerator : ICodeGenerator
    {
        private ClassDeclarationSyntax? _classDeclaration;
        private CompilationUnitSyntax? _compiledCode;
        private int _internalCount;
        private NamespaceDeclarationSyntax? _namespace;

        /// <summary>
        ///     The main class being generated (usually <code>Oberon0.{module name}</code>)
        /// </summary>
        public string MainClassName { get; set; } = null!;

        /// <summary>
        ///     The name space for the main class (to be used for integration and testing)
        /// </summary>
        public string MainClassNamespace { get; set; } = null!;

        /// <summary>
        ///     The used compiler module
        /// </summary>
        public Module Module { get; }

        /// <summary>
        /// Initialize the generator
        /// </summary>
        /// <param name="module">The compiled module to be translated to MSIL</param>
        public MsilBinGenerator(Module module)
        {
            Module = module;
            StandardFunctionRepository.Initialize(module);
        }

        /// <summary>
        ///     Dump generated code as string
        /// </summary>
        /// <returns>a (hopefully) compilable string</returns>
        public string IntermediateCode()
        {
            return _compiledCode!.ToFullString();
        }

        /// <summary>
        ///     Dump generated code to a <see cref="TextWriter" />
        /// </summary>
        /// <param name="writer">The TextWriter to write to</param>
        public void WriteIntermediateCode(TextWriter writer)
        {
            _compiledCode!.WriteTo(writer);
        }

        /// <summary>
        ///     Start code generation
        /// </summary>
        public void GenerateIntermediateCode()
        {
            if (Module == null)
            {
                throw new InternalCompilerException("Please set Module before calling GenerateIntermediateCode()");
            }

            _compiledCode = SyntaxFactory.CompilationUnit();

            MainClassNamespace = "Oberon0." + Module.Name;
            MainClassName = Module.Name + "__Impl";

            // Create a namespace: (namespace CodeGenerationSample)
            _namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(MainClassNamespace))
                                      .NormalizeWhitespace();

            // Add System using statement: (using System)
            _namespace = _namespace.AddUsings(
                SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")),
                SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Linq")),
                SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("AnyClone")));

            GenerateClass();

            _namespace = _namespace.AddMembers(_classDeclaration!);

            _compiledCode = _compiledCode.AddMembers(_namespace).NormalizeWhitespace();
        }

        private void GenerateClass()
        {
            _classDeclaration = SyntaxFactory.ClassDeclaration(MainClassName);

            // Add the public modifier: (public class Order)
            _classDeclaration = _classDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            ProcessMainBlock(Module.Block);
        }

        private MethodDeclarationSyntax GenerateFunctionOrProcedure(FunctionDeclaration functionDeclaration)
        {
            GenerateComplexTypeMappings(functionDeclaration.Block);
            var function = StartFunction(functionDeclaration);

            var statements =
                new SyntaxList<StatementSyntax>(GenerateLocalDefinitions(functionDeclaration));
            statements = GenerateBlockStatements(functionDeclaration.Block, statements);
            function = function.WithBody(
                SyntaxFactory.Block(statements));
            return function;
        }

        private void ProcessMainBlock(Block block)
        {
            if (_classDeclaration == null)
            {
                throw new InvalidOperationException("Please call GenerateClass() before ProcessMainBlock()");
            }

            PatchConstDeclarations();

            _classDeclaration = GenerateRecordDeclarations(_classDeclaration, block);

            GenerateComplexTypeMappings(block);

            // declarations
            foreach (var declaration in block.Declarations)
            {
                var fieldDeclaration = GenerateFieldDeclaration(declaration, false);
                if (fieldDeclaration != null)
                {
                    _classDeclaration = _classDeclaration.AddMembers(fieldDeclaration);
                }
            }

            // add functions/procedures
            foreach (var functionDeclaration in block.Procedures)
            {
                // ignore system and external libraries
                if (functionDeclaration.IsInternal || functionDeclaration is ExternalFunctionDeclaration)
                {
                    continue;
                }

                var function = GenerateFunctionOrProcedure(functionDeclaration);
                _classDeclaration = _classDeclaration.AddMembers(function);
            }

            var mainBlock = new Block(block, Module);
            mainBlock.Statements.AddRange(block.Statements);
            var mainFuncDecl =
                new FunctionDeclaration("__MAIN__" + Module.Name, mainBlock, SimpleTypeDefinition.VoidType);
            _classDeclaration = _classDeclaration.AddMembers(GenerateFunctionOrProcedure(mainFuncDecl));

            _classDeclaration = _classDeclaration.AddMembers(SyntaxFactory.MethodDeclaration(
                                                                               SyntaxFactory.PredefinedType(
                                                                                   SyntaxFactory.Token(SyntaxKind
                                                                                      .VoidKeyword)),
                                                                               MapIdentifier("Main"))
                                                                          .WithModifiers(
                                                                               SyntaxFactory.TokenList(
                                                                                   SyntaxFactory.Token(SyntaxKind
                                                                                      .PublicKeyword),
                                                                                   SyntaxFactory.Token(SyntaxKind
                                                                                      .StaticKeyword)))
                                                                          .WithBody(
                                                                               SyntaxFactory.Block(
                                                                                   SyntaxFactory
                                                                                      .SingletonList<StatementSyntax>(
                                                                                           SyntaxFactory
                                                                                              .ExpressionStatement(
                                                                                                   SyntaxFactory
                                                                                                      .InvocationExpression(
                                                                                                           SyntaxFactory
                                                                                                              .MemberAccessExpression(
                                                                                                                   SyntaxKind
                                                                                                                      .SimpleMemberAccessExpression,
                                                                                                                   SyntaxFactory
                                                                                                                      .ObjectCreationExpression(
                                                                                                                           MapIdentifierName(
                                                                                                                               MainClassName))
                                                                                                                      .WithArgumentList(
                                                                                                                           SyntaxFactory
                                                                                                                              .ArgumentList()),
                                                                                                                   MapIdentifierName(
                                                                                                                       "__MAIN__" +
                                                                                                                       Module
                                                                                                                          .Name))))))));
        }

        /**
         * start a new procedure/function
         */
        private static MethodDeclarationSyntax StartFunction(FunctionDeclaration functionDeclaration)
        {
            var method =
                SyntaxFactory.MethodDeclaration(GetSyntaxType(functionDeclaration.ReturnType), functionDeclaration.Name);

            var list = new SyntaxNodeOrTokenList();
            bool first = true;
            foreach (var declaration in functionDeclaration.Block.Declarations.OfType<ProcedureParameterDeclaration>())
            {
                if (!first)
                {
                    list = list.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                }

                first = false;
                var param = SyntaxFactory.Parameter(MapIdentifier(declaration.Name))
                                         .WithType(AddTypeSyntaxSpecification(declaration));
                if (declaration.IsVar)
                {
                    param = param.WithModifiers(SyntaxFactory.TokenList(
                        SyntaxFactory.Token(SyntaxKind.RefKeyword)));
                }

                list = list.Add(param);
            }

            if (list.Any())
            {
                method = method.WithParameterList(SyntaxFactory.ParameterList(
                    SyntaxFactory.SeparatedList<ParameterSyntax>(list)));
            }

            return method;
        }
    }
}
