using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Generator;
using Oberon0.Compiler.Statements;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Definitions
{
    public class Block
    {
        public Block()
        {
            InitLists();
        }

        public Block(Block parent)
        {
            InitLists();
            Parent = parent;
        }

        private void InitLists()
        {
            Declarations = new List<Declaration>();
            Types = new List<TypeDefinition>();
            Statements = new List<BasicStatement>();
            Procedures = new List<FunctionDeclaration>();
        }

        public List<Declaration> Declarations { get; private set; }

        public List<TypeDefinition> Types { get; private set; }

        public List<BasicStatement> Statements { get; private set; }

        public List<FunctionDeclaration> Procedures { get; private set; }

        public Block Parent { get; set; }

        /// <summary>
        /// Additional information used by the generator engine
        /// </summary>
        /// <value>Generator information.</value>
        [UsedImplicitly]
        public IGeneratorInfo GeneratorInfo { get; set; }



        public TypeDefinition LookupType(string name)
        {
            Block b = this;
            if (name.StartsWith("&", StringComparison.InvariantCulture))
                name = name.Substring(1);
            while (b != null)
            {
                var res = b.Types.FirstOrDefault(x => x.Name == name);
                if (res != null)
                    return res;
                b = b.Parent;
            }
            return null;
        }

        /// <summary>
        /// Lookups a variable.
        /// </summary>
        /// <param name="name">The variable name.</param>
        /// <returns>Declaration.</returns>
        internal Declaration LookupVar(string name)
        {
            return LookupVar(name, true);
        }

        /// <summary>
        /// Lookups a variable.
        /// </summary>
        /// <param name="name">The variable name.</param>
        /// <param name="lookupParents"></param>
        /// <returns>Declaration.</returns>
        internal Declaration LookupVar(string name, bool lookupParents)
        {
            Block b = this;
            while (b != null)
            {
                var res = b.Declarations.FirstOrDefault(x => x.Name == name);
                if (res != null)
                    return res;
                if (!lookupParents)
                {
                    return null; // nothing in local env
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
                    return res;
                b = b.Parent;
            }
            return null;
        }

        /// <summary>
        /// Lookups the type based on <see cref="BaseTypes"/>.
        /// </summary>
        /// <param name="baseTypes">The base type.</param>
        /// <returns>TypeDefinition.</returns>
        public TypeDefinition LookupTypeByBaseType(BaseTypes baseTypes)
        {
            Block b = this;
            // internal types are only available on level 0
            while (b.Parent != null)
            {
                b = b.Parent;
            }
            var res = b.Types.FirstOrDefault(x => x.BaseTypes == baseTypes && x.IsInternal);
            return res;
        }

        internal FunctionDeclaration LookupFunction(string procedureName, IList<Expression> parameters)
        {
            Block b = this;
            while (b != null)
            {
                var functionDeclaration = CheckForFunction(procedureName, parameters, b);
                if (functionDeclaration != null)
                {
                    return functionDeclaration;
                }
                b = b.Parent;
            }
            return null;
        }

        private static FunctionDeclaration CheckForFunction(string procedureName, IList<Expression> parameters, Block b)
        {
            var res = b.Procedures.Where(x => x.Name == procedureName);
            foreach (FunctionDeclaration func in res)
            {
                var paramList = func.Block.GetParameters();
                if (paramList.Count != parameters.Count) continue;
                bool found = true;
                for (int i = 0; i < parameters.Count; i++)
                {
                    if (paramList[i].Type.BaseTypes != parameters[i].TargetType.BaseTypes)
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                    return func;
            }
            return null;
        }

        private IList<ProcedureParameter> GetParameters()
        {
            return Declarations.OfType<ProcedureParameter>().ToList();
        }
    }
}