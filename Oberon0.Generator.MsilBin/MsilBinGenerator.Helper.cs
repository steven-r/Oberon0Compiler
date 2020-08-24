#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Oberon0.Compiler.Types;

namespace Oberon0.Generator.MsilBin
{
    partial class MsilBinGenerator
    {
        private static TypeSyntax GetTypeName(TypeDefinition typeDefinition)
        {
            while (true)
            {
                if (typeDefinition.Type.HasFlag(BaseTypes.Simple))
                {
                    return GetSimpleTypeSyntaxName(typeDefinition);
                }

                if (typeDefinition is ArrayTypeDefinition atd)
                {
                    typeDefinition = atd.ArrayType;
                    continue;
                }

                return SyntaxFactory.ParseTypeName(typeDefinition.Name);
            }
        }

        private static TypeSyntax GetSimpleTypeSyntaxName(TypeDefinition typeDefinition)
        {
            return typeDefinition.Type switch
            {
                BaseTypes.Int    => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword)),
                BaseTypes.Bool   => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword)),
                BaseTypes.Real   => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.DoubleKeyword)),
                BaseTypes.String => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword)),
                BaseTypes.Void   => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                _ => throw new InvalidDataException("provided type is not simple " +
                    Enum.GetName(typeof(BaseTypes), typeDefinition.Type))
            };
        }

        private static SyntaxToken MapIdentifier(string identifier)
        {
            return SyntaxFactory.Identifier(MapReservedWordName(identifier));
        }

        internal static IdentifierNameSyntax MapIdentifierName(string identifier)
        {
            return SyntaxFactory.IdentifierName(MapReservedWordName(identifier));
        }

        // thanks to https://stackoverflow.com/a/38836600
        private static string MapReservedWordName(string identifier)
        {
            bool isAnyKeyword = SyntaxFacts.GetKeywordKind(identifier) != SyntaxKind.None
             || SyntaxFacts.GetContextualKeywordKind(identifier) != SyntaxKind.None;
            return isAnyKeyword ? "@" + identifier : identifier;
        }
    }
}
