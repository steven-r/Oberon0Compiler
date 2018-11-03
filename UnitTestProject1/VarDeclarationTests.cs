#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VarDeclarationTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler.Tests/VarDeclarationTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Tests
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Types;
    using Oberon0.TestSupport;

    [TestFixture]
    public class VarDeclarationTests
    {
        [Test]
        public void ArrayFailBooleanIndex()
        {
            TestHelper.CompileString(
                @"MODULE Test; 

VAR
  id: ARRAY TRUE OF INTEGER;
 END Test.",
                "The array size must return a constant integer expression");
        }

        [Test]
        public void ArrayOfArray()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  id: ARRAY 5 OF ARRAY 10 OF INTEGER;
 END Test.");

            var id = m.Block.LookupVar("id");
            var intType = m.Block.LookupType("INTEGER");
            Assert.NotNull(id);
            Assert.IsInstanceOf<ArrayTypeDefinition>(id.Type);

            ArrayTypeDefinition atd = (ArrayTypeDefinition)id.Type;

            Assert.AreEqual(5, atd.Size);
            Assert.IsInstanceOf<ArrayTypeDefinition>(atd.ArrayType);

            ArrayTypeDefinition atd1 = (ArrayTypeDefinition)atd.ArrayType;
            Assert.AreEqual(10, atd1.Size);
            Assert.AreEqual(intType, atd1.ArrayType);
        }

        [Test]
        public void OneVar()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  id: INTEGER;
 END Test.");

            Assert.AreNotEqual(null, m.Block.LookupVar("id"));
            Assert.AreEqual(BaseTypes.Int, m.Block.LookupVar("id").Type.Type);
        }

        [Test]
        public void SimpleArray()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  id: ARRAY 5 OF INTEGER;
 END Test.");

            var id = m.Block.LookupVar("id");
            var intType = m.Block.LookupType("INTEGER");
            Assert.NotNull(id);
            Assert.IsInstanceOf<ArrayTypeDefinition>(id.Type);

            ArrayTypeDefinition atd = (ArrayTypeDefinition)id.Type;

            Assert.AreEqual(5, atd.Size);
            Assert.AreEqual(intType, atd.ArrayType);
        }

        [Test]
        public void TwoVars1()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  id: INTEGER;
  id1: INTEGER;
 END Test.");

            var id = m.Block.LookupVar("id");
            Assert.AreNotEqual(null, id);
            Assert.AreEqual(BaseTypes.Int, id.Type.Type);
            var id1 = m.Block.LookupVar("id1");
            Assert.AreNotEqual(null, id1);
            Assert.AreEqual(BaseTypes.Int, id1.Type.Type);
        }

        [Test]
        public void TwoVars2()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  id, id1: INTEGER;
 END Test.");

            var id = m.Block.LookupVar("id");
            var id1 = m.Block.LookupVar("id1");
            Assert.AreNotEqual(null, id);
            Assert.AreEqual(BaseTypes.Int, id.Type.Type);
            Assert.AreNotEqual(null, id1);
            Assert.AreEqual(BaseTypes.Int, id1.Type.Type);
        }

        [Test]
        public void TwoVarsFail1()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
VAR
  id: INTEGER;
  id: INTEGER;
 END Test.",
                "Variable declared twice");
        }

        [Test]
        public void TwoVarsFail2()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
VAR
  id, id: INTEGER;
 END Test.",
                "Variable declared twice");
        }

        [Test]
        public void VarArrayNotConst()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
VAR
    id: INTEGER;
    arr: ARRAY id OF INTEGER;

 END Test.",
                "The array size must return a constant integer expression");
        }

        [Test]
        public void VarWithoutId()
        {
            List<CompilerError> errors = new List<CompilerError>();
            TestHelper.CompileString(
                @"MODULE Test; 
VAR
 END Test.",
                errors);
            Assert.AreEqual(3, errors.Count);
        }

        [Test]
        public void VarWithoutWrongType()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
VAR
    id: DUMMY;

 END Test.",
                "Type not known");
        }
    }
}