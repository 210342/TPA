﻿using Library.Data;
using Library.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.IO;
using GalaSoft.MvvmLight.Command;
using Tracing;
using System.Reflection;
using Persistance;
using Library.Logic.MEFProviders;
using Library.Logic.MEFProviders.Exceptions;
using ModelContract;

namespace Library.Logic.ViewModel
{
    
    public class ViewModel : INotifyPropertyChanged
    {
        #region Fields

        private TreeViewItem objectSelected;
        private string _loadedAssembly;
        private TreeViewItem _objectToDisplay;
        
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
            SaveModel = new RelayCommand(() => Save(SaveFileSourceProvider), () => Persister != null && !(ObjectsList.First() is null));
            LoadModel = new RelayCommand(() => Load(OpenFileSourceProvider), () => Persister != null);
        }

        private void ImportTracer()
        {
            TracingProvider tracingProvider = 
                new TracingProvider(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            try
            {
                if (Tracer is null)
                {
                    Tracer = tracingProvider.ProvideTracer();
                }
            }
            catch(MEFLoaderException ex)
            {
                // Dialog Box
                Tracing = false;
                throw ex;
            }
        }

        private void ImportPersister()
        {
            
            PersistanceProvider persistanceProvider = 
                new PersistanceProvider(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            try
            {
                if (Persister is null)
                {
                    Persister = persistanceProvider.ProvidePersister();
                }
            }
            catch (MEFLoaderException ex)
            {
                // Dialog Box
                throw ex;
            }
        }

        private void LoadAssembly()
        {
            if (Tracing)
            {
                Tracer.LogLoadingModel(LoadedAssembly);
                Tracer.Flush();
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
                SaveModel.RaiseCanExecuteChanged();
                ObjectSelected = item;
            }
            if (Tracing)
            {
                Tracer.LogModelLoaded(LoadedAssembly);
                Tracer.Flush();
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
                        if(!serializer.IsInitialised)
                        {
                            List<Type> types = new List<Type>(Enumerable.Repeat(typeof(IMetadata), 1));
                            IEnumerable<Type> model = from type in typeof(SerializationModel.SerializationAssemblyMetadata).Assembly.GetTypes()
                                                      where typeof(IMetadata).IsAssignableFrom(type) && !type.IsInterface
                                                      select type;
                            types.AddRange(model);
                            serializer.KnownTypes = types;
                            serializer.NodeType = typeof(IMetadata);
                            serializer.InitialiseSerialization();
                        }
                        SaveModel.RaiseCanExecuteChanged();
                        serializer.SourceName = filePathProvider.GetFilePath();
                        Task.Run(() =>
                        {
                            IAssemblyMetadata graph = new ModelMapper().Map(
                                root: ObjectsList.First().ModelObject as IAssemblyMetadata, 
                                model: typeof(SerializationModel.SerializationAssemblyMetadata).Assembly
                                );
                            serializer.Save(graph);
                            serializer.SerializationStream.Close();
                        });
                    }
                    catch (IOException ex)
                    {
                        if (Tracing)
                        {
                            Tracer.LogFailure($"Exception caught when trying to open a file for writing (serialization) {Environment.NewLine}{ex.Message}");
                            Tracer.Flush();
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
                        if (!serializer.IsInitialised)
                        {
                            List<Type> types = new List<Type>(Enumerable.Repeat(typeof(List<IMetadata>), 1));
                            types.AddRange(DataLoadedDictionary.GetKnownMetadata(LoadedAssemblyRepresentation));
                            serializer.KnownTypes = types;
                            serializer.NodeType = typeof(IMetadata);
                            serializer.InitialiseSerialization();
                        }
                        serializer.SourceName = filePathProvider.GetFilePath();
                        Dispatcher.CurrentDispatcher.BeginInvoke((Action)delegate ()
                        {
                            object result = serializer.Load();

                            serializer.SerializationStream.Close();
                            if (result is IAssemblyMetadata)
                            {
                                IAssemblyMetadata graph = new ModelMapper().Map(
                                    root: result as IAssemblyMetadata,
                                    model: typeof(AssemblyMetadata).Assembly
                                    );
                                ObjectsList.Clear();
                                ObjectsList.Add(new AssemblyItem(graph as AssemblyMetadata));
                                LoadedAssembly = "Model deserialized";
                            }
                        });
                    }
                    catch (IOException ex)
                    {
                        if (Tracing)
                        {
                            Tracer.LogFailure($"Exception caught when trying to open a file for reading (deserialization){Environment.NewLine}{ex.Message}");
                            Tracer.Flush();
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
