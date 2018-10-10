using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Library.Logic.ViewModel
{
    public class ClassPresenter : INotifyPropertyChanged
    {
        public ICommand ShowCurrentClass { get; }
        public ICommand ShowClassesList { get; }

        private ClassRepresentation classSelected;

        private ClassRepresentation ClassToDisplay;
        public ClassRepresentation ClassSelected {
            get
            {
                return classSelected;
            }
            set
            {
                classSelected = value;
                OnPropertyChanged("SelectedClass");
                //Messenger.Default.Send(new SelectedChangedMessage(currentlySelected)); TODO MESSENGER PATTERN
            }
        }
        public ObservableCollection<ClassRepresentation> ClassesList { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ClassPresenter()
        {
            ShowCurrentClass = new RelayCommand(ChangeClassToDisplay, () => ClassSelected != null);
            ShowClassesList = new RelayCommand(ReloadOrSetClassList, () => ClassesList == null);
            ClassesList = new ObservableCollection<ClassRepresentation>();
            ClassesList.Add(new ClassRepresentation("xd", null, null, null, null));
            ClassesList.Add(new ClassRepresentation("xd1", null, null, null, null));
            ClassesList.Add(new ClassRepresentation("xd2", null, null, null, null));
        }

        private void ChangeClassToDisplay()
        {
            ClassToDisplay = ClassSelected;
        }
        private void ReloadOrSetClassList()
        {
            //ISSUE HERE:
            /* We will have to implement way to load classes into observable collection
             * but before that proper loading, so we won't have to reload every time
             */

            ClassesList.Add(new ClassRepresentation("xd", null, null, null, null));
            ClassesList.Add(new ClassRepresentation("xd1", null, null, null, null));
            ClassesList.Add(new ClassRepresentation("xd2", null, null, null, null));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
