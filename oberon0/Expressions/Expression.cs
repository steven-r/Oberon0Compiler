﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Generator;
using Oberon0.Compiler.Solver;

namespace Oberon0.Compiler.Expressions
{
    public abstract class Expression
    {
        public TokenType Operator { get; set; }

        public BaseType TargetType { get; set; }

        public IGeneratorInfo GeneratorInfo { get; set; }

        /// <summary>
        /// Gets a value indicating whether this expression is constant.
        /// </summary>
        /// <value><c>true</c> if this instance is constant; otherwise, <c>false</c>.</value>
        public virtual bool IsConst => false;

        /// <summary>
        /// Gets a value indicating whether this instance is unary.
        /// </summary>
        /// <value><c>true</c> if this instance is unary; otherwise, <c>false</c>.</value>
        public virtual bool IsUnary => false;
    }
}
