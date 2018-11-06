using Library.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using TP.GraphicalData.TreeView;
using TPA.Reflection;
using TPA.Reflection.Model;

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
                OnPropertyChanged("ObjectSelected");
                //Messenger.Default.Send(new SelectedChangedMessage(currentlySelected)); TODO MESSENGER PATTERN
            }
        }
        public TreeViewItem PreviousSelection { get; private set; }
        public string LoadedAssembly { get; set; }
        public AssemblyMetadata LoadedAssemblyRepresentation { get; private set; }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public ClassPresenter()
        {
            ShowCurrentObject = new RelayCommand(ChangeClassToDisplay, () => ObjectSelected != null);
            ObjectsList = new ObservableCollection<TreeViewItem>() { null };
            ReloadAssemblyCommand = new RelayCommand(ReloadAssembly);
        }

        private void LoadAssembly()
        {
            Reflector reflector = new Reflector(LoadedAssembly);
            LoadedAssemblyRepresentation = reflector.m_AssemblyModel;
        }

        private void ReloadAssembly()
        {
            LoadAssembly();
            ObjectsList.Clear();
            if(LoadedAssemblyRepresentation != null)
            {
                TreeViewItem item = new TreeViewItem(LoadedAssemblyRepresentation);
                ObjectsList.Add(item);
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
