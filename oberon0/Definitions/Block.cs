using System;
using System.Collections.Generic;
using System.Linq;
using Oberon0.Compiler.Generator;
using Oberon0.Compiler.Statements;

namespace Oberon0.Compiler.Definitions
{
    public class Block
    {
        public List<Declaration> Declarations { get; }

        public List<TypeDefinition> Types { get; }

        public List<BasicStatement> Statements { get; }

        public List<FunctionDeclaration> Procedures { get; }

        public Block Parent { get; set; }

        /// <summary>
        /// Additional information used by the generator engine
        /// </summary>
        /// <value>Generator information.</value>
        public IGeneratorInfo GeneratorInfo { get; set; }

        public Block()
        {
            Declarations = new List<Declaration>();
            Types = new List<TypeDefinition>();
            Statements = new List<BasicStatement>();
            Procedures = new List<FunctionDeclaration>();
        }

        public TypeDefinition LookupType(string name)
        {
            // ReSharper disable once IntroduceOptionalParameters.Global
            return LookupType(name, false);
        }

        public TypeDefinition LookupType(string name, bool allowInternal)
        {
            Block b = this;
            if (name.StartsWith("&", StringComparison.InvariantCulture))
            {
                // ignore reference marker
                name = name.Substring(1);
            }
            while (b != null)
            {
                var res = b.Types.FirstOrDefault(x => x.Name == name && (allowInternal || !x.IsInternal));
                if (res != null)
                {
                    return res;
                }
                b = b.Parent;
            }
            return null;
        }

        /// <summary>
        /// Lookups a variable.
        /// </summary>
        /// <param name="name">The variable name.</param>
        /// <returns>Declaration.</returns>
        public Declaration LookupVar(string name)
        {
            Block b = this;
            while (b != null)
            {
                var res = b.Declarations.FirstOrDefault(x => x.Name == name);
                if (res != null)
                {
                    return res;
                }
                b = b.Parent;
            }
            return null;
        }

        /// <summary>
        /// Lookup a procedure definition
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <returns>FunctionDeclaration.</returns>
        public FunctionDeclaration LookupFunction(string procedureName)
        {
            Block b = this;
            while (b != null)
            {
                var res = b.Procedures.FirstOrDefault(x => x.Name == procedureName);
                if (res != null)
                {
                    return res;
                }
                b = b.Parent;
            }
            return null;
        }
    }
}