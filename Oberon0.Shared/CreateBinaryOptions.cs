#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Oberon0.Shared
{
    /// <summary>
    /// Options for binary file generation
    /// </summary>
    [UsedImplicitly, ExcludeFromCodeCoverage]
    public class CreateBinaryOptions
    {
        /// <summary>
        /// IF set, retrieves all error output from the processes
        /// </summary>
        public Action<object, ProcessOutputReceivedEventArgs>? ErrorDataRetrieved { get; set; }

        // events

        /// <summary>
        /// If handled, retrieve any output from the process(es)
        /// </summary>
        public Action<object, ProcessOutputReceivedEventArgs>? OutputDataRetrieved { get; set; }

        /// <summary>
        /// Gets ot sets the information if the existing information on the binary generation should be deleted.
        /// </summary>
        public bool CleanSolution { get; set; }

        /// <summary>
        /// Gets or sets the solution path that will be used instead of <code>%LOCALAPPDATA%\Oberon0\MSIL\{hash}\{hash}\{ModuleName}</code>
        /// </summary>
        public string? SolutionPath {get; set;}

        /// <summary>
        /// Gets or sets the name of the executable. If not set the module name of the compilation unit is used.
        /// </summary>
        public string? ModuleName { get; set; }

        /// <summary>
        /// The framework to be used
        /// </summary>
        public string FrameworkVersion { get; [UsedImplicitly] set; } = "net8.0";

        /// <summary>
        /// Gets or sets the output path where the application will be stored at. 
        /// </summary>
        public string? OutputPath { get; set; }


        /// <summary>
        /// Have verbose output
        /// </summary>
        public bool Verbose { get; set; }
    }
}
