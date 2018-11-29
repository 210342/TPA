using Library.Data;
using Library.Data.Model;
using Library.Logic.TreeView;
using Library.Logic.TreeView.Items;
using Serializing;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Library.Logic.ViewModel
{
    
    public class ViewModel : INotifyPropertyChanged
    {
        #region Fields
        [ImportMany(typeof(TraceListener))]
        private System.Collections.Generic.IEnumerable<TraceListener> 
            importedTraceListener = null;
        [Import(typeof(IPersister))]
        private IPersister persister;

        private CompositionContainer _container;
        private TreeViewItem objectSelected;
        private string _loadedAssembly;
        private TreeViewItem _objectToDisplay;
        private bool isSerializationChecked;
        
        #endregion
        #region Properties
        public ICommand ShowCurrentObject { get; }
        public ICommand ReloadAssemblyCommand { get; }
        public ICommand OpenFileCommand { get; }
        public ICommand SaveModel { get; }
        public ICommand LoadModel { get; }
        public ISourceProvider OpenFileSourceProvider { get; set; }
        public ISourceProvider SaveFileSourceProvider { get; set; }
        public bool Tracing { get; set; }
        public ObservableCollection<TreeViewItem> ObjectsList { get; }
        public bool IsSerializationChecked
        {
            get
            {
                return isSerializationChecked;
            }
            set
            {
                isSerializationChecked = value;
                ImportPersister();
                OnPropertyChanged();
                if(Tracing)
                {
                    Trace.TraceInformation("Repository changed.");
                    Trace.Flush();
                }
            }
        }
        public TreeViewItem ObjectSelected
        {
            get
            {
                return objectSelected;
            }
            set
            {
                PreviousSelection = objectSelected;
                objectSelected = value;
                if (Tracing)
                {
                    Trace.TraceInformation("ObjectSelected changed.");
                    Trace.Flush();
                }
                OnPropertyChanged();
            }
        }
        public TreeViewItem ObjectToDisplay
        {
            get
            {
                return _objectToDisplay;
            }
            set
            {
                this._objectToDisplay = value;
                OnPropertyChanged();
                if(Tracing)
                {
                    Trace.TraceInformation("ObjectToDisplay changed.");
                    Trace.Flush();
                }
                
            }
        }
        public TreeViewItem PreviousSelection { get; private set; }

        public string LoadedAssembly
        {
            get
            {
                return _loadedAssembly;
            }
            set
            {
                _loadedAssembly = value;
                OnPropertyChanged();
            }
        }
        public IMetadata LoadedAssemblyRepresentation { get; private set; } = new AssemblyMetadata();
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel() : this(false)
        {
        }
        public ViewModel(bool tracing)
        {
            Tracing = tracing;
            if (Tracing)
            {
                ImportTraceListener();
            }
            ImportPersister();
            LoadedAssemblyRepresentation = new AssemblyMetadata(System.Reflection.Assembly.GetAssembly(this.GetType()));
            ShowCurrentObject = new RelayCommand(ChangeClassToDisplay, () => ObjectSelected != null);
            ObjectsList = new ObservableCollection<TreeViewItem>() { null };
            ReloadAssemblyCommand = new RelayCommand(ReloadAssembly);
            OpenFileCommand = new RelayCommand(() => OpenFile(OpenFileSourceProvider));
            SaveModel = new RelayCommand(() => Save(SaveFileSourceProvider), () => true);
            LoadModel = new RelayCommand(() => Load(OpenFileSourceProvider), () => true);
        }

        private void LoadAssembly()
        {
            if (Tracing)
            {
                Trace.TraceInformation("Assembly loading.");
                Trace.Flush();
            }
            Reflector reflector = new Reflector(LoadedAssembly);
            LoadedAssemblyRepresentation = reflector.m_AssemblyModel;
        }

        private void ReloadAssembly()
        {
            LoadAssembly();
            ObjectsList.Clear();
            if (LoadedAssemblyRepresentation != null)
            {
                TreeViewItem item = new AssemblyItem((AssemblyMetadata)LoadedAssemblyRepresentation);
                ObjectsList.Add(item);
                ObjectSelected = item;
            }
            if (Tracing)
            {
                Trace.TraceInformation("Assembly reloaded.");
                Trace.Flush();
            }
        }

        public void ChangeClassToDisplay()
        {
            ObjectToDisplay = ObjectSelected;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ImportTraceListener()
        {
            var catalog = new AggregateCatalog();
            System.Reflection.Assembly loadedTracingAssembly =
            System.Reflection.Assembly.UnsafeLoadFrom(@".\Tracing.dll");

            catalog.Catalogs.Add(new AssemblyCatalog(loadedTracingAssembly));
            _container = new CompositionContainer(catalog);
            try
            {
                this._container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Trace.TraceError("External TraceListener import failed. " +
                compositionException.Message);
            }
            if(importedTraceListener != null)
                foreach(TraceListener listener in importedTraceListener)
                    Trace.Listeners.Add(listener);
        }

        private void ImportPersister() //TODO
        {
            if(IsSerializationChecked)
            {
                List<Type> types = new List<Type>();
                types.AddRange(DataLoadedDictionary.GetKnownMetadata(LoadedAssemblyRepresentation));
                types.Add(typeof(List<IMetadata>));
                persister = new XmlModelSerializer(types, typeof(Dictionary<int, IMetadata>));
            }
            else
            {

            }
        }

        private void Save(ISourceProvider filePathProvider)
        {
            if (filePathProvider == null)
                throw new System.ArgumentNullException("SourceProvider can't be null.");
            if (filePathProvider.GetAccess())
            {
                persister.SourceName = filePathProvider.GetFilePath();
                IEnumerable<IMetadata> objects = from TreeViewItem item in ObjectsList
                                                 select item.ModelObject;
                Task.Run(() => persister.Save(objects.ToList()));
            }
        }

        private void Load(ISourceProvider filePathProvider)
        {
            if (filePathProvider == null)
                throw new System.ArgumentNullException("SourceProvider can't be null.");
            if (filePathProvider.GetAccess())
            {
                persister.SourceName = filePathProvider.GetFilePath();
                Dispatcher.CurrentDispatcher.BeginInvoke((Action)delegate()
                {
                    object result = persister.Load();
                    if (result is IEnumerable<IMetadata>)
                    {
                        ObjectsList.Clear();
                        IEnumerable<IMetadata> objectsRead = result as IEnumerable<IMetadata>;
                        foreach (IMetadata item in objectsRead)
                        {
                            ObjectsList.Add(new AssemblyItem(item as AssemblyMetadata));
                        }
                    }
                });
            }
        }

        private void OpenFile(ISourceProvider sourceProvider)
        {
            if (sourceProvider == null)
                throw new System.ArgumentNullException("SourceProvider can't be null.");
            if (sourceProvider.GetAccess())
            {
                LoadedAssembly = sourceProvider.GetFilePath();
                ReloadAssemblyCommand.Execute(null);
            }
        }
    }
}
