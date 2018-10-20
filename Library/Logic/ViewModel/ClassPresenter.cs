using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
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

            /*
            string className, List< string > classProp, List<string> classAtt,
              List< string > classMeth, List<string> classFields */



            var human = 
                new ClassRepresentation("Human",
                new List<string>() {"Heart heart", "Kidney kidney", "Human loveInterest" },
                new List<string>() { "private int size", "private int age" },
                new List<string>() { "public void Walk()", "public void Run()" }, 
                null);
            var heart = new ClassRepresentation("Heart", null, null, null, null);
            var kidney = new ClassRepresentation("Kidney", null, null, null, null);

            human.AddAReference(heart);
            human.AddAReference(kidney);
            human.AddAReference(human);

            ClassesList.Add(heart);
            ClassesList.Add(human);
            ClassesList.Add(kidney);
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
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
