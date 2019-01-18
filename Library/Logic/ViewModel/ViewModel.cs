using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Command;
using Library.Data;
using Library.Logic.MEFProviders;
using Library.Logic.MEFProviders.Exceptions;
using Library.Model;
using ModelContract;
using Persistance;
using Tracing;

namespace Library.Logic.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel() : this(true)
        {
        }

        public ViewModel(bool tracing)
        {
            IsTracingEnabled = tracing;
            LoadedAssemblyRepresentation = new AssemblyMetadata(Assembly.GetAssembly(GetType()));
            ShowCurrentObject = new RelayCommand(ChangeClassToDisplay, () => ObjectSelected != null);
            ObjectsList = new ObservableCollection<TreeViewItem> {null};
            ReloadAssemblyCommand = new RelayCommand(ReloadAssembly);
            OpenFileCommand = new RelayCommand(() => OpenFile(OpenFileSourceProvider));
            SaveModel = new RelayCommand(() => Save(SaveFileSourceProvider),
                () => Persister != null && !(ObjectsList.First() is null));
            LoadModel = new RelayCommand(() => Load(OpenFileSourceProvider), () => Persister != null);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void EndInit()
        {
            if (IsTracingEnabled) ImportTracer();
            ImportPersister();
            SaveModel.RaiseCanExecuteChanged();
            LoadModel.RaiseCanExecuteChanged();
        }

        private void ImportTracer()
        {
            tracingProvider = new TracingProvider(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            try
            {
                if (Tracer is null) Tracer = tracingProvider.ProvideTracer();
            }
            catch (MEFLoaderException ex)
            {
                ErrorMessageTarget.SendMessage("MEF composition error", ex.Message);
                ErrorMessageTarget.CloseApp();
                IsTracingEnabled = false;
            }
        }

        private void ImportPersister()
        {
            persistenceProvider = new PersistenceProvider(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            try
            {
                if (Persister is null) Persister = persistenceProvider.ProvidePersister();
            }
            catch (MEFLoaderException ex)
            {
                if (IsTracingEnabled)
                    Tracer.LogFailure("Persister loading failure. " +
                                      $"{ex.Message}  {ex.StackTrace} " +
                                      $"{ex.InnerException?.Message} {ex.InnerException?.StackTrace}");
                ErrorMessageTarget.SendMessage("MEF composition error", ex.Message);
                ErrorMessageTarget.CloseApp();
            }
        }

        private void LoadAssembly()
        {
            if (IsTracingEnabled)
            {
                Tracer.LogLoadingModel(LoadedAssembly);
                Tracer.Flush();
            }
            try
            {
                Reflector reflector = new Reflector(LoadedAssembly);
                LoadedAssemblyRepresentation = reflector.AssemblyModel;

                if (IsTracingEnabled)
                {
                    Tracer.LogModelLoaded(LoadedAssembly);
                    Tracer.Flush();
                }
            }
            catch(BadImageFormatException ex)
            {
                if(IsTracingEnabled)
                {
                    Tracer.LogFailure($"Failed when reading assembly {ex.Message}");
                    Tracer.Flush();
                }
                ErrorMessageTarget.SendMessage("Library reading error", ex.Message);
            }
        }

        private void ReloadAssembly()
        {
            LoadAssembly();
            ObjectsList.Clear();
            if (LoadedAssemblyRepresentation != null)
            {
                TreeViewItem item = new AssemblyItem((AssemblyMetadata) LoadedAssemblyRepresentation);
                ObjectsList.Add(item);
                SaveModel.RaiseCanExecuteChanged();
                ObjectSelected = item;
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

        private async void Save(ISourceProvider targetPathProvider)
        {
            if (targetPathProvider == null)
                throw new ArgumentNullException(nameof(targetPathProvider), "SourceProvider can't be null.");

            if (Persister.FileSystemDependency == FileSystemDependency.Independent)
                targetPathProvider = new NullSourceProvider();

            if (targetPathProvider.GetAccess())
                try
                {
                    string target = targetPathProvider.GetPath();
                    await Task.Run(() =>
                    {
                        if (IsTracingEnabled)
                        {
                            Tracer.LogSavingModel(target);
                            Tracer.Flush();
                        }

                        (ObjectsList.First().ModelObject as AssemblyMetadata)?.Save(Persister, target);
                        InformationMessageTarget.SendMessage("Saving completed", "Model was successfully saved.");

                        if (IsTracingEnabled)
                        {
                            Tracer.LogModelSaved(target);
                            Tracer.Flush();
                        }
                    });
                }
                catch (Exception ex)
                {
                    ErrorMessageTarget.SendMessage("Saving error", ex.Message);
                    if (IsTracingEnabled)
                    {
                        Tracer.LogFailure(
                            $"Exception caught when trying to open a file for writing {Environment.NewLine}{ex.Message}");
                        Tracer.Flush();
                    }
                }
        }

        private void Load(ISourceProvider targetPathProvider)
        {
            if (targetPathProvider == null)
                throw new ArgumentNullException(nameof(targetPathProvider), "SourceProvider can't be null.");

            if (Persister.FileSystemDependency == FileSystemDependency.Independent)
                targetPathProvider = new NullSourceProvider();

            if (targetPathProvider.GetAccess())
            {
                try
                {
                    string target = targetPathProvider.GetPath();
                    if (SynchronizationContext.Current is null)
                    {
                        LoadRootAssembly(target);
                    }
                    else
                    {
                        Dispatcher.CurrentDispatcher.BeginInvoke((Action<string>)LoadRootAssembly, target);
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessageTarget.SendMessage("Loading error", ex.Message);
                    if (IsTracingEnabled)
                    {
                        Tracer.LogFailure(
                            $"Exception caught when trying to open a file for reading {Environment.NewLine}{ex.Message}");
                        Tracer.Flush();
                    }
                }
            }
            else
            {
                ErrorMessageTarget.SendMessage("Target in use", "File you tried to open is currently in use by another program");
            }
        }

        private void LoadRootAssembly(string target)
        {
            if (IsTracingEnabled)
            {
                Tracer.LogLoadingModel(target);
                Tracer.Flush();
            }

            AssemblyMetadata graph = null;
            try
            {
                graph = AssemblyMetadata.Load(Persister, target);
            }
            catch (Exception e)
            {
                string errorMessage = $"Error during retrieval of elements from repository. {e.Message}";
                ErrorMessageTarget.SendMessage("Loading error", errorMessage);
                if (IsTracingEnabled)
                {
                    Tracer.LogFailure($"{target}; {errorMessage}");
                    Tracer.Flush();
                }
            }

            if (graph != null)
            {
                ObjectsList.Clear();
                ObjectsList.Add(new AssemblyItem(graph));
                LoadedAssembly = "Model loaded from repository";
                SaveModel.RaiseCanExecuteChanged();
                Persister.Dispose();
                InformationMessageTarget?.SendMessage("Loading completed", "Model was successfully loaded.");

                if (IsTracingEnabled)
                {
                    Tracer.LogModelLoaded(target);
                    Tracer.Flush();
                }
            }
        }

        private void OpenFile(ISourceProvider sourceProvider)
        {
            if (sourceProvider == null)
                throw new ArgumentNullException(nameof(sourceProvider), "SourceProvider can't be null.");
            if (sourceProvider.GetAccess())
            {
                LoadedAssembly = sourceProvider.GetPath();
                ReloadAssemblyCommand.Execute(null);
            }
        }

        #region Fields

        private TreeViewItem objectSelected;
        private string _loadedAssembly;
        private TreeViewItem _objectToDisplay;
        private PersistenceProvider persistenceProvider;
        private TracingProvider tracingProvider;

        #endregion

        #region Properties

        public ITracing Tracer { get; private set; }
        public IPersister Persister { get; private set; }
        public ICommand ShowCurrentObject { get; }
        public ICommand ReloadAssemblyCommand { get; }
        public ICommand OpenFileCommand { get; }

        public ICommand AppClosing
        {
            get
            {
                return new RelayCommand<CancelEventArgs>(
                    args =>
                    {
                        if (Tracer != null)
                        {
                            Tracer.LogSuccess("shutting down system");
                            Tracer.Flush();
                        }

                        try
                        {
                            Persister.Dispose();
                            Tracer?.Dispose();
                            persistenceProvider.Dispose();
                            tracingProvider.Dispose();
                        }
                        catch (Exception e)
                        {
                            Tracer?.LogFailure($"Caught and exception during application closing process. {e.Message}");
                            Tracer?.Flush();
                        }
                        
                    });
            }
        }

        public RelayCommand SaveModel { get; }
        /// <summary>
        /// This command checks whether SynchronizationContext is null;
        /// if it is, it runs synchronously
        /// </summary>
        public RelayCommand LoadModel { get; }
        public ISourceProvider OpenFileSourceProvider { get; set; }
        public ISourceProvider SaveFileSourceProvider { get; set; }
        public IErrorFlushTarget ErrorMessageTarget { get; set; }
        public IInformationMessageTarget InformationMessageTarget { get; set; }
        public bool IsTracingEnabled { get; set; }
        public ObservableCollection<TreeViewItem> ObjectsList { get; }

        public TreeViewItem ObjectSelected
        {
            get => objectSelected;
            set
            {
                PreviousSelection = objectSelected;
                objectSelected = value;
                OnPropertyChanged();
            }
        }

        public TreeViewItem ObjectToDisplay
        {
            get => _objectToDisplay;
            set
            {
                _objectToDisplay = value;
                OnPropertyChanged();
            }
        }

        public TreeViewItem PreviousSelection { get; private set; }

        public string LoadedAssembly
        {
            get => _loadedAssembly;
            set
            {
                _loadedAssembly = value;
                OnPropertyChanged();
            }
        }

        public IMetadata LoadedAssemblyRepresentation { get; private set; }

        #endregion
    }
}