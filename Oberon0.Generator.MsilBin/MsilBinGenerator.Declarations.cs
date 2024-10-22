#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Types;
using Oberon0.Generator.MsilBin.GeneratorInfo;

namespace Oberon0.Generator.MsilBin
{
    public partial class MsilBinGenerator
    {
        /**
         * Handle call by value parameters based on complex types (arrays, records).
         *
         * @remarks
         *
         * This is done by creating a copy of the original parameter (which comes as a reference) and as a starting point of the method
         * all call by value parameters are copied to their local parts by deep copy.
         */
        private static void GenerateComplexTypeMappings(Block block)
        {
            var addDeclarations = new List<Declaration>();
            foreach (var pp in block.Declarations.OfType<ProcedureParameterDeclaration>())
            {
                if (pp.IsVar || pp.Type.Type.HasFlag(BaseTypes.Simple))
                {
                    continue;
                }

                string name = pp.Name;
                pp.Name = "__param__" + name;
                pp.GeneratorInfo = new DeclarationGeneratorInfo();

                // rename parameter and create a new field
                var field = new Declaration(name, pp.Type, block)
                {
                    GeneratorInfo = new DeclarationGeneratorInfo()
                };
                ((DeclarationGeneratorInfo) field.GeneratorInfo).OriginalField = pp;
                ((DeclarationGeneratorInfo) pp.GeneratorInfo).ReplacedBy = field;
                addDeclarations.Add(field);
            }

            block.Declarations.AddRange(addDeclarations);
        }

        /**
         * Add RECORD elements to a class
         * 
         * @return The updated @see classDeclaration
         */
        private ClassDeclarationSyntax GenerateRecordDeclarations(ClassDeclarationSyntax classDeclaration, Block block)
        {
            foreach (var typeDefinition in block.Types.Where(x => x is RecordTypeDefinition))
            {
                var recordType = (RecordTypeDefinition) typeDefinition;
                classDeclaration = classDeclaration.AddMembers(GenerateRecordType(recordType));
            }

            // add anonymous variables
            foreach (var typeDefinition in block.Declarations.Where(x => x.Type is RecordTypeDefinition))
            {
                var recordType = (RecordTypeDefinition) typeDefinition.Type;
                if (string.IsNullOrEmpty(recordType.Name))
                    // anonymous type...
                    // Name is updated with call to GenerateRecordType
                {
                    classDeclaration = classDeclaration.AddMembers(GenerateRecordType(recordType));
                }
            }

            return classDeclaration;
        }

        /**
 * 
 * 
 * @returns the class declaration containing all added records
 */

        /// <summary>
        /// Generate a class containing a RECORD definitions
        /// </summary>
        /// <param name="recordType">The record type definition to be processed.</param>
        /// <returns>A roslyn intermediate structure of the record definition</returns>
        /// <remarks>
        /// There's a feature in Oberon0 that allows records to be anonymous. Please look at the following code example:
        /// <example>
        /// MODULE test;
        ///   VAR foo: RECORD
        ///     field: INTEGER;
        ///     another: REAL
        ///   END;
        /// END test.
        /// </example>
        /// <para>
        /// This code will create a variable a with a record as the data type. In this case the record does not have a name
        /// as it is (legally) created as part of the variable declaration.
        /// </para>
        /// <para>
        /// To create a structure that is valid for the intermediate code, those fields get a unique name <code>__internal__{id}</code>
        /// where id is an increasing counter.
        /// </para>
        /// </remarks>
        private ClassDeclarationSyntax GenerateRecordType(RecordTypeDefinition recordType)
        {
            if (string.IsNullOrEmpty(recordType.Name))
            {
                recordType.Name = $"__internal__{++_internalCount}";
            }

            var record = SyntaxFactory.ClassDeclaration(recordType.Name)
                                      .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            foreach (var declaration in recordType.Elements)
            {
                var fieldDeclaration = GenerateFieldDeclaration(declaration, true);
                if (fieldDeclaration != null)
                {
                    record = record.AddMembers(fieldDeclaration);
                }
            }

            return record;
        }

        private static VariableDeclaratorSyntax FieldConstDeclaration(ConstDeclaration constDeclaration,
                                                                      VariableDeclaratorSyntax varDeclarator)
        {
            EqualsValueClauseSyntax assignment;
            if (constDeclaration.GeneratorInfo is ConstDeclarationGeneratorInfo { GeneratorFunc: not null } gi)
            {
                assignment = SyntaxFactory.EqualsValueClause(gi.GeneratorFunc(constDeclaration));
            } 
            else
            {
                // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
                assignment = constDeclaration.Type.Type switch
                {
                    BaseTypes.Int => SyntaxFactory.EqualsValueClause(
                        SyntaxFactory.LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            SyntaxFactory.Literal(constDeclaration.Value.ToInt32()))),
                    BaseTypes.Bool => SyntaxFactory.EqualsValueClause(
                        SyntaxFactory.LiteralExpression(
                            constDeclaration.Value.ToBool()
                                ? SyntaxKind.TrueLiteralExpression
                                : SyntaxKind.FalseLiteralExpression)),
                    BaseTypes.Real => SyntaxFactory.EqualsValueClause(
                        SyntaxFactory.LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            SyntaxFactory.Literal(constDeclaration.Value.ToDouble()))),
                    _ => throw new ArgumentException(
                        "Cannot handle type " + Enum.GetName(typeof(BaseTypes), constDeclaration.Type.Type),
                        nameof(constDeclaration))
                };
            }
            return varDeclarator.WithInitializer(assignment);
        }

        private static FieldDeclarationSyntax? GenerateFieldDeclaration(Declaration declaration, bool makePublic)
        {
            var varDeclaration = GenerateVariableDeclaration(declaration);
            if (varDeclaration == null)
            {
                return null;
            }
            var field = SyntaxFactory.FieldDeclaration(varDeclaration);
            if (makePublic || declaration.Exportable)
            {
                field = field.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
            } else
            {
                field = field.AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
            }

            return field;
        }

        private static VariableDeclarationSyntax? GenerateVariableDeclaration(Declaration declaration)
        {
            if (declaration.Type.Type.HasFlag(BaseTypes.Simple))
            {
                return GenerateSimpleVariableDeclaration(declaration);
            }

            // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
            return declaration.Type.Type switch
            {
                BaseTypes.Array   => GenerateArrayVariableDefinition(declaration),
                BaseTypes.Record  => GenerateRecordVariableDeclaration(declaration),
                _                 => throw new InvalidDataException($"Provided type is not known: {declaration.Type.Type:G}")            };
        }

        private static VariableDeclarationSyntax GenerateRecordVariableDeclaration(Declaration declaration)
        {
            var dgi = declaration.GeneratorInfo as DeclarationGeneratorInfo;

            SeparatedSyntaxList<VariableDeclaratorSyntax> varDeclarator;

            if (dgi?.OriginalField == null)
            {
                varDeclarator =
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.VariableDeclarator(
                                          MapIdentifier(declaration.Name))
                                     .WithInitializer(
                                          SyntaxFactory.EqualsValueClause(
                                              SyntaxFactory.ObjectCreationExpression(
                                                                GetSyntaxType(declaration.Type))
                                                           .WithArgumentList(
                                                                SyntaxFactory.ArgumentList()))));
            } else
            {
                varDeclarator = SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.VariableDeclarator(
                                      MapIdentifier(declaration.Name))
                                 .WithInitializer(
                                      SyntaxFactory.EqualsValueClause(
                                          SyntaxFactory.InvocationExpression(
                                              SyntaxFactory.MemberAccessExpression(
                                                  SyntaxKind.SimpleMemberAccessExpression,
                                                  MapIdentifierName(dgi.OriginalField.Name),
                                                  SyntaxFactory.GenericName(
                                                                    MapIdentifier("Clone"))
                                                               .WithTypeArgumentList(
                                                                    SyntaxFactory.TypeArgumentList(
                                                                        SyntaxFactory.SingletonSeparatedList(
                                                                            GetSyntaxType(declaration.Type)))))))));
            }

            var variable = SyntaxFactory.VariableDeclaration(GetSyntaxType(declaration.Type))
                                        .WithVariables(varDeclarator);
            return variable;
        }

        private static VariableDeclarationSyntax? GenerateSimpleVariableDeclaration(Declaration declaration)
        {
            var typeSyntax = GetSyntaxType(declaration.Type);

            var varDeclarator = SyntaxFactory.VariableDeclarator(MapReservedWordName(declaration.Name));
            if (declaration is ConstDeclaration constDeclaration)
            {
                if (constDeclaration.GeneratorInfo is ConstDeclarationGeneratorInfo { DropGeneration: true })
                {
                    return null;
                }
                varDeclarator = FieldConstDeclaration(constDeclaration, varDeclarator);
            }

            var variable = SyntaxFactory.VariableDeclaration(typeSyntax)
                                        .AddVariables(varDeclarator);
            return variable;
        }

        private static VariableDeclarationSyntax GenerateArrayVariableDefinition(Declaration declaration)
        {
            var arrayType = declaration.Type as ArrayTypeDefinition;
            Debug.Assert(arrayType != null, nameof(arrayType) + " != null");
            var dgi = declaration.GeneratorInfo as DeclarationGeneratorInfo;

            var typeSyntax = GetSyntaxType(arrayType.ArrayType);

            VariableDeclaratorSyntax varDeclarator;

            if (dgi?.OriginalField == null)
            {
                varDeclarator = SyntaxFactory.VariableDeclarator(MapReservedWordName(declaration.Name)).WithInitializer(
                    SyntaxFactory.EqualsValueClause(
                        SyntaxFactory.ArrayCreationExpression(SyntaxFactory.ArrayType(
                                                                                GetSimpleTypeSyntax(
                                                                                    arrayType.ArrayType))
                                                                           .WithRankSpecifiers(
                                                                                SyntaxFactory.SingletonList(
                                                                                    SyntaxFactory.ArrayRankSpecifier(
                                                                                        SyntaxFactory
                                                                                           .SingletonSeparatedList<
                                                                                                ExpressionSyntax>(
                                                                                                SyntaxFactory
                                                                                                   .LiteralExpression(
                                                                                                        SyntaxKind
                                                                                                           .NumericLiteralExpression,
                                                                                                        SyntaxFactory
                                                                                                           .Literal(
                                                                                                                arrayType
                                                                                                                   .Size)))))))));
            } else
            {
                varDeclarator = SyntaxFactory.VariableDeclarator(MapReservedWordName(declaration.Name)).WithInitializer(
                    SyntaxFactory.EqualsValueClause(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                MapIdentifierName(dgi.OriginalField.Name),
                                SyntaxFactory.GenericName(
                                                  MapIdentifier("Clone"))
                                             .WithTypeArgumentList(
                                                  SyntaxFactory.TypeArgumentList(
                                                      SyntaxFactory.SingletonSeparatedList<TypeSyntax>(SyntaxFactory
                                                         .ArrayType(
                                                              SyntaxFactory.PredefinedType(
                                                                  SyntaxFactory.Token(SyntaxKind.IntKeyword)))
                                                         .WithRankSpecifiers(
                                                              SyntaxFactory.SingletonList(
                                                                  SyntaxFactory.ArrayRankSpecifier(
                                                                      SyntaxFactory
                                                                         .SingletonSeparatedList<ExpressionSyntax>(
                                                                              SyntaxFactory
                                                                                 .OmittedArraySizeExpression())))))))))));
            }

            var variable = SyntaxFactory.VariableDeclaration(
                                             SyntaxFactory.ArrayType(typeSyntax)
                                                          .WithRankSpecifiers(
                                                               SyntaxFactory.SingletonList(
                                                                   SyntaxFactory.ArrayRankSpecifier(
                                                                       SyntaxFactory
                                                                          .SingletonSeparatedList<ExpressionSyntax>(
                                                                               SyntaxFactory
                                                                                  .OmittedArraySizeExpression())))))
                                        .AddVariables(varDeclarator);
            return variable;
        }

        private static IReadOnlyList<StatementSyntax> GenerateLocalDefinitions(FunctionDeclaration functionDeclaration)
        {
            var statements =
                new SyntaxList<StatementSyntax>();
            // declarations
            foreach (var declaration in functionDeclaration.Block.Declarations)
            {
                switch (declaration)
                {
                    case ProcedureParameterDeclaration:
                    case ConstDeclaration constDeclaration when
                        ((constDeclaration.GeneratorInfo as ConstDeclarationGeneratorInfo)!).DropGeneration:
                        continue;
                    default:
                        var localDeclaration = GenerateVariableDeclaration(declaration);
                        if (localDeclaration != null)
                        {
                            statements =
                                statements.Add(SyntaxFactory.LocalDeclarationStatement(localDeclaration));
                        }
                        break;
                }
            }

            return statements;
        }

        private static TypeSyntax AddTypeSyntaxSpecification(Declaration declaration)
        {
            var type = GetSyntaxType(declaration.Type);
            if (declaration.Type.Type.HasFlag(BaseTypes.Simple) || declaration.Type.Type == BaseTypes.Record)
            {
                return type;
            }

            if (declaration.Type.Type == BaseTypes.Array)
            {
                return SyntaxFactory.ArrayType(type)
                                    .WithRankSpecifiers(SyntaxFactory.SingletonList(
                                         SyntaxFactory.ArrayRankSpecifier(
                                             SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                                 SyntaxFactory.OmittedArraySizeExpression()))));
            }

            // should not happen
            throw new ArgumentException("Unknown type " + Enum.GetName(typeof(BaseTypes), declaration.Type.Type),
                nameof(declaration));
        }

        private void PatchConstDeclarations()
        {
            PatchConstDeclarations(Module.Block);
        }

        private static void PatchConstDeclarations(Block block)
        {
            foreach (var constDeclaration in block.Declarations.OfType<ConstDeclaration>())
            {
                var gi = new ConstDeclarationGeneratorInfo();
                if (block.Parent == null)
                {
                    switch (constDeclaration.Name)
                    {
                        // top level
                        case "EPSILON":
                            gi.GeneratorFunc = _ => SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.DoubleKeyword)), 
                                  MapIdentifierName("Epsilon"));
                            break;
                        case "TRUE" or "FALSE":
                            gi.DropGeneration = true;
                            break;
                    }
                }
                constDeclaration.GeneratorInfo = gi;
            }

            foreach (var blockProcedure in block.Procedures.Where(b => !b.IsInternal))
            {
                PatchConstDeclarations(blockProcedure.Block);
            }
        }
    }
}
