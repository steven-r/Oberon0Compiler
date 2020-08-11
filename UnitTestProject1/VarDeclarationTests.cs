#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using Xunit;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Types;
using Oberon0.TestSupport;

namespace Oberon0.Compiler.Tests
{
    public class VarDeclarationTests
    {
        [Fact]
        public void ArrayFailBooleanIndex()
        {
            TestHelper.CompileString(
                @"MODULE Test; 

VAR
  id: ARRAY TRUE OF INTEGER;
 END Test.",
                "The array size must return a constant integer expression");
        }

        [Fact]
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
            Assert.IsType<ArrayTypeDefinition>(id.Type);

            ArrayTypeDefinition atd = (ArrayTypeDefinition)id.Type;

            Assert.Equal(5, atd.Size);
            Assert.IsType<ArrayTypeDefinition>(atd.ArrayType);

            ArrayTypeDefinition atd1 = (ArrayTypeDefinition)atd.ArrayType;
            Assert.Equal(10, atd1.Size);
            Assert.Equal(intType, atd1.ArrayType);
        }

        [Fact]
        public void OneVar()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  id: INTEGER;
 END Test.");

            Assert.NotNull(m.Block.LookupVar("id"));
            Assert.Equal(BaseTypes.Int, m.Block.LookupVar("id").Type.Type);
        }

        [Fact]
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
            Assert.IsType<ArrayTypeDefinition>(id.Type);

            ArrayTypeDefinition atd = (ArrayTypeDefinition)id.Type;

            Assert.Equal(5, atd.Size);
            Assert.Equal(intType, atd.ArrayType);
        }

        [Fact]
        public void TwoVars1()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  id: INTEGER;
  id1: INTEGER;
 END Test.");

            var id = m.Block.LookupVar("id");
            Assert.NotNull(id);
            Assert.Equal(BaseTypes.Int, id.Type.Type);
            var id1 = m.Block.LookupVar("id1");
            Assert.NotNull(id1);
            Assert.Equal(BaseTypes.Int, id1.Type.Type);
        }

        [Fact]
        public void TwoVars2()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  id, id1: INTEGER;
 END Test.");

            var id = m.Block.LookupVar("id");
            var id1 = m.Block.LookupVar("id1");
            Assert.NotNull(id);
            Assert.Equal(BaseTypes.Int, id.Type.Type);
            Assert.NotNull(id1);
            Assert.Equal(BaseTypes.Int, id1.Type.Type);
        }

        [Fact]
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

        [Fact]
        public void TwoVarsFail2()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
VAR
  id, id: INTEGER;
 END Test.",
                "Variable declared twice");
        }

        [Fact]
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

        [Fact]
        public void VarWithoutId()
        {
            List<CompilerError> errors = new List<CompilerError>();
            TestHelper.CompileString(
                @"MODULE Test; 
VAR
 END Test.",
                errors);
            Assert.Equal(3, errors.Count);
        }

        [Fact]
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