#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler.Tests/ArrayTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Tests.Types
{
    using NUnit.Framework;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Types;
    using Oberon0.TestSupport;

    [TestFixture]
    public class ArrayTests
    {
        [Test]
        public void ArrayType()
        {
            Module m = TestHelper.CompileString(
                @"
MODULE test; 
TYPE 
    aType= ARRAY 5 OF INTEGER;
VAR 
  a : aType;
                
END test.");
            var type = m.Block.LookupType("aType");
            Assert.NotNull(type);
            var arrayType = type as ArrayTypeDefinition;
            Assert.NotNull(arrayType);
            Assert.AreEqual("ARRAY 5 OF INTEGER", arrayType.ToString());
        }
    }
}