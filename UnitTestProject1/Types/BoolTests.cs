using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oberon0.Compiler.Tests.Types
{
    using NUnit.Framework;

    using Oberon0.Compiler.Expressions.Constant;
    using Oberon0.Compiler.Statements;
    using Oberon0.TestSupport;

    [TestFixture]
    public class BoolTests
    {
        [Test]
        public void TestAssignConst()
        {
            var m = TestHelper.CompileString(
                @"MODULE TestAssignment;
VAR r: BOOLEAN;

BEGIN
    r := TRUE;
END TestAssignment.
");
            Assert.NotNull(m);
            Assert.AreEqual(1, m.Block.Statements.Count);
            AssignmentStatement statement = m.Block.Statements[0] as AssignmentStatement;
            Assert.NotNull(statement);
            Assert.IsInstanceOf<ConstantBoolExpression>(statement.Expression);
            Assert.AreEqual($"r:BOOLEAN := {true}", statement.ToString());
        }

        /// <summary>
        /// Cheat logic to cover not used functions required by interfaces / base classes
        /// </summary>
        [Test]
        public void TestAssignCheat()
        {
            var m = TestHelper.CompileString(
                @"MODULE TestAssignment;
VAR r: BOOLEAN;

BEGIN
    r := TRUE;
END TestAssignment.
");
            Assert.NotNull(m);
            Assert.AreEqual(1, m.Block.Statements.Count);
            AssignmentStatement statement = m.Block.Statements[0] as AssignmentStatement;
            Assert.NotNull(statement);
            Assert.IsInstanceOf<ConstantBoolExpression>(statement.Expression);
            var boolExpression = statement.Expression as ConstantBoolExpression;
            Assert.NotNull(boolExpression);
            Assert.AreEqual($"r:BOOLEAN := {true}", statement.ToString());
            Assert.Throws<NotImplementedException>(() => boolExpression.ToDouble());
            Assert.Throws<NotImplementedException>(() => boolExpression.ToInt32());
        }
    }
}
