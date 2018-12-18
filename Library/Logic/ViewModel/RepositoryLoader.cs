using Serializing;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Logic.ViewModel
{
    internal class RepositoryLoader
    {
        [Import(typeof(IPersister))]
        private IPersister persister;

        public IPersister Repository { get => persister; private set => persister = value; }

        private CompositionContainer _container;

        public void LoadRepository()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            _container = new CompositionContainer(catalog);
            try
            {
                this._container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Trace.TraceError("External repositories import failed. " +
                compositionException.Message);
            }
        }
        

    }
}
