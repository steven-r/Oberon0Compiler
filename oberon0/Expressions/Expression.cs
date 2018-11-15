using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions
{
    using Antlr4.Runtime;

    public abstract class Expression
    {
        protected Expression()
        {
        }

        protected Expression(TypeDefinition targetType)
        {
            TargetType = targetType;
        }

#pragma warning disable CS3003 // Type is not CLS-compliant
        public IToken Token { get; set; }
#pragma warning restore CS3003 // Type is not CLS-compliant

        public int Operator { get; set; }

        public TypeDefinition TargetType { get; set; }

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
