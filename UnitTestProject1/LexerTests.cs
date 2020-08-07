#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using NUnit.Framework;
using Oberon0.TestSupport;

namespace Oberon0.Compiler.Tests
{
    [TestFixture]
    public class LexerTests
    {
        [Test]
        public void ModuleMissingId()
        {
            var errors = new List<CompilerError>();
            TestHelper.CompileString(@"MODULÜE XXÄ; BEGIN END XXÄ.", errors);
            Assert.AreEqual(2, errors.Count);

            Assert.AreEqual("missing 'MODULE' at 'MODUL'", errors[0].Message);
            Assert.That(errors[1].Message.StartsWith("mismatched input "));
        }
    }
}