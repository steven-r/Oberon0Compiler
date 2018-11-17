using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Generator.Msil.Tests
{
    using Oberon0.Compiler;

    public static class MsilTestHelper
    {
        private static string tempPath;

        private static int tempFileCount;

        public static string NlFix(this string data)
        {
            return data.Replace("\r\n", "\n");
        }

        public static bool CompileRunTest(string source, List<string> inputData, out string outputData, Module m)
        {
            outputData = string.Empty;
            if (Oberon0Compiler.Instance.HasError || m == null)
            {
                return false;
            }

            var filename = GetTempFile();
            return CompileCode(source, filename, false, m) && RunCode(filename, inputData, out outputData);
        }

        private static string GetTempFile()
        {
            if (tempPath == null)
            {
                tempPath = Path.GetTempFileName();
                var file = Path.GetFileNameWithoutExtension(tempPath);
                tempPath = Path.Combine(Path.GetTempPath(), file);
                Directory.CreateDirectory(tempPath);
            }

            tempFileCount++;
            return Path.Combine(tempPath, "Test" + tempFileCount.ToString("0000"));
        }

        private static bool CompileCode(string source, string filename, bool dumpOutput = false, Module m = null)
        {
            using (TextWriter w = File.CreateText(filename + ".il"))
            {
                w.Write(source);
            }

            CopyReferencedAssemblies(m);

            var runtimePath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            StringBuilder output = new StringBuilder();
            StringBuilder error = new StringBuilder();
            using (Process ilasm = new Process())
            {
                ilasm.StartInfo.FileName = runtimePath + "ilasm.exe";
                ilasm.StartInfo.Arguments = "\"" + filename + ".il\" /exe /output:\"" + filename + ".exe\" /debug=IMPL";
                ilasm.StartInfo.UseShellExecute = false;
                ilasm.StartInfo.CreateNoWindow = true;
                ilasm.StartInfo.WorkingDirectory = runtimePath;
                ilasm.StartInfo.RedirectStandardOutput = true;
                ilasm.StartInfo.RedirectStandardError = true;
                ilasm.OutputDataReceived += (sender, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                        output.AppendLine(args.Data);
                };
                ilasm.ErrorDataReceived += (sender, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                        error.AppendLine(args.Data);
                };
                if (!ilasm.Start())
                {
                    return false;
                }

                // Start the asynchronous read of the sort output stream.
                ilasm.BeginOutputReadLine();
                ilasm.BeginErrorReadLine();

                // Wait for the process to write output.
                ilasm.WaitForExit();
                if (error.Length > 0)
                {
                    Console.Error.WriteLine("ERROR output:");
                    Console.Error.WriteLine(error.ToString());
                }

                if (ilasm.ExitCode != 0 || dumpOutput)
                {
                    Console.WriteLine("Output");
                    Console.Write(output.ToString());
                }

                if (ilasm.ExitCode != 0)
                    return false; // fail
            }

            return true;
        }

        private static void CopyReferencedAssemblies(Module module)
        {
            foreach (var reference in module.ExternalReferences)
            {
                if (reference.GlobalAssemblyCache)
                {
                    continue; // ignored
                }

                File.Copy(reference.Location, Path.Combine(tempPath, Path.GetFileName(reference.Location)), true);
            }
        }

        private static bool RunCode(string filename, List<string> inputData, out string outputData)
        {
            string runtimePath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            StringBuilder output = new StringBuilder();
            StringBuilder error = new StringBuilder();
            using (Process codeProc = new Process())
            {
                codeProc.StartInfo.FileName = filename + ".exe";
                codeProc.StartInfo.UseShellExecute = false;
                codeProc.StartInfo.CreateNoWindow = true;
                codeProc.StartInfo.WorkingDirectory = runtimePath;
                codeProc.StartInfo.RedirectStandardOutput = true;
                codeProc.StartInfo.RedirectStandardError = true;
                codeProc.StartInfo.RedirectStandardInput = inputData != null && inputData.Count > 0;
                codeProc.OutputDataReceived += (sender, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                        output.AppendLine(args.Data);
                };
                codeProc.ErrorDataReceived += (sender, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                        error.AppendLine(args.Data);
                };
                if (!codeProc.Start())
                {
                    outputData = string.Empty;
                    return false;
                }

                // Start the asynchronous read of the sort output stream.
                codeProc.BeginOutputReadLine();
                codeProc.BeginErrorReadLine();

                if (inputData != null && inputData.Count > 0)
                {
                    // Use a stream writer to synchronously write the sort input.
                    StreamWriter inputWriter = codeProc.StandardInput;
                    foreach (var s in inputData)
                    {
                        inputWriter.WriteLine(s);
                    }
                }

                // Wait for the process to write output.
                codeProc.WaitForExit();
                outputData = output.ToString();
                if (error.Length > 0)
                {
                    Console.Error.WriteLine("ERROR output:");
                    Console.Error.WriteLine(error.ToString());
                }

                if (codeProc.ExitCode != 0)
                {
                    return false; // fail
                }
            }
            return true;
        }

    }
}