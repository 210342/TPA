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

namespace Library.Logic.ViewModel
{
    public class ClassPresenter : INotifyPropertyChanged
    {
        #region Fields
        private IRepresentation objectSelected;

        private IRepresentation ObjectToDisplay;
        #endregion

        #region Properties
        public ICommand ShowCurrentObject { get; }
        public ICommand ReloadAssemblyCommand { get; }


        public ObservableCollection<IRepresentation> ObjectsList { get; }
        public IRepresentation ObjectSelected
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
                //Messenger.Default.Send(new SelectedChangedMessage(currentlySelected)); TODO MESSENGER PATTERN
            }
        }
        public IRepresentation PreviousSelection { get; private set; }
        private string _loadedAssembly;
        public string LoadedAssembly { get
            {
                return _loadedAssembly;
            }
            set
            {
                _loadedAssembly = value;
                OnPropertyChanged();
            }
        }
        public IRepresentation LoadedAssemblyRepresentation { get; private set; }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public ClassPresenter()
        {
            ShowCurrentObject = new RelayCommand(ChangeClassToDisplay, () => ObjectSelected != null);
            ObjectsList = new ObservableCollection<IRepresentation>() { null };
            ReloadAssemblyCommand = new RelayCommand(ReloadAssembly);
        }

        private void LoadAssembly()
        {
            Reflector reflector = new Reflector(LoadedAssembly);
            LoadedAssemblyRepresentation = reflector.AssemblyModel;
        }

        private void ReloadAssembly()
        {
            LoadAssembly();
            ObjectsList.Clear();
            if(LoadedAssemblyRepresentation != null)
            {
                ObjectsList.Add(LoadedAssemblyRepresentation);
                ObjectSelected = LoadedAssemblyRepresentation;
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

        public void InteractWithTreeItem(IRepresentation item)
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
        }
    }
}
