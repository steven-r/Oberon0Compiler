#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Compiler.Statements
{
    public class AssignmentStatement : IStatement
    {
        public required Declaration Variable { get; init; }

        public required Expression Expression { get; init; }

        public VariableSelector? Selector { get; set; }

        public override string ToString()
        {
            return $"{Variable} := {Expression}";
        }
    }
}
