#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LexerTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler.Tests/LexerTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Tests
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using Oberon0.TestSupport;

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