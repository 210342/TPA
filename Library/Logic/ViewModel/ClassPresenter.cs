using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TP.GraphicalData.TreeView;
using TPA.Reflection;
using TPA.Reflection.Model;
using Tracing;

namespace Library.Logic.ViewModel
{
    public class ClassPresenter : INotifyPropertyChanged
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
                //Messenger.Default.Send(new SelectedChangedMessage(currentlySelected)); TODO MESSENGER PATTERN
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

        public ClassPresenter()
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

        /*public void InteractWithTreeItem(IRepresentation item)
        {
            if (item != null)
            {
                if (ObjectsList.Contains(item))
                {
                    if(item.Children.Count() != 0)
                    {
                        if (ObjectsList.Contains(item.Children.First()))
                        {
                            CloseTreeItem(item);
                        }
                        else
                        {
                            OpenTreeItem(item);
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("This item isn't on the list");
                }
            }
            else
            {
                throw new ArgumentNullException("Item cannot be null");
            }
        }

        private void OpenTreeItem(IRepresentation item)
        {
            foreach (IRepresentation kid in item.Children)
            {
                ObjectsList.Add(kid);
            }
        }
        private void CloseTreeItem(IRepresentation item)
        {
            foreach(IRepresentation kid in item.Children)
            {
                if(kid.Children.Count() != 0) // if kid has children
                {
                    if(ObjectsList.Contains(kid.Children.First())) // check if his children are on the list
                    {
                        CloseTreeItem(kid); // close children recursively if opened
                    }
                }
                ObjectsList.Remove(kid);
            }
        */
    }
}
