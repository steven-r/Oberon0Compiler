#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using Oberon0.Test.Support;
using Xunit;

namespace Oberon0.Compiler.Tests
{
    public class LexerTests
    {
        [Fact]
        public void ModuleMissingId()
        {
            var errors = new List<CompilerError>();
            TestHelper.CompileString(@"MODULÜE XXÄ; BEGIN END XXÄ.", errors);
            Assert.Equal(2, errors.Count);

            Assert.Equal("missing 'MODULE' at 'MODUL'", errors[0].Message);
            Assert.StartsWith("mismatched input ", errors[1].Message);
        }
    }
}
