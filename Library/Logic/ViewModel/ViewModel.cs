using Library.Data;
using Library.Model;
using Serializing;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.IO;
using GalaSoft.MvvmLight.Command;
using Tracing;
using System.Reflection;

namespace Library.Logic.ViewModel
{
    
    public class ViewModel : INotifyPropertyChanged
    {
        #region Fields

        TracingProvider tracingProvider;
        RepositoryLoader repositoriesLoader;

        private TreeViewItem objectSelected;
        private string _loadedAssembly;
        private TreeViewItem _objectToDisplay;
        private bool isSerializationChecked;
        
        #endregion

        #region Properties

        public ITracing Tracer { get; private set; }
        public ICommand ShowCurrentObject { get; }
        public ICommand ReloadAssemblyCommand { get; }
        public ICommand OpenFileCommand { get; }
        public RelayCommand SaveModel { get; }
        public RelayCommand LoadModel { get; }
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
                SaveModel.RaiseCanExecuteChanged();
                LoadModel.RaiseCanExecuteChanged();
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
        public IMetadata LoadedAssemblyRepresentation { get; private set; }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel() : this(true)
        {
        }
        public ViewModel(bool tracing)
        {
            if (repositoriesLoader == null)
                repositoriesLoader = new RepositoryLoader();
            Tracing = tracing;
            if (Tracing)
            {
                ImportTracer();
            }
            ImportPersister();
            LoadedAssemblyRepresentation = new AssemblyMetadata(System.Reflection.Assembly.GetAssembly(this.GetType()));
            ShowCurrentObject = new RelayCommand(ChangeClassToDisplay, () => ObjectSelected != null);
            ObjectsList = new ObservableCollection<TreeViewItem>() { null };
            ReloadAssemblyCommand = new RelayCommand(ReloadAssembly);
            OpenFileCommand = new RelayCommand(() => OpenFile(OpenFileSourceProvider));
            SaveModel = new RelayCommand(() => Save(SaveFileSourceProvider), () => repositoriesLoader.Repository != null 
                                         && repositoriesLoader.Repository is XmlModelSerializer);
            LoadModel = new RelayCommand(() => Load(OpenFileSourceProvider), () => repositoriesLoader.Repository != null 
                                         && repositoriesLoader.Repository is XmlModelSerializer);
        }

        private void ImportTracer()
        {
            if (tracingProvider == null)
            {
                tracingProvider = new TracingProvider()
                {
                    DirectoryCatalog = new System.ComponentModel.Composition.Hosting.DirectoryCatalog
                    (new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath)
                };
            }

            try
            {
                if (Tracer is null)
                {
                    Tracer = tracingProvider.ProvideTracer();
                }
            }
            catch(Tracing.Exceptions.MEFLoaderException ex)
            {
                // Dialog Box
            }
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

        

        private void ImportPersister() //TODO
        {
            //TODO: PLUGIN for repository
            if(IsSerializationChecked)
            {
                //TEMPORARY SOLUTION, PLEASE CHECK
                List<Type> types = new List<Type>(Enumerable.Repeat(typeof(List<IMetadata>), 1));
                types.AddRange(DataLoadedDictionary.GetKnownMetadata(LoadedAssemblyRepresentation));
                if (repositoriesLoader.Repository == null)
                {
                    typeof(RepositoryLoader)
                        .GetProperty("Repository")
                        .SetValue(
                            repositoriesLoader,
                            new XmlModelSerializer(types, typeof(IMetadata)),
                            null);
                }
            }
            else
            {
                // any other implementation of IPersister
            }
        }

        private void Save(ISourceProvider filePathProvider)
        {
            if (filePathProvider == null)
                throw new System.ArgumentNullException("SourceProvider can't be null.");
            if (filePathProvider.GetAccess())
            {
                try
                {
                    repositoriesLoader.Repository.SourceName = filePathProvider.GetFilePath();
                    IEnumerable<IMetadata> objects = from TreeViewItem item in ObjectsList
                                                     select item.ModelObject;
                    Task.Run(() =>
                    {
                        repositoriesLoader.Repository.Save(objects.ToList());
                        (repositoriesLoader.Repository as XmlModelSerializer).SerializationStream.Close();
                    });
                }
                catch(IOException ex)
                {
                    if(Tracing)
                    {
                        Trace.TraceWarning("Exception caught when trying to open a file for writing (serialization)");
                        Trace.TraceWarning(ex.Message);
                    }
                }
            }
        }

        private void Load(ISourceProvider filePathProvider)
        {
            if (filePathProvider == null)
                throw new System.ArgumentNullException("SourceProvider can't be null.");
            if (filePathProvider.GetAccess())
            {
                try
                {
                    repositoriesLoader.Repository.SourceName = filePathProvider.GetFilePath();
                    Dispatcher.CurrentDispatcher.BeginInvoke((Action)delegate ()
                    {
                        object result = repositoriesLoader.Repository.Load();
                        (repositoriesLoader.Repository as XmlModelSerializer).SerializationStream.Close();
                        if (result is IEnumerable<IMetadata>)
                        {
                            ObjectsList.Clear();
                            IEnumerable<IMetadata> objectsRead = result as IEnumerable<IMetadata>;
                            foreach (IMetadata item in objectsRead)
                            {
                                ObjectsList.Add(new AssemblyItem(item as AssemblyMetadata));
                            }
                            LoadedAssembly = "Model deserialized";
                        }
                    });
                }
                catch(IOException ex)
                {
                    if (Tracing)
                    {
                        Trace.TraceWarning("Exception caught when trying to open a file for reading (deserialization)");
                        Trace.TraceWarning(ex.Message);
                    }
                }
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
