using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Definitions;

/// <summary>
/// Represents a variable declaration in an Oberon0 program.
/// </summary>
public class VariableDeclaration : Declaration
{
    /// <summary>
    /// Represents a declaration in an Oberon0 program, such as variables, constants, types, or procedures.
    /// </summary>
    /// <param name="name">The name of the declared entity.</param>
    /// <param name="type">The type definition of the declared entity.</param>
    /// <param name="block">The block in which the declaration is defined, or <c>null</c> if not associated with a specific block.</param>
    public VariableDeclaration(string name, TypeDefinition type, Block? block) : base(name, type, block)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Declaration" /> class without an associated block.
    /// </summary>
    /// <param name="name">The name of the declared entity.</param>
    /// <param name="type">The type definition of the declared entity.</param>
    public VariableDeclaration(string name, TypeDefinition type) : base(name, type)
    {
    }
}