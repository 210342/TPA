using Library.Data;
using System;
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
        private IRepresentation classSelected;

        private IRepresentation ClassToDisplay;
        #endregion

        #region Properties
        public ICommand ShowCurrentClass { get; }
        public ICommand ReloadAssemblyCommand { get; }

        public IRepresentation ClassSelected
        {
            get
            {
                return classSelected;
            }
            set
            {
                PreviousSelection = classSelected;
                classSelected = value;
                OnPropertyChanged("ClassSelected");
                //Messenger.Default.Send(new SelectedChangedMessage(currentlySelected)); TODO MESSENGER PATTERN
            }
        }

        public string LoadedAssembly { get; set; }
        public IRepresentation LoadedAssemblyRepresentation { get; private set; }

        public ObservableCollection<IRepresentation> ClassesList { get; }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public ClassPresenter()
        {
            ShowCurrentClass = new RelayCommand(ChangeClassToDisplay, () => ClassSelected != null);
            ClassesList = new ObservableCollection<IRepresentation>() { null };
            ReloadAssemblyCommand = new RelayCommand(RelodAssembly);
        }

        private void LoadAssembly()
        {
            Reflector reflector = new Reflector(LoadedAssembly);
            this.LoadedAssemblyRepresentation = reflector.AssemblyModel;
        }

        private void RelodAssembly()
        {
            LoadAssembly();
            this.ClassesList.Clear();
            if(this.LoadedAssemblyRepresentation != null)
                this.ClassesList.Add(this.LoadedAssemblyRepresentation);
        }

        public void ChangeClassToDisplay()
        {
            ClassToDisplay = ClassSelected;
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
