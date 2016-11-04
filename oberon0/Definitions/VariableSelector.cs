using System.Collections.Generic;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Compiler.Definitions
{
    /// <summary>
    /// hold selector lists
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{BaseSelectorElement}" />
    public class VariableSelector: List<BaseSelectorElement>
    {}

    public abstract class BaseSelectorElement {}

    public class IdentifierSelector : BaseSelectorElement
    {
        public IdentifierSelector(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    public class IndexSelector : BaseSelectorElement
    {
        public IndexSelector(Expression index)
        {
            IndexDefinition = index;
        }

        public Expression IndexDefinition { get; }
    }
}
