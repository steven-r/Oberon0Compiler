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
#endregion

namespace Oberon0.Msil
{
    using System;
    using System.IO;

    using CommandLine;

    using Oberon0.Compiler;
    using Oberon0.Compiler.Definitions;

    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
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

            Module m = Oberon0Compiler.CompileString(File.ReadAllText(options.SourceFile));
            if (Oberon0Compiler.Instance.HasError)
            {
                Environment.Exit(1);
            }
        }

        private class Options
        {
            [Value(0, Required = true, HelpText = "Source file to compile")]
            public string SourceFile { get; set; }
        }
    }
}