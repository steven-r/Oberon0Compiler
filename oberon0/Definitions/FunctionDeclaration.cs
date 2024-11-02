#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Definitions
{
    [DebuggerDisplay("{ReturnType} {Name}")]
    public class FunctionDeclaration
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FunctionDeclaration" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="block">The block.</param>
        /// <param name="returnType">Type of the return.</param>
        /// <param name="parameters">The parameters.</param>
        public FunctionDeclaration(
            string name,
            Block block,
            TypeDefinition returnType,
            params ProcedureParameterDeclaration[]? parameters)
        {
            Name = name;
            Block = block;
            if (parameters is { Length: > 0 })
            {
                Block.Declarations.AddRange(parameters);
            }

            ReturnType = returnType;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FunctionDeclaration" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="block">The block.</param>
        /// <param name="returnType">Type of the return.</param>
        /// <param name="parameters">The parameters.</param>
        internal FunctionDeclaration(
            string name,
            TypeDefinition returnType,
            Block block,
            params string[]? parameters)
        {
            Name = name;
            Block = block;
            int i = 0;
            if (parameters != null)
            {
                foreach (string parameter in parameters)
                {
                    Block.Declarations.Add(
                        Module.GetProcedureParameterByName("__param__" + i++, parameter, Block));
                }
            }

            ReturnType = returnType;
        }

        public Block Block { get; }

        /// <summary>
        ///     Gets a value indicating whether this instance is internal (e.g. <c>WriteInt()</c>
        /// </summary>
        /// <value><c>true</c> if this instance is internal; otherwise, <c>false</c>.</value>
        public bool IsInternal { get; private init; }

        /// <summary>
        ///     Gets or sets a value indicating whether this procedure can be exported.
        /// </summary>
        public bool Exportable { get; set; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets the return type.
        /// </summary>
        public TypeDefinition ReturnType { get; }

        public static FunctionDeclaration AddHardwiredFunction(
            string name,
            Module module,
            TypeDefinition returnType,
            params string[] parameters)
        {
            var res = new FunctionDeclaration(name, returnType, new Block(module.Block, module), parameters)
            {
                IsInternal = true
            };
            return res;
        }

        /// <summary>
        ///     Builds a prototype definition for a function.
        /// </summary>
        /// <example>
        ///     An example prototype might look like:
        ///     <code>$$Void WriteInt(INTEGER)</code> or <code>$$VOID ReadInt(&amp;INTEGER);</code>
        /// </example>
        /// <param name="name">The function/procedure</param>
        /// <param name="returnType">the return type</param>
        /// <param name="parameters">A list of <see cref="ProcedureParameterDeclaration" /> entries.</param>
        /// <returns>A valid prototype description</returns>
        public static string BuildPrototype(
            string name,
            TypeDefinition returnType,
            ProcedureParameterDeclaration[]? parameters)
        {
            var sb = new StringBuilder();
            if (returnType.Name != TypeDefinition.VoidTypeName)
            {
                sb.Append(returnType.Name);
                sb.Append(' ');
            }

            sb.Append(name);
            if (parameters != null)
            {
                sb.Append('(');
                var list = new List<string>(parameters.Length);
                foreach (var parameter in parameters)
                {
                    string parameterName = parameter.IsVar ? "&" : string.Empty;
                    if (!string.IsNullOrWhiteSpace(parameter.Type.Name))
                    {
                        parameterName += parameter.Type.Name;
                    } else if (parameter.Type is ArrayTypeDefinition array)
                    {
                        parameterName += $"{array.ArrayType}[{array.Size}]";
                    } else if (parameter.Type is RecordTypeDefinition)
                    {
                        parameterName += "RECORD {anonymous} END";
                    }

                    list.Add(parameterName);
                }

                sb.Append(string.Join(",", list));
                sb.Append(')');
            }

            return sb.ToString();
        }

        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return $"{(IsInternal ? "internal " : string.Empty)}{GeneratePrototype(this)}";
        }

        public static string GeneratePrototype(FunctionDeclaration fd)
        {
            var @params =
                fd.Block.Declarations.OfType<ProcedureParameterDeclaration>()
                  .Select(x => x.Type.Name);
            return $"{fd.ReturnType.Name} {fd.Name}(" + string.Join(", ", @params) + ")";
        }
    }
}
