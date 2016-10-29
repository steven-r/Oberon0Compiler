﻿using Oberon0.Compiler.Generator;

namespace Oberon0.Compiler.Statements
{
    public abstract class BasicStatement
    {
        /// <summary>
        /// Additional information used by the generator engine
        /// </summary>
        /// <value>Generator information.</value>
        public IGeneratorInfo GeneratorInfo { get; set; }
    }
}
