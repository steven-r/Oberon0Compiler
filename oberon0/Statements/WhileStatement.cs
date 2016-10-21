using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Compiler.Statements
{
    /// <summary>
    /// handle WHILE
    /// </summary>
    /// <seealso cref="BasicStatement" />
    public class WhileStatement: BasicStatement
    {
        /// <summary>
        /// Gets or sets the condition.
        /// </summary>
        /// <value>The condition.</value>
        public Expression Condition { get; set; }

        /// <summary>
        /// Gets or sets the block.
        /// </summary>
        /// <value>The block.</value>
        public Block Block { get; set; }

        public WhileStatement(Block parent)
        {
            Block = new Block {Parent = parent};
        }
    }
}
