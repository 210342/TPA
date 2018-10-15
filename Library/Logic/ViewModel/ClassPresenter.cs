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

        public ClassRepresentation ClassSelected
        {
            get
            {
                return classSelected;
            }
            set
            {
                classSelected = value;
                OnPropertyChanged("ClassSelected");
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
            var tmp = new ClassRepresentation("xd", null, null, null, null);
            var tmp2 = new ClassRepresentation("xd2", null, null, null, null);
            var tmp3 = new ClassRepresentation("xd3", null, null, null, null);

            tmp.AddAReference(tmp2);
            tmp.AddAReference(tmp3);
            tmp.AddAReference(tmp);

            ClassesList.Add(tmp2);
            ClassesList.Add(tmp);
            ClassesList.Add(tmp3);
        }

        public void ChangeClassToDisplay()
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
