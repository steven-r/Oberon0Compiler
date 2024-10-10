#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Build.Locator;
using Oberon0.Shared;

namespace Oberon0.Generator.MsilBin
{
    /// <summary>
    /// Use dotnet to compile and create a final binary
    /// </summary>
    internal class CreateBinary
    {
        private readonly ICodeGenerator _codeGenerator;
        private readonly CreateBinaryOptions _options;

        public CreateBinary(ICodeGenerator codeGenerator, CreateBinaryOptions? options = null)
        {
            ArgumentNullException.ThrowIfNull(codeGenerator);

            MSBuildLocator.RegisterDefaults();
            _codeGenerator = codeGenerator ?? throw new ArgumentNullException(nameof(codeGenerator));
            _options = SetOptions(options ?? new CreateBinaryOptions());
            if (!Directory.Exists(_options.OutputPath))
            {
                throw new ArgumentException("Output path does not exist", nameof(options));
            }
        }

        private static string GetDotnetExe ()
        {
            string execName = "dotnet";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                execName += ".exe";
            }
            return execName;
        }

        private CreateBinaryOptions SetOptions(CreateBinaryOptions options)
        {
            options.ModuleName ??= _codeGenerator.Module.Name;
            options.SolutionPath ??= BuildOutputPath(options);
            options.OutputPath ??= Environment.CurrentDirectory;
            return options;
        }

        private static string BuildOutputPath(CreateBinaryOptions options)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Oberon0", "MSIL",
                GetFileHashPath(Directory.GetCurrentDirectory()), options.ModuleName!);
        }

        public bool Execute()
        {
            var directoryInfo = new DirectoryInfo(_options.SolutionPath!);
            if (_options.CleanSolution || !directoryInfo.Exists)
            {
                bool res = BuildNewSolution();
                if (!res)
                {
                    return false;
                }
            }

            return DropFileAndCompile() && PublishFileToCurrentFolder();
        }

        private bool PublishFileToCurrentFolder()
        {
            return ExecuteProcess(GetDotnetExe(), $"publish --output \"{_options.OutputPath}\"");
        }

        private bool DropFileAndCompile()
        {
            string code = _codeGenerator.IntermediateCode();
            File.WriteAllText(Path.Combine(_options.SolutionPath!, "Program.cs"), code, Encoding.UTF8);
            return ExecuteProcess(GetDotnetExe(), "build --force");
        }

        private static string GetFileHashPath(string path)
        {
            using var sha256Hash = SHA256.Create();
            string hash = GetHash(sha256Hash, path);
            return Path.Combine(hash[..2], hash);
        }

        // from https://docs.microsoft.com/de-de/dotnet/api/system.security.cryptography.hashalgorithm.computehash?view=netcore-3.1
        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new string builder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            foreach (byte t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        private bool BuildNewSolution()
        {
            var directoryInfo = new DirectoryInfo(_options.SolutionPath!);
            if (_options.CleanSolution && directoryInfo.Exists)
            {
#pragma warning disable S1215 // "GC.Collect" should not be called
                GC.Collect(); // try to unload blocking resources
#pragma warning restore S1215 // "GC.Collect" should not be called
                Directory.Delete(_options.SolutionPath!, true);
            }

            if (File.Exists(Path.Combine(_options.SolutionPath!, "Program.cs")))
            {
                throw new InvalidDataException("Target directory is not empty");
            }

            Directory.CreateDirectory(_options.SolutionPath!);

            // add new project
            string[] parameters =
            [
                "new",
                // ReSharper disable once StringLiteralTypo
                _codeGenerator.Module.HasExports ? "classlib" : "console",
                "--framework", _options.FrameworkVersion
            ];

            string execName = GetDotnetExe();
            return ExecuteProcess(execName, string.Join(' ', parameters)) &&
                // add AnyClone
                ExecuteProcess(execName, "add package AnyClone");
        }

        private bool ExecuteProcess(string command, string parameters)
        {
            var procStartInfo = new ProcessStartInfo(command, parameters)
            {
                RedirectStandardError = true, RedirectStandardOutput = true, ErrorDialog = false,
                UseShellExecute = false,
                WorkingDirectory = _options.SolutionPath!
            };

            var process = new Process() {StartInfo = procStartInfo};
            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.Data))
                {
                    _options.OutputDataRetrieved?.Invoke(sender, new ProcessOutputReceivedEventArgs(_options, e.Data));
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.Data))
                {
                    _options.ErrorDataRetrieved?.Invoke(sender, new ProcessOutputReceivedEventArgs(_options, e.Data));
                }
            };
            if (!process.Start())
            {
                return false;
            }

            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            process.WaitForExit();
            return process.ExitCode == 0;
        }
    }
}
