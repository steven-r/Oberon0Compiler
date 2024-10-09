#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Oberon0.Msil;
using Xunit;
using Xunit.Abstractions;

namespace Oberon0.Generator.MsilBin.Tests.Libraries
{
    public class FrontendTests
    {
        private readonly ITestOutputHelper _output;

        public FrontendTests(ITestOutputHelper output)
        {
            _output = output;
        }

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
                Program.Main(new[] {"dummy-file.ob0"});
            }
            finally
            {
                Console.SetOut(currentOut);
                Console.SetError(currentError);
            }

            _output.WriteLine(sw.ToString());
            Assert.StartsWith("File does not exist: 'dummy-file.ob0'.", sw.ToString());
        }

        [Fact]
        public void TestCompileAndViewOutput()
        {
            const string code = @"
MODULE TestCompileAndViewOutput;
VAR
  x, y, z: INTEGER;

    PROCEDURE Multiply(x: INTEGER; y: INTEGER; VAR z: INTEGER);
    VAR
      negate : BOOLEAN;
    BEGIN 
        negate := x < 0;
        IF (negate) THEN x := -x END;
        z := 0;
        WHILE x > 0 DO
            IF x MOD 2 = 1 THEN 
                z := z + y 
            END;
            y := 2*y; 
            x := x DIV 2
        END ;
        IF (negate) THEN z := -z END;
    END Multiply;

BEGIN 
 x := 10;
 y := 15;
 Multiply(x, y, z);
 WriteInt(z);
 WriteLn
END TestCompileAndViewOutput.
";
            var currentOut = Console.Out;
            var currentError = Console.Error;
            string dirName = Path.GetTempFileName().Replace('.', '-');
            string sourceFileName = Path.Combine(dirName, "Multiply.ob0");

            Directory.CreateDirectory(dirName);

            File.WriteAllText(sourceFileName, code);

            using var sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetError(sw);

            try
            {
                int res = Program.Main(new[] { sourceFileName, "--verbose", "--clean", "--project-name", "Multiply" });
                if (res == 1)
                {
                    _output.WriteLine(sw.ToString());
                }

                string execName = Path.Combine(dirName, "Multiply");
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    execName += ".exe";
                }
                CheckProcessOutput(execName, null, "150");
                Assert.Equal(0, res);
            }
            finally
            {
                Console.SetOut(currentOut);
                Console.SetError(currentError);
                GC.Collect();
                Directory.Delete(dirName, true);
            }
        }

        [Fact]
        public void FrontendTestCompileWithError()
        {
            const string code = @"
MODULE TestCompileAndViewOutput some error here;
END TestCompileAndViewOutput.
";
            var currentOut = Console.Out;
            var currentError = Console.Error;
            string dirName = Path.GetTempFileName().Replace('.', '-');
            string sourceFileName = Path.Combine(dirName, "Multiply.ob0");

            Directory.CreateDirectory(dirName);


            File.WriteAllText(sourceFileName, code);

            using var sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetError(sw);

            try
            {
                int res = Program.Main(new[] { sourceFileName, "--verbose", "--clean", "--project-name", "Multiply" });
                if (res == 1)
                {
                    _output.WriteLine(sw.ToString());
                }

                Assert.Equal(1, res);
            }
            finally
            {
                Console.SetOut(currentOut);
                Console.SetError(currentError);
                GC.Collect();
                Directory.Delete(dirName, true);
            }
        }

        private static void CheckProcessOutput(string executableName, IReadOnlyCollection<string> inputStrings, params string[] expectedOutput)
        {
            var processStart = new ProcessStartInfo(executableName)
            {
                RedirectStandardError = true,
                RedirectStandardInput = inputStrings != null && inputStrings.Count > 0,
                RedirectStandardOutput = true,
                ErrorDialog = false,
                UseShellExecute = false,
            };
            int outputIndex = 0;
            var p = new Process {StartInfo = processStart};
            p.OutputDataReceived += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(e.Data))
                {
                    return;
                }

                if (outputIndex < expectedOutput.Length)
                {
                    Assert.Equal(expectedOutput[outputIndex], e.Data);
                }

                outputIndex++;
            };
            p.ErrorDataReceived += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(e.Data))
                {
                    return;
                }

                if (outputIndex < expectedOutput.Length)
                {
                    Assert.Equal(expectedOutput[outputIndex], e.Data);
                }

                outputIndex++;
            };
            if (p.Start())
            {
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                if (inputStrings != null)
                {
                    foreach (string s in inputStrings)
                    {
                        p.StandardInput.WriteLine(s);
                    }
                }

                p.WaitForExit();
                Assert.Equal(0, p.ExitCode);
                Assert.Equal(expectedOutput.Length, outputIndex);
            } else
            {
                Assert.Fail("Process could not start");
            }
        }
    }
}
