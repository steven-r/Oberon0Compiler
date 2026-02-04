#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Diagnostics;
using System.IO;
using LLVMSharp.Interop;
using Oberon0.Compiler.Exceptions;
using Oberon0.Compiler.Generator;
using Oberon0.Generator.LLVM.GeneratorInfo;

namespace Oberon0.Generator.LLVM;

/// <summary>
/// Erstellt eine ausführbare Binärdatei aus LLVM IR
/// </summary>
internal class CreateLLVMBinary
{
    private readonly LLVMCodeGenerator _codeGenerator;
    private readonly LlvmCreateBinaryOptions _options;

    public CreateLLVMBinary(LLVMCodeGenerator codeGenerator, LlvmCreateBinaryOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(codeGenerator);

        _codeGenerator = codeGenerator;
        _options = SetOptions(options ?? new LlvmCreateBinaryOptions());

        if (!Directory.Exists(_options.OutputPath))
        {
            Directory.CreateDirectory(_options.OutputPath!);
        }
    }

    private LlvmCreateBinaryOptions SetOptions(LlvmCreateBinaryOptions options)
    {
        options.ModuleName ??= _codeGenerator.Module.Name
            ?? throw new InternalCompilerException("Name needs to be set");
        options.OutputPath ??= Environment.CurrentDirectory;
        return options;
    }

    public bool Execute()
    {
        try
        {
            // LLVM IR in Datei schreiben
            var irFilePath = Path.Combine(_options.OutputPath!, $"{_options.ModuleName}.ll");
            File.WriteAllText(irFilePath, _codeGenerator.IntermediateCode());

            LogOutput($"LLVM IR written to: {irFilePath}");

            // Optional: Objektdatei erstellen
            var objFilePath = Path.Combine(_options.OutputPath!, $"{_options.ModuleName}.o");
            if (!CompileLLVMToObject(irFilePath, objFilePath))
            {
                return false;
            }

            // Optional: Ausführbare Datei linken
            if (!_codeGenerator.Module.HasExports)
            {
                var exeFilePath = Path.Combine(_options.OutputPath!, _options.ModuleName!);
                return LinkObjectToExecutable(objFilePath, exeFilePath);
            }

            return true;
        }
        catch (Exception ex)
        {
            LogError($"Error during binary creation: {ex.Message}");
            return false;
        }
    }

    private bool CompileLLVMToObject(string irFilePath, string objFilePath)
    {
        // llc aufrufen, um IR zu Objektdatei zu kompilieren
        var arguments = $"-filetype=obj -o \"{objFilePath}\" \"{irFilePath}\"";
        return ExecuteProcess("llc", arguments);
    }

    private bool LinkObjectToExecutable(string objFilePath, string exeFilePath)
    {
        // clang verwenden zum Linken
        var arguments = $"-o \"{exeFilePath}\" \"{objFilePath}\"";
        return ExecuteProcess("clang", arguments);
    }

    private bool ExecuteProcess(string command, string arguments)
    {
        try
        {
            var procStartInfo = new ProcessStartInfo(command, arguments)
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = _options.OutputPath!
            };

            using var process = Process.Start(procStartInfo);
            if (process == null)
            {
                LogError($"Failed to start process: {command}");
                return false;
            }

            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (!string.IsNullOrWhiteSpace(output))
            {
                LogOutput(output);
            }

            if (!string.IsNullOrWhiteSpace(error))
            {
                LogError(error);
            }

            return process.ExitCode == 0;
        }
        catch (Exception ex)
        {
            LogError($"Error executing {command}: {ex.Message}");
            return false;
        }
    }

    private void LogOutput(string message)
    {
        _options.OutputDataRetrieved?.Invoke(this,
            new ProcessOutputReceivedEventArgs(_options, message));
    }

    private void LogError(string message)
    {
        _options.ErrorDataRetrieved?.Invoke(this,
            new ProcessOutputReceivedEventArgs(_options, message));
    }
}