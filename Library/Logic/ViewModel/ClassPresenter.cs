using Library.Data;
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

        public IRepresentation ClassSelected
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

        public ObservableCollection<IRepresentation> ClassesList { get; }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public ClassPresenter()
        {
            ShowCurrentClass = new RelayCommand(ChangeClassToDisplay, () => ClassSelected != null);
            ClassesList = new ObservableCollection<IRepresentation>() { null };
            string tmp = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\test.dll";
            LoadDllTestsClass.LoadAssembly(tmp);
            var types = LoadDllTestsClass.MemberTypes();
            foreach (var type in types)
                ClassesList.Add(type);
        }

        public void ChangeClassToDisplay()
        {
            ClassToDisplay = ClassSelected;
        }

        private void PopulateChildren(string FullName)
        {

        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
