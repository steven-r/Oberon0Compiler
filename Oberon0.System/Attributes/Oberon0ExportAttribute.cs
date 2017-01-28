using System;
using JetBrains.Annotations;

namespace Oberon0.Attributes
{
    /// <summary>
    /// Declare a library export for the Oberon0 language
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method)]
    [UsedImplicitly]
    public class Oberon0ExportAttribute : Attribute
    {
        public string Name { get; }
        public string ReturnType { get; }
        public string[] Parameters { get; }

        public Oberon0ExportAttribute([NotNull] string name, [NotNull] string returnType, params string[] parameters)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (returnType == null) throw new ArgumentNullException(nameof(returnType));
            Name = name;
            ReturnType = returnType;
            Parameters = parameters;
        }
    }
}