using Library.Data;
using System;
using System.Linq;
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

        public IRepresentation PreviousSelection { get; private set; }

        public ObservableCollection<IRepresentation> ObjectsList { get; }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public ClassPresenter()
        {
            ShowCurrentClass = new RelayCommand(ChangeClassToDisplay, () => ClassSelected != null);
            ObjectsList = new ObservableCollection<IRepresentation>() { null };
            string tmp = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\test.dll";
            Reflector reflector = new Reflector(tmp);
            ObjectsList.Add(reflector.AssemblyModel);
            ObjectsList.Remove(null);
            ClassSelected = ObjectsList[0];
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
