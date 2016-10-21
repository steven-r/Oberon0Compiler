using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Compiler.Definitions
{
    /// <summary>
    /// hold selector lists
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{BaseSelectorElement}" />
    public class VariableSelector: List<BaseSelectorElement>
    { }

    public abstract class BaseSelectorElement { }

    public class IdentifierSelector : BaseSelectorElement
    {
        public string Name { get; set; }

        public IdentifierSelector(string name)
        {
            Name = name;
        }
    }

    public class IndexSelector : BaseSelectorElement
    {
        public Expression IndexDefinition { get; set; }

        public IndexSelector(Expression index)
        {
            IndexDefinition = index;
        }
    }
}
