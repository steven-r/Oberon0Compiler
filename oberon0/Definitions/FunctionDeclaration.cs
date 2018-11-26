#region copyright

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FunctionDeclaration.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/FunctionDeclaration.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#endregion

namespace Oberon0.Compiler.Definitions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Oberon0.Compiler.Types;

    public class FunctionDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionDeclaration"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="block">The block.</param>
        /// <param name="returnType">Type of the return.</param>
        /// <param name="parameters">The parameters.</param>
        public FunctionDeclaration(
            string name,
            Block block,
            TypeDefinition returnType,
            params ProcedureParameterDeclaration[] parameters)
        {
            this.Name = name;
            this.Block = block;
            if (parameters != null && parameters.Length > 0)
            {
                this.Block.Declarations.AddRange(parameters);
            }

            this.ReturnType = returnType;
        }

        public Block Block { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is internal (e.g. <c>WriteInt()</c>
        /// </summary>
        /// <value><c>true</c> if this instance is internal; otherwise, <c>false</c>.</value>
        public bool IsInternal { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this procedure can be exported.
        /// </summary>
        public bool Exportable { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the return type.
        /// </summary>
        public TypeDefinition ReturnType { get; }

        public static FunctionDeclaration AddHardwiredFunction(
            string name,
            Module module,
            params ProcedureParameterDeclaration[] parameters)
        {
            var res = new FunctionDeclaration(name, new Block(module.Block), SimpleTypeDefinition.VoidType, parameters)
                {
                    IsInternal = true
                };
            return res;
        }

        public static FunctionDeclaration AddHardwiredFunction(
            string name,
            Module module,
            TypeDefinition returnType,
            params ProcedureParameterDeclaration[] parameters)
        {
            var res = new FunctionDeclaration(name, new Block(module.Block), returnType, parameters)
                {
                    IsInternal = true
                };
            return res;
        }

        /// <summary>
        /// Builds a prototype definition for a function. 
        /// </summary>
        /// <example>
        /// An example prototype might look like:
        /// <code>$$Void WriteInt(INTEGER)</code> or <code>$$VOID ReadInt(&amp;INTEGER);</code>
        /// </example>
        /// <param name="name">The function/procedure</param>
        /// <param name="returnType">the return type</param>
        /// <param name="parameters">A list of <see cref="ProcedureParameterDeclaration"/> entries.</param>
        /// <returns>A valid prototype description</returns>
        public static string BuildPrototype(
            string name,
            TypeDefinition returnType,
            ProcedureParameterDeclaration[] parameters)
        {
            StringBuilder sb = new StringBuilder();
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
                    }
                    else if (parameter.Type is ArrayTypeDefinition array)
                    {
                        parameterName += $"{array.ArrayType}[{array.Size}]";
                    }
                    else if (parameter.Type is RecordTypeDefinition)
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

        public override string ToString()
        {
            var @params = string.Join(
                ", ",
                this.Block.Declarations.OfType<ProcedureParameterDeclaration>().Select(x => x.Type.Type.ToString("G")));
            return $"{(this.IsInternal ? "internal " : string.Empty)}{this.ReturnType:G} {this.Name}(" + @params + ")";
        }
    }
}