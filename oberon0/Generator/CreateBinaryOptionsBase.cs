using System;

namespace Oberon0.Compiler.Generator;

public abstract class CreateBinaryOptionsBase
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
    /// Gets or sets the information if the existing information on the binary generation should be deleted.
    /// </summary>
    public bool CleanSolution { get; set; }

    /// <summary>
    /// Gets or sets the solution path that will be used instead of <c>%LOCALAPPDATA%\Oberon0\MSIL\{hash}\{hash}\{ModuleName}</c>
    /// </summary>
    public string? SolutionPath { get; set; }

    /// <summary>
    /// Gets or sets the name of the executable. If not set the module name of the compilation unit is used.
    /// </summary>
    public string? ModuleName { get; set; }

    /// <summary>
    /// Gets or sets the output path where the application will be stored at.
    /// </summary>
    public string? OutputPath { get; set; }


    /// <summary>
    /// Have verbose output
    /// </summary>
    public bool Verbose { get; set; }
}
