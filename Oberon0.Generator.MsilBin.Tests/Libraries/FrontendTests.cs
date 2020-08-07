#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.IO;
using Oberon0.Msil;
using Xunit;
using Xunit.Abstractions;

namespace Oberon0.Generator.MsilBin.Tests.Libraries
{
    public class FrontendTests
    {
        public FrontendTests(ITestOutputHelper output)
        {
            this._output = output;
        }

        private readonly ITestOutputHelper _output;

        [Fact]
        public void TestEmptyArgsRun()
        {
            var currentOut = Console.Out;
            var currentError = Console.Error;

            using var sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetError(sw);

            try
            {
                Program.Main(new string[0]);
            }
            finally
            {
                Console.SetOut(currentOut);
                Console.SetError(currentError);
            }

            _output.WriteLine(sw.ToString());
            Assert.Contains("Compile an Oberon0 source file.", sw.ToString());
        }

        [Fact]
        public void TestFileNotFound()
        {
            var currentOut = Console.Out;
            var currentError = Console.Error;

            using var sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetError(sw);

            try
            {
                Program.Main(new[] {"--input", "dummy-file.ob0"});
            }
            finally
            {
                Console.SetOut(currentOut);
                Console.SetError(currentError);
            }

            _output.WriteLine(sw.ToString());
            Assert.Contains("File does not exist: dummy-file.ob0", sw.ToString());
        }
    }
}