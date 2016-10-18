using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Oberon0.Compiler.Solver;

namespace Oberon0.Compiler.Expressions.Arithmetic
{
    class ArithmeticRepository
    {

#pragma warning disable 649
        [ImportMany(typeof(ICalculatable))]
        public List<ICalculatable> ClassRepository;
#pragma warning restore 649

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly CompositionContainer _container;

        public ArithmeticRepository()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(ICalculatable).Assembly));

            _container = new CompositionContainer(catalog,
                                      CompositionOptions.DisableSilentRejection |
                                      CompositionOptions.IsThreadSafe);
            _container.ComposeParts(this);
        }
    }
}
