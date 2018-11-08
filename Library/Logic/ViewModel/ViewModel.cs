using Library.Logic.TreeView;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TPA.Reflection;
using TPA.Reflection.Model;
using Tracing;

namespace Library.Logic.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region Fields
        private TreeViewItem objectSelected;

        private TreeViewItem ObjectToDisplay;
        #endregion

        #region Properties
        public ICommand ShowCurrentObject { get; }
        public ICommand ReloadAssemblyCommand { get; }


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
                Trace.TraceInformation("ObjectSelected changed.");
                Trace.Flush();
                OnPropertyChanged("ObjectSelected");
            }
        }
        public TreeViewItem PreviousSelection { get; private set; }
        private string _loadedAssembly;
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
        public AssemblyMetadata LoadedAssemblyRepresentation { get; private set; }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel()
        {
            Trace.Listeners.Add(new DbTraceListener(@".\connConfig.xml"));
            Trace.Flush();
            ShowCurrentObject = new RelayCommand(ChangeClassToDisplay, () => ObjectSelected != null);
            ObjectsList = new ObservableCollection<TreeViewItem>() { null };
            ReloadAssemblyCommand = new RelayCommand(ReloadAssembly);
        }

        private void LoadAssembly()
        {
            Trace.TraceInformation("Assembly loading.");
            Trace.Flush();
            Reflector reflector = new Reflector(LoadedAssembly);
            LoadedAssemblyRepresentation = reflector.m_AssemblyModel;
        }

        private void ReloadAssembly()
        {
            LoadAssembly();
            ObjectsList.Clear();
            if (LoadedAssemblyRepresentation != null)
            {
                TreeViewItem item = new TreeViewItem(LoadedAssemblyRepresentation);
                ObjectsList.Add(item);
                ObjectSelected = item;
            }
            Trace.TraceInformation("Assembly reloaded.");
            Trace.Flush();
        }

        public void ChangeClassToDisplay()
        {
            ObjectToDisplay = ObjectSelected;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
