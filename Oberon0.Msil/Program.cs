#region copyright

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Msil/Program.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#endregion copyright

namespace Oberon0.Msil
{
    using CommandLine;
    using ILRepacking;
    using JetBrains.Annotations;
    using Oberon0.Compiler;
    using Oberon0.Generator.Msil;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using Module = Compiler.Definitions.Module;

    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        private static string tempPath;

        private static string fileName;

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(StartCompile);
        }

        private static void StartCompile(Options options)
        {
            if (!File.Exists(options.SourceFile))
            {
                Console.Error.Write("Cannot find {0}", options.SourceFile);
            }

            fileName = Path.GetFileNameWithoutExtension(options.SourceFile);
            var targetPath = Path.GetDirectoryName(Path.GetFullPath(options.SourceFile));

            Module m = Oberon0Compiler.CompileString(File.ReadAllText(options.SourceFile));
            if (Oberon0Compiler.Instance.HasError)
            {
                Environment.Exit(1);
            }

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            var code = cg.DumpCode();

            if (!CompileCode(code, fileName, targetPath, m, options.Verbose))
            {
                Environment.Exit(2);
            }
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

            return Path.Combine(tempPath, fileName);
        }

        private static bool CompileCode(string source, string filename, string targetPath, Module m, bool dumpOutput = false)
        {
            var tempFile = GetTempFile();
            using (TextWriter w = File.CreateText(tempFile + ".il"))
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
                ilasm.StartInfo.Arguments =
                    $"\"{Path.Combine(tempPath, filename)}.il\" /exe /output:\"{Path.Combine(tempPath, fileName)}.exe\" /debug=IMPL";
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
                    Console.Write(output.ToString());
                }

                if (ilasm.ExitCode != 0)
                    return false; // fail
            }

            MergeLibraries(tempPath, filename, targetPath, dumpOutput);
            return true;
        }

        private static void MergeLibraries(string path, string filename, string targetPath, bool logOutput)
        {
            var repack = new ILRepack(
                new RepackOptions(
                    new CommandLine(
                        new[]
                            {
                        $"/out:{Path.Combine(targetPath, filename + ".exe")}", Path.Combine(path, filename + ".exe"),
                            Path.Combine(path, "Oberon0.System.dll")
                        })),
                new MyLogger(logOutput));
            repack.Repack();
        }

        private static void CopyReferencedAssemblies(Module module)
        {
            var assembly = Assembly.GetEntryAssembly();
            using (Stream resFilestream = assembly.GetManifestResourceStream("Oberon0.Msil.Oberon0.System.dll"))
            {
                if (resFilestream == null) throw new InvalidOperationException("Cannot read embedded resource");
                byte[] ba = new byte[resFilestream.Length];
                resFilestream.Read(ba, 0, ba.Length);
                File.WriteAllBytes(
                    Path.Combine(tempPath, "Oberon0.System.dll"),
                    ba);
            }
            foreach (var reference in module.ExternalReferences)
            {
                if (reference.GlobalAssemblyCache)
                {
                    continue; // ignored
                }

                if (reference.GetName().Name != "Oberon0.System")
                {
                    File.Copy(reference.Location, Path.Combine(tempPath, Path.GetFileName(reference.Location)), true);
                }
            }
        }

        [UsedImplicitly]
        private class Options
        {
            [Value(0, Required = true, HelpText = "Source file to compile")]
            public string SourceFile { get; [UsedImplicitly] set; }

            [Option('v', "verbose", HelpText = "If set, verbose output is generated")]
            public bool Verbose { get; [UsedImplicitly] set; }
        }

        private class MyLogger : ILogger
        {
            private readonly bool logEnabled;

            public MyLogger(bool logEnabled)
            {
                this.logEnabled = logEnabled;
            }

            public void Log(object str)
            {
                if (this.logEnabled)
                {
                    Console.WriteLine(str);
                }
            }

            public void Error(string msg)
            {
                Log("ERR:" + msg);
            }

            public void Warn(string msg)
            {
                Log("WARN:" + msg);
            }

            public void Info(string msg)
            {
                Log("INFO:" + msg);
            }

            public void Verbose(string msg)
            {
                if (ShouldLogVerbose)
                {
                    Log("VERBOSE:" + msg);
                }
            }

            public void DuplicateIgnored(string ignoredType, object ignoredObject)
            {
                // ignored
            }

            public bool ShouldLogVerbose { get; set; }
        }
    }
}