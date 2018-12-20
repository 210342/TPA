using Library.Data;
using Library.Model;
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
using Persistance;

namespace Library.Logic.ViewModel
{
    
    public class ViewModel : INotifyPropertyChanged
    {
        #region Fields

        private TreeViewItem objectSelected;
        private string _loadedAssembly;
        private TreeViewItem _objectToDisplay;
        private bool isSerializationChecked;
        
        #endregion

        #region Properties

        public ITracing Tracer { get; private set; }
        public IPersister Persister { get; private set; }
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
            SaveModel = new RelayCommand(() => Save(SaveFileSourceProvider), () => Persister != null && Persister is ISerializer);
            LoadModel = new RelayCommand(() => Load(OpenFileSourceProvider), () => Persister != null && Persister is ISerializer);
        }

        private void ImportTracer()
        {
            TracingProvider tracingProvider = new TracingProvider()
            {
                DirectoryCatalog = new System.ComponentModel.Composition.Hosting.DirectoryCatalog
                (new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath)
            };

            try
            {
                if (Tracer is null)
                {
                    Tracer = tracingProvider.ProvideTracer();
                }
            }
            catch(Tracing.Exceptions.MEFTracingLoaderException)
            {
                // Dialog Box
                Tracing = false;
            }
        }

        private void ImportPersister() //TODO
        {
            PersistanceProvider persistanceProvider = new PersistanceProvider()
            {
                DirectoryCatalog = new System.ComponentModel.Composition.Hosting.DirectoryCatalog
                (new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath)
            };

            try
            {
                if (Persister is null)
                {
                    Persister = persistanceProvider.ProvidePersister();
                }
            }
            catch (Persistance.Exceptions.MEFPersistanceLoaderException)
            {
                // Dialog Box
            }
            if(Persister is ISerializer)
            {
                List<Type> types = new List<Type>(Enumerable.Repeat(typeof(List<IMetadata>), 1));
                types.AddRange(DataLoadedDictionary.GetKnownMetadata(LoadedAssemblyRepresentation));
                ISerializer serializer = Persister as ISerializer;
                serializer.KnownTypes = types;
                serializer.NodeType = typeof(IMetadata);
                serializer.InitialiseSerialization();
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

        private void Save(ISourceProvider filePathProvider)
        {
            if(Persister is ISerializer)
            {
                if (filePathProvider == null)
                    throw new System.ArgumentNullException("SourceProvider can't be null.");
                if (filePathProvider.GetAccess())
                {
                    ISerializer serializer = Persister as ISerializer;
                    try
                    {
                        serializer.SourceName = filePathProvider.GetFilePath();
                        IEnumerable<IMetadata> objects = from TreeViewItem item in ObjectsList
                                                         select item.ModelObject;
                        Task.Run(() =>
                        {
                            serializer.Save(objects.ToList());
                            serializer.SerializationStream.Close();
                        });
                    }
                    catch (IOException ex)
                    {
                        if (Tracing)
                        {
                            Tracer.LogFailure($"Exception caught when trying to open a file for writing (serialization) {Environment.NewLine}{ex.Message}");
                        }
                    }
                }
            }
            else
            {
                // unsupported yet
            }
        }

        private void Load(ISourceProvider filePathProvider)
        {
            if(Persister is ISerializer)
            {
                if (filePathProvider == null)
                    throw new System.ArgumentNullException("SourceProvider can't be null.");
                if (filePathProvider.GetAccess())
                {
                    ISerializer serializer = Persister as ISerializer;
                    try
                    {
                        serializer.SourceName = filePathProvider.GetFilePath();
                        Dispatcher.CurrentDispatcher.BeginInvoke((Action)delegate ()
                        {
                            object result = serializer.Load();
                            serializer.SerializationStream.Close();
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
                    catch (IOException ex)
                    {
                        if (Tracing)
                        {
                            Tracer.LogFailure($"Exception caught when trying to open a file for reading (deserialization){Environment.NewLine}{ex.Message}");
                        }
                    }
                }
            }
            else
            {
                // not supported yet;
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
