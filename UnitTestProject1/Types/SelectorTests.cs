#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Statements;
using Oberon0.Compiler.Types;
using Oberon0.Test.Support;
using Xunit;

namespace Oberon0.Compiler.Tests.Types
{
    public class SelectorTests
    {
        [Fact]
        public void SelectorArrayAccess()
        {
            TestHelper.CompileString(
                """
                MODULE test; 
                TYPE 
                    aType= ARRAY 5 OF INTEGER;
                VAR 
                  a : aType;
                                
                BEGIN
                    a[1] := 1;
                END test.
                """);
        }

        [Fact]
        public void SelectorFailSimpleTypeArrayAccess()
        {
            TestHelper.CompileString(
                """
                MODULE test; 
                TYPE 
                    aType= ARRAY 5 OF INTEGER;
                VAR 
                  a : INTEGER;
                                
                BEGIN
                    a[1] := 1;
                END test.
                """, "Simple variables or constants do not allow any selector");
        }

        [Fact]
        public void SelectorNestedArrayRecord()
        {
            var m = TestHelper.CompileString(
                """

                MODULE test; 
                TYPE 
                    rType = RECORD
                       field1: INTEGER;
                       field2: REAL
                    END;
                    aType= ARRAY 5 OF rType;
                VAR 
                  a : aType;
                                
                BEGIN
                    a[1].field1 := 1;
                    a[2].field2 := 1.23;
                END test.
                """);
            Assert.Equal(2, m.Block.Statements.Count);
            var assignment = Assert.IsType<AssignmentStatement>(m.Block.Statements[0]);
            Assert.NotNull(assignment.Selector);
            Assert.Equal(2, assignment.Selector.Count);
            Assert.IsType<ArrayTypeDefinition>(assignment.Variable.Type);
            var index = Assert.IsType<IndexSelector>(assignment.Selector[0]);
            var intExpression = Assert.IsType<ConstantIntExpression>(index.IndexDefinition);
            Assert.Equal(1, intExpression.ToInt32());
            var record = Assert.IsType<IdentifierSelector>(assignment.Selector[1]);
            Assert.Equal("field1", record.Name);
            assignment = Assert.IsType<AssignmentStatement>(m.Block.Statements[1]);
            Assert.NotNull(assignment.Selector);
            index = Assert.IsType<IndexSelector>(assignment.Selector[0]);
            intExpression = Assert.IsType<ConstantIntExpression>(index.IndexDefinition);
            Assert.Equal(2, intExpression.ToInt32());
            record = Assert.IsType<IdentifierSelector>(assignment.Selector[1]);
            Assert.Equal("field2", record.Name);
        }

        [Fact]
        public void SelectorFailNestedArrayRecordWrongElement()
        {
            TestHelper.CompileString(
                """

                MODULE test; 
                TYPE 
                    rType = RECORD
                       field1: INTEGER;
                       field2: REAL
                    END;
                    aType= ARRAY 5 OF rType;
                VAR 
                  a : aType;
                  b: INTEGER;                
                BEGIN
                    a[12].b := 1.23;
                END test.
                """, "Array index out of bounds", "Record reference expected", "Left & right side do not match types");
        }
    }
}
