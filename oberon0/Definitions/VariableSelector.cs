using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using JetBrains.Annotations;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Definitions
{
    /// <summary>
    /// hold selector lists
    /// </summary>
    public class VariableSelector: List<BaseSelectorElement>
    {
        public TypeDefinition SelectorResultType { get; set; }
    }

    public abstract class BaseSelectorElement
    {
#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public IToken Token { get; }
#pragma warning restore CS3001 // Argument type is not CLS-compliant

#pragma warning disable CS3001 // Argument type is not CLS-compliant
        protected BaseSelectorElement([NotNull] IToken tokenStart)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            if (tokenStart == null) throw new ArgumentNullException(nameof(tokenStart));
            this.Token = tokenStart;
        }
    }

    public class IdentifierSelector : BaseSelectorElement
    {
#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public IdentifierSelector(string name, [NotNull] IToken tokenStart)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
            : base(tokenStart)
        {
            Name = name;
        }

        public string Name { get; }
    }

    public class IndexSelector : BaseSelectorElement
    {
#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public IndexSelector(Expression index, [NotNull] IToken tokenStart) : base(tokenStart)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            IndexDefinition = index;
        }

        public Expression IndexDefinition { get; set; }
    }
}
